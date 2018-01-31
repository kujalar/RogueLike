using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

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