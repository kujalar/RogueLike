using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoardData : MonoBehaviour {
    public int[] meArray = new int[0];
    public string[] ploro;
    public int maxX;
    public int maxY;
    public DataEntry[] dataEntries = new DataEntry[0];

    public void initDatastore()
    {
        if (maxX * maxY != dataEntries.Length)
        {
            dataEntries = new DataEntry[maxX*maxY];
        }
    }
    public void Write(string code)
    {
        dataEntries[2].code = code;
    }
}
[System.Serializable]
public class DataEntry
{
    public string code;
}
/*
[CustomEditor(typeof(BoardData))]
[CanEditMultipleObjects]
public class BoardDataEditor: Editor
{
    SerializedProperty maxX;
    SerializedProperty maxY;

    void OnEnable()
    {
        maxX = serializedObject.FindProperty("maxX");
        maxY = serializedObject.FindProperty("maxY");
    }

    public override void OnInspectorGUI()
    {
        BoardData boardData = target as BoardData;

        serializedObject.Update();


        EditorGUILayout.PropertyField(maxX);
        EditorGUILayout.PropertyField(maxY);

        serializedObject.ApplyModifiedProperties();

        boardData.initDatastore();
    }
    
}*/
