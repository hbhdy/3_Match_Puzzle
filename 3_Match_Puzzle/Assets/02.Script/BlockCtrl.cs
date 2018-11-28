﻿using System.Collections;
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
    [Header("Board Variables")]
    public Sprite[] colorSprite;
    public int column; 
    public int row;
    public int currentColorNumber;
    public int previousCol;
    public int previuseRow;
   
    public bool isMatched = false;

    //private FindMatchesCtrl findMatches;

    private BoardCtrl board;
    private GameObject otherHexagon;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition;

    [Header("Swipe Stuff")]
    public float swipeAngle = 0f;
    public float swipeResist = 0.1f;


    // Use this for initialization
    void Start()
    {
        previousCol = column;
        previuseRow = row;
        board = FindObjectOfType<BoardCtrl>();
        //currentColorNumber = Random.Range(0, colorSprite.Length
        //Random.Range(0,BlockColor())
        //gameObject.GetComponent<SpriteRenderer>().sprite = colorSprite[currentColorNumber];
    }


    //Update is called once per frame
    void Update()
    {
        //if (isMatched)
        //{
        //    SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        //    mySprite.color = new Color(0f, 0f, 0f, .1f);
        //}

        if (Mathf.Abs(column - transform.position.x) > .1)
        {
            // Move Toward the target
            tempPosition = new Vector2(column, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
            //if (board.blockList[column, row] != this.gameObject)
            //{
            //    board.blockList[column, row] = this.gameObject;
            //}
            //findMatches.FindAllMatches();
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(column, transform.position.y);
            transform.position = tempPosition;
        }

        if (Mathf.Abs(row - transform.position.y) > .1)
        {
            // Move Toward the target
            tempPosition = new Vector2(transform.position.x, row);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
            //if (board.blockList[column, row] != this.gameObject)
            //{
            //    board.blockList[column, row] = this.gameObject;
            //}
            //findMatches.FindAllMatches();

        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, row);
            transform.position = tempPosition;
        }
    }

    public void ChangeColor(int colorNumber)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = colorSprite[colorNumber];
        currentColorNumber = colorNumber;
    }

    public IEnumerator CheckMoveCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        if (otherHexagon != null)
        {
            //if (!isMatched && !otherHexagon.GetComponent<BlockCtrl>().isMatched)
            //{
                otherHexagon.GetComponent<BlockCtrl>().row = row;
                otherHexagon.GetComponent<BlockCtrl>().column = column;
                row = previuseRow;
                column = previousCol;
                yield return new WaitForSeconds(.5f);
                //board.currentState = GameState.move;
            //}
            //else
            //{
            //    board.DestroyMatches();
            //}
            otherHexagon = null;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("클릭 됨");
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //if (board.currentState == GameState.move)
        //{
        //    Debug.Log("클릭 됨");
        //    startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //}
    }

    private void OnMouseUp()
    {
        //if (board.currentState == GameState.move)
        //{
            endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        //}
    }

    void CalculateAngle()
    {
        // 그냥 클릭만으로 헥사곤이 움직이지 않게 저항값과 스와이프 거리 비교
        if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > swipeResist || Mathf.Abs(endTouchPosition.y - startTouchPosition.y) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
            MoveHexagonPieces();
            //board.currentState = GameState.wait;
        }
        //else
        //{
        //    board.currentState = GameState.move;
        //}
    }

    void MoveHexagonPieces()
    {   
        // Right 방향으로 움직임
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            otherHexagon = board.blockList[column + 1, row];
            previousCol = column;
            previuseRow = row;
            otherHexagon.GetComponent<BlockCtrl>().column -= 1;
            column += 1;
        }
        // Up 방향으로 움직임
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            otherHexagon = board.blockList[column, row + 1];
            previousCol = column;
            previuseRow = row;
            otherHexagon.GetComponent<BlockCtrl>().row -= 1;
            row += 1;
        }
        // Left 방향으로 움직임
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            otherHexagon = board.blockList[column - 1, row];
            previousCol = column;
            previuseRow = row;
            otherHexagon.GetComponent<BlockCtrl>().column += 1;
            column -= 1;
        }
        // Down 방향으로 움직임
        else if (swipeAngle >= -135 && swipeAngle < -45 && row > 0)
        {
            otherHexagon = board.blockList[column, row - 1];
            previousCol = column;
            previuseRow = row;
            otherHexagon.GetComponent<BlockCtrl>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCoroutine());
    }

    //void FindMatches()
    //{
    //    if (col > 0 && col < board.width - 1)
    //    {
    //        GameObject leftHexagon1 = board.allHexagons[col - 1, row];
    //        GameObject rightHexagon1 = board.allHexagons[col + 1, row];
    //        if (leftHexagon1 != null && rightHexagon1 != null)
    //        {
    //            if (leftHexagon1.tag == this.gameObject.tag && rightHexagon1.tag == this.gameObject.tag)
    //            {
    //                leftHexagon1.GetComponent<HexagonCtrl>().isMatched = true;
    //                rightHexagon1.GetComponent<HexagonCtrl>().isMatched = true;
    //                isMatched = true;
    //            }
    //        }
    //    }

    //    if (row > 0 && row < board.height - 1)
    //    {
    //        GameObject upHexagon1 = board.allHexagons[col, row + 1];
    //        GameObject downHexagon1 = board.allHexagons[col, row - 1];
    //        if (upHexagon1 != null && downHexagon1 != null)
    //        {
    //            if (upHexagon1.tag == this.gameObject.tag && downHexagon1.tag == this.gameObject.tag)
    //            {
    //                upHexagon1.GetComponent<HexagonCtrl>().isMatched = true;
    //                downHexagon1.GetComponent<HexagonCtrl>().isMatched = true;
    //                isMatched = true;
    //            }
    //        }
    //    }
    //}
}
