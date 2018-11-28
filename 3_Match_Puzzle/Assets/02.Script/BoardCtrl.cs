using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait, move
}


public class BoardCtrl : MonoBehaviour
{

    public GameState currentState = GameState.move;

    public int width;   // 가로 블록 개수
    public int height;  // 세로 블록 개수
    public int offSet;  // 초기 시작점과의 거리를 두기 위함

    //public GameObject[] hexagons;
    public GameObject blockPrefab;
    public GameObject[,] blockList;
    //public GameObject[,] allHexagons;

    public GameObject destroyEffect;

    //private FindMatchesCtrl findMatches;

    public int initBlockColor;

    // Use this for initialization
    void Start()
    {
        blockList = new GameObject[width, height]; // 크기만큼 2차원 배열 동적할당

        //InitializeBlock();
        //findMatches = FindObjectOfType<FindMatchesCtrl>();
        //blockList = new BlockCtrl[width, height];
        InitializeBlock();
    }

    //public void InitializeBlock()
    //{
    //    for (int i = 0; i < width; ++i)
    //    {
    //        for (int j = 0; j < height; ++j)
    //        {
    //            Vector2 startPosition = new Vector2(i, j);
    //            GameObject block = Instantiate(blockPrefab, startPosition, Quaternion.identity);
    //            block.GetComponent<BlockCtrl>().SetData(i, j);
    //            block.transform.parent = this.transform;
    //            block.name = "block";
    //            blockList[i, j] = block.GetComponent<BlockCtrl>();
    //        }
    //    }

    //}
    private void InitializeBlock()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                initBlockColor = Random.Range(0, 7);
                Vector2 startPosition = new Vector2(i, j + offSet);  // 초기 위치를 잡아준다.
                int maxCycle = 0;

                GameObject block = Instantiate(blockPrefab, startPosition, Quaternion.identity);
                block.GetComponent<BlockCtrl>().ChangeColor(initBlockColor);
                block.GetComponent<BlockCtrl>().column = i;
                block.GetComponent<BlockCtrl>().row = j;
                block.transform.parent = this.transform;
                block.name = "(" + i + "," + j + ")";

                while (MatchesAt(i, j, block) && maxCycle < 100)
                {
                    initBlockColor = Random.Range(0, 7);
                    maxCycle++;
                    block.GetComponent<BlockCtrl>().ChangeColor(initBlockColor);
                    Debug.Log(maxCycle);
                }  
                blockList[i, j] = block;
            }
        }
    }

    private bool MatchesAt(int col, int row, GameObject block) // 초기 생성때 3 매치된 블록 판별
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

    //public void DestroyMatchesAt(int col, int row)
    //{
    //    if (allHexagons[col, row].GetComponent<HexagonCtrl>().isMatched)
    //    {
    //        findMatches.currentMatches.Remove(allHexagons[col, row]);
    //        GameObject particle = Instantiate(destroyEffect, allHexagons[col, row].transform.position, Quaternion.identity);
    //        Destroy(particle, .5f);
    //        Destroy(allHexagons[col, row]);
    //        allHexagons[col, row] = null;
    //    }
    //}

    ////코드 수정해야함, Destroy 대신 재사용하기 위함
    //public void DestroyMatches()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (allHexagons[i, j] != null)
    //            {
    //                DestroyMatchesAt(i, j);
    //            }
    //        }
    //    }
    //    StartCoroutine(DecreaseRowCoroutine());
    //}

    ////헥사곤이 3 매치되어 없어진후 나머지가 내려오기 위함
    //private IEnumerator DecreaseRowCoroutine()
    //{
    //    int nullCount = 0;
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (allHexagons[i, j] == null)
    //            {
    //                nullCount++;
    //            }
    //            else if (nullCount > 0)
    //            {
    //                allHexagons[i, j].GetComponent<HexagonCtrl>().row -= nullCount;
    //                allHexagons[i, j] = null;
    //            }
    //        }
    //        nullCount = 0;
    //    }
    //    yield return new WaitForSeconds(.4f);
    //    StartCoroutine(FillBoardCoroutine());
    //}

    ////빈칸에 다시 채워넣는 함수
    //private void RefillBoard()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (allHexagons[i, j] == null)
    //            {
    //                Vector2 tempPosition = new Vector2(i, j + offSet);
    //                int useToHexagon = Random.Range(0, hexagons.Length);
    //                GameObject hexagon = Instantiate(hexagons[useToHexagon], tempPosition, Quaternion.identity);
    //                allHexagons[i, j] = hexagon;
    //                hexagon.GetComponent<HexagonCtrl>().col = i;
    //                hexagon.GetComponent<HexagonCtrl>().row = j;

    //            }
    //        }
    //    }
    //}

    ////채워진 보드에 다시 매치되는 블럭 판단
    //private bool MatchesOnBoard()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (allHexagons[i, j] != null)
    //            {
    //                if (allHexagons[i, j].GetComponent<HexagonCtrl>().isMatched)
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    ////재귀 함수처럼 내려오고 매치될 블록이 없을 때까지 돈다.
    //private IEnumerator FillBoardCoroutine()
    //{
    //    RefillBoard();
    //    yield return new WaitForSeconds(.5f);

    //    while (MatchesOnBoard())
    //    {
    //        yield return new WaitForSeconds(.5f);
    //        DestroyMatches();
    //    }

    //    yield return new WaitForSeconds(.5f);
    //    currentState = GameState.move;
    //}

}
