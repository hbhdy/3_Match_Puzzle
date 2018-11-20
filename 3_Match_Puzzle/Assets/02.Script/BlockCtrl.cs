using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockColor
{
    Black,
    Blue,
    Green,
    Orange,
    Pink,
    Red,
    Yellow
}

public class BlockCtrl : MonoBehaviour {

    //private BlockColor currentBlockColor;

    public Sprite[] colorSprite;
    public int column; 
    public int row;
    public int currentColorNumber;
    // Use this for initialization
    void Start()
    {
        currentColorNumber = Random.Range(0, colorSprite.Length);

        //int useToHexagon = Random.Range(0, hexagons.Length);
        //Random.Range(0,BlockColor())
        gameObject.GetComponent<SpriteRenderer>().sprite = colorSprite[currentColorNumber];

    }

    public void SetColor()
    {

    }

    public void SetData(int x,int y)
    {
        column = x;
        row = y;
    }
}
