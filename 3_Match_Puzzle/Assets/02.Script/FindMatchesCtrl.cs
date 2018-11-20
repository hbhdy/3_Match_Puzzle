//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//public class FindMatchesCtrl : MonoBehaviour {

//    private BoardCtrl board;
//    public List<GameObject> currentMatches = new List<GameObject>();
//    // Use this for initialization
//    void Start()
//    {
//        board = FindObjectOfType<BoardCtrl>();
//    }
//    public void FindAllMatches()
//    {
//        StartCoroutine(FindAllMatchesCoroutine());
//    }

    
//    private IEnumerator FindAllMatchesCoroutine()
//    {
//        yield return new WaitForSeconds(.2f);
//        for(int i=0;i<board.width;i++)
//        {
//            for(int j=0;j<board.height;j++)
//            {
//                GameObject currentHexagon = board.allHexagons[i, j];
//                if (currentHexagon != null)
//                {
//                    if (i > 0 && i < board.width - 1)
//                    {
//                        GameObject leftHexagon = board.allHexagons[i - 1, j];
//                        GameObject rightHexagon = board.allHexagons[i + 1, j];

//                        if (leftHexagon!= null && rightHexagon != null)
//                        {
//                            if(leftHexagon.tag == currentHexagon.tag && rightHexagon.tag== currentHexagon.tag)
//                            {
//                                //if(currentHexagon.GetComponent<HexagonCtrl>().isRowBomb
//                                //    || leftHexagon.GetComponent<HexagonCtrl>().isRowBomb
//                                //    || rightHexagon.GetComponent<HexagonCtrl>().isRowBomb)
//                                //{
//                                //    currentMatches.Union(GetRowPieces(j));
//                                //}

//                                //if (currentHexagon.GetComponent<HexagonCtrl>().isColBomb)
//                                //{
//                                //    currentMatches.Union(GetColPieces(i));
//                                //}

//                                //if (leftHexagon.GetComponent<HexagonCtrl>().isColBomb)
//                                //{
//                                //    currentMatches.Union(GetColPieces(i - 1));
//                                //}

//                                //if (rightHexagon.GetComponent<HexagonCtrl>().isColBomb)
//                                //{
//                                //    currentMatches.Union(GetColPieces(i + 1));
//                                //}

//                                if (!currentMatches.Contains(leftHexagon))
//                                {
//                                    currentMatches.Add(leftHexagon);
//                                }
//                                leftHexagon.GetComponent<HexagonCtrl>().isMatched = true;

//                                if (!currentMatches.Contains(rightHexagon))
//                                {
//                                    currentMatches.Add(rightHexagon);
//                                }
//                                rightHexagon.GetComponent<HexagonCtrl>().isMatched = true;

//                                if (!currentMatches.Contains(currentHexagon))
//                                {
//                                    currentMatches.Add(currentHexagon);
//                                }
//                                currentHexagon.GetComponent<HexagonCtrl>().isMatched = true;                            
//                            }
//                        }
//                    }

//                    if (j > 0 && j < board.height - 1)
//                    {
//                        GameObject upHexagon = board.allHexagons[i, j+1];
//                        GameObject downHexagon = board.allHexagons[i, j-1];

//                        if (upHexagon != null && downHexagon != null)
//                        {
//                            //if (currentHexagon.GetComponent<HexagonCtrl>().isColBomb
//                            //        || upHexagon.GetComponent<HexagonCtrl>().isColBomb
//                            //        || downHexagon.GetComponent<HexagonCtrl>().isColBomb)
//                            //{
//                            //    currentMatches.Union(GetColPieces(i));
//                            //}

//                            //if (currentHexagon.GetComponent<HexagonCtrl>().isRowBomb)
//                            //{
//                            //    currentMatches.Union(GetColPieces(j));
//                            //}

//                            //if (upHexagon.GetComponent<HexagonCtrl>().isRowBomb)
//                            //{
//                            //    currentMatches.Union(GetColPieces(j + 1));
//                            //}

//                            //if (downHexagon.GetComponent<HexagonCtrl>().isRowBomb)
//                            //{
//                            //    currentMatches.Union(GetColPieces(j - 1));
//                            //}

//                            if (upHexagon.tag == currentHexagon.tag && downHexagon.tag == currentHexagon.tag)
//                            {
//                                if (!currentMatches.Contains(upHexagon))
//                                {
//                                    currentMatches.Add(upHexagon);
//                                }
//                                upHexagon.GetComponent<HexagonCtrl>().isMatched = true;

//                                if (!currentMatches.Contains(downHexagon))
//                                {
//                                    currentMatches.Add(downHexagon);
//                                }
//                                downHexagon.GetComponent<HexagonCtrl>().isMatched = true;

//                                if (!currentMatches.Contains(currentHexagon))
//                                {
//                                    currentMatches.Add(currentHexagon);
//                                }
//                                currentHexagon.GetComponent<HexagonCtrl>().isMatched = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }

//    //List<GameObject> GetColPieces(int col)
//    //{
//    //    List<GameObject> hexagons = new List<GameObject>();

//    //    for(int i = 0; i < board.height; i++)
//    //    {
//    //        if(board.allHexagons[col,i]!= null)
//    //        {
//    //            hexagons.Add(board.allHexagons[col, i]);
//    //            board.allHexagons[col, i].GetComponent<HexagonCtrl>().isMatched = true;
//    //        }
//    //    }
//    //    return hexagons;
//    //}

//    //List<GameObject> GetRowPieces(int row)
//    //{
//    //    List<GameObject> hexagons = new List<GameObject>();

//    //    for (int i = 0; i < board.width; i++)
//    //    {
//    //        if (board.allHexagons[i, row] != null)
//    //        {
//    //            hexagons.Add(board.allHexagons[i, row]);
//    //            board.allHexagons[i, row].GetComponent<HexagonCtrl>().isMatched = true;
//    //        }
//    //    }
//    //    return hexagons;
//    //}

//}
