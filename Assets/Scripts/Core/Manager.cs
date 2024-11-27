using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] LevelData CurrentLevel;
    [SerializeField] ScreenGrid screenGrid;
    [SerializeField] Transform boxesParent;
    [SerializeField] Box[,] matrix=new Box[GameData.cols,GameData.rows];
    private void Start()
    {
        spwanBoxes();
    }
    public void spwanBoxes()
    {
        for (int i = 0; i < CurrentLevel.matrixData.Length; i++)
        {
            Vector2 pos = screenGrid.GetCellCenter(getRowByIndex(i), getColumnByIndex(i));
            Box box=findValidObject();
            if (box == null) continue;
            box.transform.position= pos;
            box.transform.localScale = screenGrid.cellScale;
            box.row = getRowByIndex(i);
            box.col = getColumnByIndex(i);
            matrix[box.row,box.col] = box;
            box.gameObject.SetActive(true);
        }
    }


    public Box findValidObject()
    {
        int boxparentIndex = Random.Range(0, boxesParent.childCount);
        for (int i=0;i<boxesParent.GetChild(boxparentIndex).childCount;i++)
        {
            if (!boxesParent.GetChild(boxparentIndex).GetChild(i).gameObject.activeInHierarchy) return boxesParent.GetChild(boxparentIndex).GetChild(i).GetComponent<Box>();
        }
        return findValidObject();
    }

    private int getRowByIndex(int index)
    {
        return index / GameData.cols;
    }
    private int getColumnByIndex(int index)
    {
        return index % GameData.cols; 
    }


    private void findMatches()
    {
        for(int i=0;i<GameData.rows;i++)
        {
            for(int j=0;j<GameData.cols;j++)
            {

            }
        }
    }
}
