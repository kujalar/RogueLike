using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomGridBrush(true, false, false, "Data Brush")]
public class DataBrush : GridBrush
{

    public GameObject targetTilemap;
    public string wallCode;

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        //we will write only to the dataTileMap
        // this.SetTile(new Vector3Int(0, 0, 0), new DataTile());
        
        DataTilemap dataTilemap = brushTarget.GetComponent<DataTilemap>();

        base.Paint(gridLayout, brushTarget, position);
        Debug.Log("CellCount=" + cellCount + " We would paint " + brushTarget.name + " with " + wallCode + " if we would gridLayout=" + gridLayout.name + " brushTarget=" + brushTarget.name);
        //some testing
        dataTilemap.GetBoardData().Write(wallCode,position.x,position.y);
    }
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Brushes/Data Brush")]
        public static void CreateBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Data Brush", "NewDataBrush", "Asset", "Save Data Brush", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DataBrush>(), path);
        }
#endif
    }

    [CustomEditor(typeof(DataBrush))]
    public class MoveRuleBrushEditor : GridBrushEditor
    {
        public override void OnPaintInspectorGUI()
        {
            base.OnPaintInspectorGUI();
        }
        public override void OnPaintSceneGUI(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
        {
            base.OnPaintSceneGUI(gridLayout, brushTarget, position, tool, executing);
            //this is when our editor is painting the scene
        }
    }

