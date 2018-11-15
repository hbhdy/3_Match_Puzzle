using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCtrl : MonoBehaviour
{
    public int col;
    public int row;
    public int targetX;
    public int targetY;

    private BoardCtrl board;
    private GameObject otherHexagon;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0f;


    // Use this for initialization
    void Start()
    {
        board = FindObjectOfType<BoardCtrl>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        col = targetX;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = col;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            // Move Toward the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allHexagons[col, row] = this.gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move Toward the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allHexagons[col, row] = this.gameObject;

        }
    }

    private void OnMouseDown()
    {
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(startTouchPosition);
    }

    private void OnMouseUp()
    {
        endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {

        swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
        //Debug.Log(swipeAngle);
        MoveHexagonPieces();
    }

    void MoveHexagonPieces()
    {
        // Right 방향으로 움직임
        if (swipeAngle > -45 && swipeAngle <= 45 && col < board.width)
        {
            otherHexagon = board.allHexagons[col + 1, row];
            otherHexagon.GetComponent<HexagonCtrl>().col -= 1;
            col += 1;
        }
        //  방향으로 움직임
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            otherHexagon = board.allHexagons[col, row + 1];
            otherHexagon.GetComponent<HexagonCtrl>().row -= 1;
            row += 1;
        }
        //  방향으로 움직임
        else if ((swipeAngle > 135 || swipeAngle <= -135) && col >0)
        {
            otherHexagon = board.allHexagons[col -1, row];
            otherHexagon.GetComponent<HexagonCtrl>().col += 1;
            col -= 1;
        }
        //  방향으로 움직임
        else if (swipeAngle >= -135 && swipeAngle < -45 && row > 0)
        {
            otherHexagon = board.allHexagons[col, row-1];
            otherHexagon.GetComponent<HexagonCtrl>().row+= 1;
            row -= 1;
        }
    }
}
