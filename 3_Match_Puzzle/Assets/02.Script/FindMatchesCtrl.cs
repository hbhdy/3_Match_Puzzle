using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatchesCtrl : MonoBehaviour
{

    private BoardCtrl board;
    public List<GameObject> currentMatches = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        board = FindObjectOfType<BoardCtrl>();
    }
    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCoroutine());
    }

    private IEnumerator FindAllMatchesCoroutine()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentBlock = board.blockList[i, j];

                if (currentBlock != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftBlock = board.blockList[i - 1, j];
                        GameObject rightBlock = board.blockList[i + 1, j];

                        if (leftBlock != null && rightBlock != null)
                        {
                            if (leftBlock.GetComponent<BlockCtrl>().currentColorNumber == currentBlock.GetComponent<BlockCtrl>().currentColorNumber 
                                && rightBlock.GetComponent<BlockCtrl>().currentColorNumber == currentBlock.GetComponent<BlockCtrl>().currentColorNumber)
                            {                            
                                leftBlock.GetComponent<BlockCtrl>().isMatched = true;                                                               
                                rightBlock.GetComponent<BlockCtrl>().isMatched = true;                                                             
                                currentBlock.GetComponent<BlockCtrl>().isMatched = true;                              
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upBlock = board.blockList[i, j + 1];
                        GameObject downBlock = board.blockList[i, j - 1];

                        if (upBlock != null && downBlock != null)
                        {
                            if (upBlock.GetComponent<BlockCtrl>().currentColorNumber == currentBlock.GetComponent<BlockCtrl>().currentColorNumber
                                && downBlock.GetComponent<BlockCtrl>().currentColorNumber == currentBlock.GetComponent<BlockCtrl>().currentColorNumber)
                            {                            
                                upBlock.GetComponent<BlockCtrl>().isMatched = true;
                                downBlock.GetComponent<BlockCtrl>().isMatched = true;
                                currentBlock.GetComponent<BlockCtrl>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
