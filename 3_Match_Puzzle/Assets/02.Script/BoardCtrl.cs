using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait, move
}


public class BoardCtrl : MonoBehaviour {

    public GameState currentState = GameState.move;

    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    private BackgroundCtrl[,] allTiles;
    public GameObject[] hexagons;
    public GameObject[,] allHexagons;

    public GameObject destroyEffect;

    private FindMatches findMatches;

    // Use this for initialization
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new BackgroundCtrl[width, height];
        allHexagons = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 startPosition = new Vector2(i, j+ offSet);  // 초기 위치를 잡아준다.
                GameObject backgroundTile = Instantiate(tilePrefab, startPosition, Quaternion.identity) as GameObject; // 생성할때 값 설정
                backgroundTile.transform.parent = this.transform; // 그후 Board 오브젝트에 넣기
                backgroundTile.name = "(" + i + "," + j + ") Tile";

                int useToHexagon = Random.Range(0, hexagons.Length);

                int maxIterations = 0;
                while (MatchesAt(i, j, hexagons[useToHexagon]) && maxIterations < 100)
                {
                    useToHexagon = Random.Range(0, hexagons.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }
                maxIterations = 0;

                GameObject hexagon = Instantiate(hexagons[useToHexagon], startPosition, Quaternion.identity);
                hexagon.GetComponent<HexagonCtrl>().col = i;
                hexagon.GetComponent<HexagonCtrl>().row = j;

                hexagon.transform.parent = this.transform;
                hexagon.name = "(" + i + "," + j + ") Hexagon";
                allHexagons[i, j] = hexagon;
            }
        }
    }

    private bool MatchesAt(int col, int row, GameObject Hexagon) // 초기 생성때 3 매치된 헥사곤 판별
    {
        if (col > 1 && row > 1)
        {
            if (allHexagons[col - 1, row].tag == Hexagon.tag && allHexagons[col - 2, row].tag == Hexagon.tag)
            {
                return true;
            }
            if (allHexagons[col, row - 1].tag == Hexagon.tag && allHexagons[col, row - 2].tag == Hexagon.tag)
            {
                return true;
            }
        }
        else if (col <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allHexagons[col, row - 1].tag == Hexagon.tag && allHexagons[col, row - 2].tag == Hexagon.tag)
                {
                    return true;
                }
            }
            if (col > 1)
            {
                if (allHexagons[col - 1, row].tag == Hexagon.tag && allHexagons[col - 2, row].tag == Hexagon.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void DestroyMatchesAt(int col, int row)
    {
        if (allHexagons[col, row].GetComponent<HexagonCtrl>().isMatched)
        {
            findMatches.currentMatches.Remove(allHexagons[col, row]);
            GameObject particle = Instantiate(destroyEffect, allHexagons[col, row].transform.position, Quaternion.identity);
            Destroy(particle,.5f);
            Destroy(allHexagons[col, row]);
            allHexagons[col, row] = null;
        }
    }

    // 코드 수정해야함, Destroy 대신 재사용하기 위함
    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allHexagons[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCoroutine());
    }

    // 헥사곤이 3 매치되어 없어진후 나머지가 내려오기 위함
    private IEnumerator DecreaseRowCoroutine()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allHexagons[i, j] == null)
                {
                    nullCount++;
                } else if (nullCount > 0)
                {
                    allHexagons[i, j].GetComponent<HexagonCtrl>().row -= nullCount;
                    allHexagons[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCoroutine());
    }

    // 빈칸에 다시 채워넣는 함수
    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allHexagons[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j+offSet);
                    int useToHexagon = Random.Range(0, hexagons.Length);
                    GameObject hexagon = Instantiate(hexagons[useToHexagon], tempPosition, Quaternion.identity);
                    allHexagons[i, j] = hexagon;
                    hexagon.GetComponent<HexagonCtrl>().col = i;
                    hexagon.GetComponent<HexagonCtrl>().row = j;

                }
            }
        }
    }

    // 채워진 보드에 다시 매치되는 블럭 판단
    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allHexagons[i, j] != null)
                {
                    if(allHexagons[i,j].GetComponent<HexagonCtrl>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // 재귀 함수처럼 내려오고 매치될 블록이 없을 때까지 돈다.
    private IEnumerator FillBoardCoroutine()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }

}
