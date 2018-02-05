using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoardData : MonoBehaviour {
    public int dimX;
    public int dimY;
    public int sizeX;
    public int sizeY;
    public bool allocateOnChange = false;
    [SerializeField]
    public DataEntry[] dataEntryArray;

    public void initDatastore()
    {
        //this is now initiated to start from zero and continue to maxX or maxY

        //do not allocate if the size would be zero or less than zero
        if (sizeX <= 0 || sizeY <= 0)
        {
            Debug.LogWarning("Cannot allocate dataentries with dimensions less than zero. X="+sizeX+" Y="+sizeY);
            return;
        }
        if ((sizeX != GetAllocatedX() || sizeY != GetAllocatedY())&&allocateOnChange)
        {
            Debug.Log("Allocating new DataEntries for size "+sizeX+"x"+sizeY);
            dataEntryArray = new DataEntry[sizeX * sizeY];
            dimX = sizeX;
            dimY = sizeY;
        }
    }
    public int GetAllocatedX()
    {
        if (dataEntryArray == null)
        {
            return 0;
        }
        return dimX;//dataEntries.GetLength(0);
    }
    public int GetAllocatedY()
    {
        if(dataEntryArray == null)
        {
            return 0;
        }
        return dimY;// dataEntries.GetLength(1);
    }

    private bool IsOutOfBounds(int x, int y)
    {
        return x >= GetAllocatedX() || y >= GetAllocatedY() || x < 0 || y < 0;
    }

    public void Write(string code,int x, int y)
    {
        if (IsOutOfBounds(x,y))
        {
            //here is easy check if we are within the allocated datamatrix
            //TODO allocating more space runtime
            Debug.LogError("Out of bounds Error while writing to ("+x+","+y+"). Cannot write.");
            return;
        }
        int dataIndex = dimX * y + x;
        if (code == null)
        {
            //code null means erase the data from a position
            dataEntryArray[dataIndex] = null;
            return;
        }
        if (dataEntryArray[dataIndex] == null)
        {
            dataEntryArray[dataIndex] = new DataEntry();
        }
        Debug.Log("We write code "+code+" to point ("+x+","+y+") which has dataIndex ="+ dataIndex);
        //dataEntries[x, y].code = code;
        dataEntryArray[dataIndex].code = code;
    }
    public DataEntry Read(int x, int y)
    {
        if (IsOutOfBounds(x, y))
        {
            return null;
        }
        int dataIndex = dimX * y + x;
        return dataEntryArray[dataIndex];
    }
}

[System.Serializable]
public class DataEntry
{
    public string code = null;
}

[CustomEditor(typeof(BoardData))]
[CanEditMultipleObjects]
public class BoardDataEditor: Editor
{
    SerializedProperty sizeX;
    SerializedProperty sizeY;
    SerializedProperty allocateOnChange;

    void OnEnable()
    {
        sizeX = serializedObject.FindProperty("sizeX");
        sizeY = serializedObject.FindProperty("sizeY");
        allocateOnChange = serializedObject.FindProperty("allocateOnChange");
    }

    public override void OnInspectorGUI()
    {
        BoardData boardData = target as BoardData;

        serializedObject.Update();

        string maxIndex;
        if (boardData.dataEntryArray!=null)
        {
            maxIndex = (boardData.dataEntryArray.Length-1).ToString();
        } else
        {
            maxIndex = "none";
        }
        string label = "Allocated size="+boardData.GetAllocatedX() + "x" + boardData.GetAllocatedY()+" maxIndex="+maxIndex;

        EditorGUILayout.LabelField(label);
        EditorGUILayout.PropertyField(sizeX);
        EditorGUILayout.PropertyField(sizeY);
        EditorGUILayout.PropertyField(allocateOnChange);
        serializedObject.ApplyModifiedProperties();

        boardData.initDatastore();
    }
    
}
