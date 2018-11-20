using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait, move
}


public class BoardCtrl : MonoBehaviour {

    public GameState currentState = GameState.move;

    public int width;   // 가로 블록 개수
    public int height;  // 세로 블록 개수
    public int offSet;  // 초기 시작점과의 거리를 두기 위함

    //public GameObject[] hexagons;
    public GameObject blockPrefab;
    public BlockCtrl[,] blockList;
    //public GameObject[,] allHexagons;

    public GameObject destroyEffect;

    //private FindMatchesCtrl findMatches;

    // Use this for initialization
    void Start()
    {
        blockList = new BlockCtrl[width, height]; // 크기만큼 동적할당

        InitializeBlock();
        //findMatches = FindObjectOfType<FindMatchesCtrl>();
        //allHexagons = new GameObject[width, height];
        //SetUp();
    }

    public void InitializeBlock()
    {
        for(int i=0;i<width; ++i)
        { 
            for(int j=0;j<height;++j)
            {
                Vector2 startPosition = new Vector2(i, j);
                GameObject block = Instantiate(blockPrefab, startPosition, Quaternion.identity);              
                block.GetComponent<BlockCtrl>().SetData(i, j);
                block.transform.parent = this.transform;
                block.name = "block";
                blockList[i, j] = block.GetComponent<BlockCtrl>();
            }
        }

    }
    //private void SetUp()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            Vector2 startPosition = new Vector2(i, j+ offSet);  // 초기 위치를 잡아준다.

    //            int useToHexagon = Random.Range(0, hexagons.Length);

    //            int maxIterations = 0;
    //            while (MatchesAt(i, j, hexagons[useToHexagon]) && maxIterations < 100)
    //            {
    //                useToHexagon = Random.Range(0, hexagons.Length);
    //                maxIterations++;
    //                Debug.Log(maxIterations);
    //            }
    //            maxIterations = 0;

    //            GameObject hexagon = Instantiate(hexagons[useToHexagon], startPosition, Quaternion.identity);
    //            hexagon.GetComponent<HexagonCtrl>().col = i;
    //            hexagon.GetComponent<HexagonCtrl>().row = j;

    //            hexagon.transform.parent = this.transform;
    //            hexagon.name = "(" + i + "," + j + ") Hexagon";
    //            allHexagons[i, j] = hexagon;
    //        }
    //    }
    //}

    //private bool MatchesAt(int col, int row, GameObject block) // 초기 생성때 3 매치된 블록 판별
    //{
    //    if (col > 1 && row > 1)
    //    {
    //        if (blockList[col - 1, row].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber 
    //            && blockList[col - 2, row].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
    //        {
    //            return true;
    //        }
    //        if (blockList[col, row - 1].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber 
    //            && blockList[col, row - 2].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
    //        {
    //            return true;
    //        }
    //    }
    //    else if (col <= 1 || row <= 1)
    //    {
    //        if (row > 1)
    //        {
    //            if (blockList[col, row - 1].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber 
    //                && blockList[col, row - 2].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
    //            {
    //                return true;
    //            }
    //        }
    //        if (col > 1)
    //        {
    //            if (blockList[col - 1, row].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber 
    //                && blockList[col - 2, row].currentColorNumber == block.GetComponent<BlockCtrl>().currentColorNumber)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    //public void DestroyMatchesAt(int col, int row)
    //{
    //    if (allHexagons[col, row].GetComponent<HexagonCtrl>().isMatched)
    //    {
    //        findMatches.currentMatches.Remove(allHexagons[col, row]);
    //        GameObject particle = Instantiate(destroyEffect, allHexagons[col, row].transform.position, Quaternion.identity);
    //        Destroy(particle,.5f);
    //        Destroy(allHexagons[col, row]);
    //        allHexagons[col, row] = null;
    //    }
    //}

    // 코드 수정해야함, Destroy 대신 재사용하기 위함
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

    // 헥사곤이 3 매치되어 없어진후 나머지가 내려오기 위함
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
    //            } else if (nullCount > 0)
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

    // 빈칸에 다시 채워넣는 함수
    //private void RefillBoard()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (allHexagons[i, j] == null)
    //            {
    //                Vector2 tempPosition = new Vector2(i, j+offSet);
    //                int useToHexagon = Random.Range(0, hexagons.Length);
    //                GameObject hexagon = Instantiate(hexagons[useToHexagon], tempPosition, Quaternion.identity);
    //                allHexagons[i, j] = hexagon;
    //                hexagon.GetComponent<HexagonCtrl>().col = i;
    //                hexagon.GetComponent<HexagonCtrl>().row = j;

    //            }
    //        }
    //    }
    //}

    // 채워진 보드에 다시 매치되는 블럭 판단
    //private bool MatchesOnBoard()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for(int j = 0; j < height; j++)
    //        {
    //            if (allHexagons[i, j] != null)
    //            {
    //                if(allHexagons[i,j].GetComponent<HexagonCtrl>().isMatched)
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    // 재귀 함수처럼 내려오고 매치될 블록이 없을 때까지 돈다.
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
