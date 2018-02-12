using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class BoardData : MonoBehaviour {
    public int dimX;
    public int dimY;
    public int sizeX;
    public int sizeY;
    public Vector2Int minXY;
    public Vector2Int offset;
    public bool allocateOnChange = false;
    public bool clearDataOnAllocate = false;
    [SerializeField]
    public DataEntry[] dataEntryArray;

    public int linearOffset;

    public void initDatastore()
    {
        //we will do init only if we have allocateOnChange setting true. this means allocation is done if there is change,
        //it is checked further.
        if (!allocateOnChange)
        {
            return;
        }
        //this is now initiated to start from zero and continue to maxX or maxY

        //do not allocate if the size would be zero or less than zero
        if (sizeX <= 0 || sizeY <= 0)
        {
            Debug.LogWarning("Cannot allocate dataentries with dimensions less than zero. X="+sizeX+" Y="+sizeY);
            return;
        }
        if (offset == null)
        {
            offset = new Vector2Int(0, 0);
        }

        if ((sizeX != GetAllocatedX() || sizeY != GetAllocatedY()|| minXY.x!= offset.x || minXY.y != offset.y))
        {
            Debug.Log("Allocating new DataEntries for size "+sizeX+"x"+sizeY);

            DataEntry[] oldData = dataEntryArray;
            int oldDimX = dimX;
            int oldDimY = dimY;
            Vector2Int oldMinXY = minXY;

            dataEntryArray = new DataEntry[sizeX * sizeY];
            dimX = sizeX;
            dimY = sizeY;
            minXY = offset;
            linearOffset = sizeX * minXY.y + minXY.x;

            if (!clearDataOnAllocate)
            {
                CopyToDataEntry(oldData, oldDimX, oldDimY,oldMinXY);
            }
        }
    }
    
    //this should copy data from old matrix to new on size changes.
    private void CopyToDataEntry(DataEntry[] sourceData,int sX, int sY, Vector2Int sMinXY)
    {
        int fromX =Mathf.Max(sMinXY.x,minXY.x);
        int fromY =Mathf.Max(sMinXY.y,minXY.y);

        int toX = Mathf.Min(sX+sMinXY.x, dimX+minXY.x);
        int toY = Mathf.Min(sY+sMinXY.y, dimY+minXY.y);

        int sLinearOffset = sX * sMinXY.y + sMinXY.x;

        for(int x = fromX; x < toX; x++)
        {
            for(int y=fromY; y< toY; y++)
            {
                DataEntry entry = sourceData[sX*y+x - sLinearOffset];
                string entryStr = "e.code=";
                if (entry == null)
                {
                    entryStr = "null";
                } else if(entry.code != null)
                {
                    entryStr += entry.code;
                    dataEntryArray[dimX * y + x - linearOffset] = entry;
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

    public int getMinX()
    {
        if (minXY != null)
        {
            return minXY.x;
        }
        return 0;
    }
    public int getMinY()
    {
        if (minXY != null)
        {
            return minXY.y;
        }
        return 0;
    }
    public int getMaxX()
    {
        return GetAllocatedX() + getMinX();
    }
    public int getMaxY()
    {
        return GetAllocatedY() + getMinY();
    }
    private bool IsOutOfBounds(int x, int y)
    {
        return x >= getMaxX() || y >= getMaxY() || x < getMinX() || y < getMinY();
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
        int dataIndex = dimX * y + x - linearOffset;
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
        int dataIndex = dimX * y + x - linearOffset;
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
    SerializedProperty offset;

    SerializedProperty sizeX;
    SerializedProperty sizeY;
   
    SerializedProperty allocateOnChange;
    SerializedProperty clearDataOnAllocate;

    void OnEnable()
    {
        offset = serializedObject.FindProperty("offset");
        sizeX = serializedObject.FindProperty("sizeX");
        sizeY = serializedObject.FindProperty("sizeY");
        allocateOnChange = serializedObject.FindProperty("allocateOnChange");
        clearDataOnAllocate = serializedObject.FindProperty("clearDataOnAllocate");


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
        string label2 = "minXY=(" + boardData.minXY.x + "," + boardData.minXY.y + ") linearOffset=" + boardData.linearOffset;

        EditorGUILayout.LabelField(label);
        EditorGUILayout.LabelField(label2);
        EditorGUILayout.PropertyField(sizeX);
        EditorGUILayout.PropertyField(sizeY);
        EditorGUILayout.PropertyField(offset);
        EditorGUILayout.PropertyField(allocateOnChange);
        EditorGUILayout.PropertyField(clearDataOnAllocate);
        serializedObject.ApplyModifiedProperties();

        boardData.initDatastore();
    }
    
}
