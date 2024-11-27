using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] LevelData CurrentLevel;
    public ScreenGrid screenGrid;
    [SerializeField] Transform boxesParent;
    [SerializeField] Box[,] matrix = new Box[GameData.cols, GameData.rows];
    [SerializeField] HashSet<Box> matchedBoxes = new HashSet<Box>();
    public Camera cam;
    public float swappSpeed;
    public int movingObjects, gravitySpeed;
    private void Start()
    {
        spwanBoxes();
    }
    public void spwanBoxes()
    {
        for (int i = 0; i < CurrentLevel.matrixData.Length; i++)
        {
            Vector2 pos = screenGrid.GetCellCenter(getRowByIndex(i), getColumnByIndex(i));
            Box box = findValidObject();
            if (box == null) continue;
            box.transform.position = pos;
            box.transform.localScale = screenGrid.cellScale;
            box.row = getRowByIndex(i);
            box.col = getColumnByIndex(i);
            setBoxIntoMatrix(box);
            box.gameObject.SetActive(true);
        }
    }


    public Box findValidObject()
    {
        int boxparentIndex = Random.Range(0, boxesParent.childCount);
        for (int i = 0; i < boxesParent.GetChild(boxparentIndex).childCount; i++)
        {
            if (!boxesParent.GetChild(boxparentIndex).GetChild(i).gameObject.activeInHierarchy) return boxesParent.GetChild(boxparentIndex).GetChild(i).GetComponent<Box>();
        }
        return findValidObject();
    }
    public Box getBox(int row, int col)
    {
        if (row < 0 || col < 0 || row > GameData.rows - 1 || col > GameData.cols - 1) return null;
        return matrix[row, col];
    }
    public void setBoxIntoMatrix(Box box)
    {
        matrix[box.row, box.col] = box;
    }
    public void removeBoxFromMatrix(Box box)
    {
        matrix[box.row, box.col] = null;
    }
    public void SwapBoxes(Box box1, Box box2)
    {
        if (box1 == null || box2 == null) return;

        //swap rows
        box1.row = box1.row + box2.row;
        box2.row = Mathf.Abs(box1.row - box2.row);
        box1.row = Mathf.Abs(box1.row - box2.row);
        //swap cols
        box1.col = box1.col + box2.col;
        box2.col = Mathf.Abs(box1.col - box2.col);
        box1.col = Mathf.Abs(box1.col - box2.col);
        setBoxIntoMatrix(box1);
        setBoxIntoMatrix(box2);


        StartCoroutine(swapBoxesPos(box1, box2));

    }
    private int getRowByIndex(int index)
    {
        return index / GameData.cols;
    }
    private int getColumnByIndex(int index)
    {
        return index % GameData.cols;
    }

    [ContextMenu("find Matches")]
    private void findMatches()
    {
        for (int i = 0; i < GameData.rows; i++)
        {
            for (int j = 0; j < GameData.cols; j++)
            {
                if(getBox(i, j)==null) continue;
                HLine(getBox(i, j));
                VLine(getBox(i, j));
            }
        }
        destroyMatchedBoxes();
    }
    private void HLine(Box box)
    {
        if (getBox(box.row - 1, box.col) == null || getBox(box.row + 1, box.col) == null) return;
        if (getBox(box.row - 1, box.col).code == box.code && matrix[box.row + 1, box.col].code == box.code)
        {
            matchedBoxes.Add(box);
            matchedBoxes.Add(getBox(box.row + 1, box.col));
            matchedBoxes.Add(getBox(box.row - 1, box.col));
        }
    }
    private void VLine(Box box)
    {
        if (getBox(box.row, box.col + 1) == null || getBox(box.row, box.col - 1) == null) return;
        if (getBox(box.row, box.col + 1).code == box.code && matrix[box.row, box.col - 1].code == box.code)
        {
            matchedBoxes.Add(box);
            matchedBoxes.Add(getBox(box.row, box.col - 1));
            matchedBoxes.Add(getBox(box.row, box.col + 1));
        }
    }


    private void destroyMatchedBoxes()
    {
        foreach (Box box in matchedBoxes) box.selfDestroy();
        matchedBoxes.Clear();
    }

    IEnumerator swapBoxesPos(Box box1, Box box2)
    {
        movingObjects++;
        Vector2 box1TargetPos = box2.transform.position;
        Vector2 box2TargetPos = box1.transform.position;
        while ((Vector2)box1.transform.position != box1TargetPos || (Vector2)box2.transform.position != box2TargetPos)
        {
            box1.transform.position = Vector2.MoveTowards(box1.transform.position, box1TargetPos, swappSpeed * Time.deltaTime);
            box2.transform.position = Vector2.MoveTowards(box2.transform.position, box2TargetPos, swappSpeed * Time.deltaTime);
            yield return null;
        }
        findMatches();
        movingObjects--;
    }
}
