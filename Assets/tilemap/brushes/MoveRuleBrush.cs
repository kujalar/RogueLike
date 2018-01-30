using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomGridBrush(true, false, false, "MoveRule Brush")]
    public class MoveRuleBrush : GridBrush
    {

    public string strangeCode;

        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            base.Paint(gridLayout, brushTarget, position);
            Debug.Log("We would paint "+brushTarget.name+" with "+strangeCode+" if we would");
        }
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Brushes/MoveRule Brush")]
        public static void CreateBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save MoveRule Brush", "NewMoveRuleBrush", "Asset", "Save MoveRule Brush", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MoveRuleBrush>(), path);
        }
#endif
    }

    [CustomEditor(typeof(MoveRuleBrush))]
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

