using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

//[ExecuteInEditMode]
public class DataTile : Tile
{
    [SerializeField]
    private Sprite[] dataSprites;
    [SerializeField]
    private Sprite preview;
    //wall data tells what wall code we have on each side of the tile. it is used to check if the tile can be entered.
    public string wallData = "        ";
    //this function is called every time we paint a tile into tilemap.
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }
    /// <summary>
    /// Changes the tiles sprite to the correct sprites based on the situation
    /// </summary>
    /// <param name="location">The location of this sprite</param>
    /// <param name="tilemap">A reference to the tilemap, that this tile belongs to</param>
    /// <param name="tileData">A reference to the actual object, that this tile belongs to</param>
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        //base.GetTileData(location, tilemap,ref tileData);
        tileData.sprite = preview;
    }

    public void setData(string data)
    {
        wallData = data;
    }
    public string getData()
    {
        return wallData;
    }
    

#if UNITY_EDITOR
    //This will place a new object into Unity Editors menu. Check Assets -> Create -> Tiles -> DataTile
    [MenuItem("Assets/Create/Tiles/DataTile")]
    public static void CreateDataTile()
    {
        //This will execute when someone selects the WaterTile menu item.
        string path = EditorUtility.SaveFilePanelInProject("Save DataTile", "NewDataTile", "asset", "Saved DataTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DataTile>(), path);

    }
#endif

}
[CustomEditor(typeof(Creature))]
public class DataTilemapEditor:Editor
{
    string code = "12345678";
    
    private Rect[] GetStatusRects(Vector2 position)
    {
        Rect[] rects = new Rect[8];
        rects[0] = new Rect(position.x-0.45f, position.y-0.45f,0.2f,0.2f);
        rects[1] = new Rect(position.x-0.45f, position.y-0.1f, 0.2f, 0.2f);
        rects[2] = new Rect(position.x-0.45f, position.y+0.25f, 0.2f, 0.2f);
        rects[3] = new Rect(position.x-0.1f, position.y-0.45f, 0.2f, 0.2f);
        rects[4] = new Rect(position.x-0.1f, position.y+0.25f, 0.2f, 0.2f);
        rects[5] = new Rect(position.x+0.25f, position.y-0.45f, 0.2f, 0.2f);
        rects[6] = new Rect(position.x+0.25f, position.y-0.1f, 0.2f, 0.2f);
        rects[7] = new Rect(position.x+0.25f, position.y+0.25f, 0.2f, 0.2f);
        return rects;
    }
    

    void OnSceneGUI()
    {
        
        Creature dataTile = target as Creature;
        Vector3 position = dataTile.gameObject.transform.position;
        Handles.color = Color.white;
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 24;
        char code0 = code[0];
        Vector3 pos0 = new Vector3(position.x+0.5f, position.y+0.5f, position.z);

        Rect rect = new Rect(position.x-0.45f,position.y-0.45f,0.2f,0.2f);

        Rect[] statusRects = GetStatusRects(new Vector2(position.x, position.y));
        for (int i = 0; i < statusRects.Length; i++)
        {
            Handles.DrawSolidRectangleWithOutline(statusRects[i], Color.white, Color.yellow);
        }


    //    Handles.Label(pos0,code0.ToString(),guiStyle);
    //    Handles.DrawLine(position,new Vector3(0,0,0));
        /*
        // get the chosen game object
        DrawLine t = target as DrawLine;

        if (t == null || t.GameObjects == null)
            return;

        // grab the center of the parent
        Vector3 center = t.transform.position;

        // iterate over game objects added to the array...
        for (int i = 0; i < t.GameObjects.Length; i++)
        {
            // ... and draw a line between them
            if (t.GameObjects[i] != null)
                Handles.DrawLine(center, t.GameObjects[i].transform.position);
        }*/
    }
}