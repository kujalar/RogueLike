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
    public bool clearDataOnAllocate = false;
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

            DataEntry[] oldData = dataEntryArray;
            int oldDimX = dimX;
            int oldDimY = dimY;

            dataEntryArray = new DataEntry[sizeX * sizeY];
            dimX = sizeX;
            dimY = sizeY;
            if (!clearDataOnAllocate)
            {
                CopyToDataEntry(oldData, oldDimX, oldDimY);
            }
        }
    }
    //this should copy data from old matrix to new on size changes.
    private void CopyToDataEntry(DataEntry[] sourceData,int sX, int sY)
    {
        int toX = Mathf.Min(sX,dimX);
        int toY = Mathf.Min(sY, dimY);

        for(int x = 0; x < toX; x++)
        {
            for(int y=0; y< toY; y++)
            {
                DataEntry entry = sourceData[sX*y+x];
                string entryStr = "e.code=";
                if (entry == null)
                {
                    entryStr = "null";
                } else if(entry.code != null)
                {
                    entryStr += entry.code;
                    dataEntryArray[dimX * y + x] = entry;
                }
                Debug.Log("(" + x + "," + y + ") = "+entryStr);
                
            }
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
    SerializedProperty clearDataOnAllocate;

    Vector3[] visualisationPoints = new Vector3[8];

    void OnEnable()
    {
        sizeX = serializedObject.FindProperty("sizeX");
        sizeY = serializedObject.FindProperty("sizeY");
        allocateOnChange = serializedObject.FindProperty("allocateOnChange");
        clearDataOnAllocate = serializedObject.FindProperty("clearDataOnAllocate");


    }
    void OnSceneGUI()
    {
        BoardData boardData = target as BoardData;
        //we want to draw rectangle to board, which shows the area how big our boardData is.
        float minX = 0-0.5f;
        float minY = 0-0.5f;
        float maxX = boardData.dimX - 0.5f;
        float maxY = boardData.dimY - 0.5f;

        visualisationPoints[0] = new Vector3(minX, minY, 0);
        visualisationPoints[1] = new Vector3(minX, maxY, 0);

        visualisationPoints[2] = visualisationPoints[0];
        visualisationPoints[3] = new Vector3(maxX, minY, 0);

        visualisationPoints[4] = new Vector3(maxX, maxY, 0);
        visualisationPoints[5] = visualisationPoints[1];

        visualisationPoints[6] = visualisationPoints[4];
        visualisationPoints[7] = visualisationPoints[3];

        Handles.DrawDottedLines(visualisationPoints,10.0f);
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
        EditorGUILayout.PropertyField(clearDataOnAllocate);
        serializedObject.ApplyModifiedProperties();

        boardData.initDatastore();
    }
    
}
