﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait, move
}


public class BoardCtrl : MonoBehaviour
{
    public GameState currentState = GameState.move;

    [Header("Board Settings")]
    public int width;   // 가로 블록 개수
    public int height;  // 세로 블록 개수
    public int offSet;  // 초기 시작점과의 거리를 두기 위함

    [Header("Object Settings")]
    public GameObject blockPrefab;
    public GameObject[,] blockList;
    public GameObject destroyEffect;
    public GameObject[] tempOjbect;

    private FindMatchesCtrl findMatches;

    public int initBlockColorNumber;

    public int tempnumber = 0;
  

    void Start()
    {
        blockList = new GameObject[width, height]; // 크기만큼 2차원 배열 동적할당   
        findMatches = FindObjectOfType<FindMatchesCtrl>();
        InitializeBlock();

        tempOjbect = new GameObject[12];
    }

    // 블록 초기화 및 생성
    private void InitializeBlock()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                initBlockColorNumber = Random.Range(0, Values.maxColorRange);
                Vector2 startPosition = new Vector2(i, j + offSet);  // 초기 위치를 잡아준다.
                int maxCycle = 0;

                GameObject block = Instantiate(blockPrefab, startPosition, Quaternion.identity);
                block.GetComponent<BlockCtrl>().ChangeColor(initBlockColorNumber);
                block.GetComponent<BlockCtrl>().column = i;
                block.GetComponent<BlockCtrl>().row = j;
                block.transform.parent = this.transform;
                block.name = "block" + i + ' ' + j;

                while (InitMatches(i, j, block) && maxCycle < 100)
                {
                    initBlockColorNumber = Random.Range(0, Values.maxColorRange);
                    maxCycle++;
                    block.GetComponent<BlockCtrl>().ChangeColor(initBlockColorNumber);
                    Debug.Log(maxCycle);
                }
                blockList[i, j] = block;

            }
        }
    }

    // 초기 생성때 3 매치된 블록 판별
    private bool InitMatches(int col, int row, GameObject block)
    {
        if (col > 1 && row > 1)
        {
            if (blockList[col - 1, row] != null && blockList[col - 2, row] != null)
            {
                if (blockList[col - 1, row].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber
                    && blockList[col - 2, row].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
                {
                    return true;
                }
            }
            if (blockList[col, row - 1] != null && blockList[col, row - 2] != null)
            {
                if (blockList[col, row - 1].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber
                    && blockList[col, row - 2].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
                {
                    return true;
                }
            }
        }
        else if (col <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (blockList[col, row - 1] != null && blockList[col, row - 2] != null)
                {
                    if (blockList[col, row - 1].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber
                    && blockList[col, row - 2].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
                    {
                        return true;
                    }
                }
            }
            if (col > 1)
            {
                if (blockList[col - 1, row] != null && blockList[col - 2, row] != null)
                {
                    if (blockList[col - 1, row].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber
                    && blockList[col - 2, row].GetComponent<BlockCtrl>().currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // 블록 Change의 첫함수
    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blockList[i, j].GetComponent<BlockCtrl>().isMatched)
                {
                    blockList[i, j].GetComponent<BlockCtrl>().BlockScaleDown();

                    GameObject particle = Instantiate(destroyEffect, blockList[i, j].transform.position, Quaternion.identity);
                    Destroy(particle, .5f);
                    tempOjbect[tempnumber++] = blockList[i, j];
                    blockList[i, j] = null;
                }
            }
        }
        tempnumber = 0;
        // 나머지 블록을 내리기 위한 코루틴 설정
        StartCoroutine(DecreaseRowCoroutine());
    }

    //블록이 3 매치되어 없어진후 나머지가 내려오기 위함
    private IEnumerator DecreaseRowCoroutine()
    {
        // 재사용할 블럭 개수를 세고 그만큼 나머지 블록의 위치를 조정하기 위한 변수
        int changeBlockCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blockList[i, j] == null)
                {
                    changeBlockCount++;
                }
                else if (changeBlockCount > 0)
                {
                    blockList[i, j - changeBlockCount] = blockList[i, j];
                    blockList[i, j - changeBlockCount].GetComponent<BlockCtrl>().row -= changeBlockCount;
                    blockList[i, j] = null;
                }
            }
            changeBlockCount = 0;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FillBoardCoroutine());
    }

    //빈칸에 다시 채워넣는 함수
    private void RefillBoard()
    {
        SoundManager.instance.PlaySound();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blockList[i, j] == null)        
                {

                    blockList[i, j] = tempOjbect[tempnumber++];
                    blockList[i, j].GetComponent<BlockCtrl>().isMatched = false;
                    initBlockColorNumber = Random.Range(0, Values.maxColorRange);
                    Vector2 reStartPosition = new Vector2(i, j + offSet);  // 초기 위치를 잡아준다.
                    blockList[i, j].GetComponent<BlockCtrl>().BlockScaleUp();
                    blockList[i, j].GetComponent<BlockCtrl>().ChangeColor(initBlockColorNumber);
                    blockList[i, j].GetComponent<BlockCtrl>().column = i;
                    blockList[i, j].GetComponent<BlockCtrl>().row = j;
                }
            }
        }
        tempnumber = 0;
    }

    //채워진 보드에 다시 매치되는 블록 판단
    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blockList[i, j] != null)              
                {
                    if (blockList[i, j].GetComponent<BlockCtrl>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // 내려온 블록이 다시 매치되지 않을 때까지 돈다.
    private IEnumerator FillBoardCoroutine()
    {
        RefillBoard();
        yield return new WaitForSeconds(1.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }

}
