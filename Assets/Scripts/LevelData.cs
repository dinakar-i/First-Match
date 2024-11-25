using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/Create Level", order = 1)]
public class LevelData : ScriptableObject
{
    public int[] matrixData=new int[GameData.rows*GameData.cols];
    public int DestroyableCount;
}
