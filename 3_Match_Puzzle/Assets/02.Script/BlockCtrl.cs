using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum BlockColor
//{
//    Black,
//    Blue,
//    Green,
//    Orange,
//    Pink,
//    Red,
//    Yellow
//}

public class BlockCtrl : MonoBehaviour {

    [Header("Block Variables")]
    public int column; 
    public int row;
    public int previousCol;
    public int previuseRow;
    public int currentColorNumber;
    public bool isMatched = false;  // 블록 일치 판별

    [Header("Block Color Settings")]
    public Sprite[] colorSprite;    // 블록 이미지 모음

    [Header("Swipe Variables")]
    public float swipeAngle = 0f;
    public float swipeResist = 0.1f;


    private FindMatchesCtrl findMatches;
    private BoardCtrl board;
    private GameObject otherBlock;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition;


    void Start()
    {
        previousCol = column;
        previuseRow = row;
        board = FindObjectOfType<BoardCtrl>();
        findMatches = FindObjectOfType<FindMatchesCtrl>();
    }

    void Update()
    {
        if (isMatched)
        {
            BlockScaleDown();
        }

        if (Mathf.Abs(column - transform.position.x) > .1)
        {
            // Move Toward the target
            tempPosition = new Vector2(column, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
            if (board.blockList[column, row] != this.gameObject)
            {
                board.blockList[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
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
            if (board.blockList[column, row] != this.gameObject)
            {
                board.blockList[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();

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

    // 블록 크기 줄이기
    public IEnumerator BlockScaleDownCoroutine()
    {
        float startTime = Time.time;
        Vector2 nowScale = transform.localScale;

        while (Time.time - startTime <= 0.3f)
        {
            transform.localScale = Vector2.Lerp(nowScale, Vector2.zero, (Time.time - startTime) / 0.2f);
            yield return null;
        }

        transform.localScale = Vector2.zero;
    }

    public void BlockScaleDown()
    {
        StartCoroutine(BlockScaleDownCoroutine());
    }

    public IEnumerator CheckMoveCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        if (otherBlock != null)
        {
            if (!isMatched && !otherBlock.GetComponent<BlockCtrl>().isMatched)
            {
                otherBlock.GetComponent<BlockCtrl>().row = row;
                otherBlock.GetComponent<BlockCtrl>().column = column;
                row = previuseRow;
                column = previousCol;
                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
            }
            otherBlock = null;
        }
    }

    private void OnMouseDown()
    {
        
        if (board.currentState == GameState.move)
        {
            startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        // 그냥 클릭만으로 블록이 움직이지 않게 저항값과 스와이프 거리 비교
        if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > swipeResist || Mathf.Abs(endTouchPosition.y - startTouchPosition.y) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
            MoveHexagonPieces();
            board.currentState = GameState.wait;
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void MoveHexagonPieces()
    {   
        // Right 방향으로 움직임
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            otherBlock = board.blockList[column + 1, row];
            previousCol = column;
            previuseRow = row;
            otherBlock.GetComponent<BlockCtrl>().column -= 1;
            column += 1;
        }
        // Up 방향으로 움직임
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            otherBlock = board.blockList[column, row + 1];
            previousCol = column;
            previuseRow = row;
            otherBlock.GetComponent<BlockCtrl>().row -= 1;
            row += 1;
        }
        // Left 방향으로 움직임
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            otherBlock = board.blockList[column - 1, row];
            previousCol = column;
            previuseRow = row;
            otherBlock.GetComponent<BlockCtrl>().column += 1;
            column -= 1;
        }
        // Down 방향으로 움직임
        else if (swipeAngle >= -135 && swipeAngle < -45 && row > 0)
        {
            otherBlock = board.blockList[column, row - 1];
            previousCol = column;
            previuseRow = row;
            otherBlock.GetComponent<BlockCtrl>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCoroutine());
    }
}
