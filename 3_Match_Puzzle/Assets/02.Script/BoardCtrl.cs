using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCtrl : MonoBehaviour {

    public int width;
    public int height;

    public GameObject tilePrefab;
    private BackgroundCtrl[,] allTiles;

    public GameObject[] hexagons;

    public GameObject[,] allHexagons;

    // Use this for initialization
    void Start()
    {
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
                Vector2 startPosition = new Vector2(i, j);  // 초기 위치를 잡아준다.
                GameObject backgroundTile = Instantiate(tilePrefab, startPosition, Quaternion.identity) as GameObject; // 생성할때 값 설정
                backgroundTile.transform.parent = this.transform; // 그후 Board 오브젝트에 넣기
                backgroundTile.name = "(" + i + "," + j + ") Tile";

                int useToHexagon = Random.Range(0, hexagons.Length);
                GameObject hexagon = Instantiate(hexagons[useToHexagon], startPosition, Quaternion.identity);
                hexagon.transform.parent = this.transform;
                hexagon.name = "(" + i + "," + j + ") Hexagon";
                allHexagons[i, j] = hexagon;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
