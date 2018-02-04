using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class DataTilemap : MonoBehaviour {
    public string[] test;
    [HideInInspector]
    Tilemap tilemap;
    [SerializeField]
    //public DataEntry[,] dataEntries = new DataEntry[1000,1000];
    public BoardData boardData;
	// Use this for initialization
	void Start () {
        tilemap = GetComponent<Tilemap>();
	}
    public DataTile getDataTileFromWorld(Vector3 worldPoint)
    {
        Vector3Int cell = tilemap.WorldToCell(worldPoint);

        TileBase tilebase = tilemap.GetTile(cell);
        DataTile dataTile = null;

        if (tilebase != null)
        {
            dataTile = tilebase as DataTile;
        }
        return dataTile;
    }
    public BoundsInt getVisibleBounds()
    {
        //TODO tähän jotkut järkevät boundsit
        int xMin = 0, yMin = 0, zMin = 0, sizeX = 5, sizeY = 5, sizeZ = 1;
        return new BoundsInt(xMin, yMin, zMin, sizeX, sizeY, sizeZ);
    }

}



[CustomEditor(typeof(DataTilemap))]
public class DataTilemapEditor : Editor
{
    string code = "12345678";

    private Rect[] GetStatusRects(Vector2 position)
    {
        Rect[] rects = new Rect[8];
        rects[0] = new Rect(position.x - 0.45f, position.y - 0.45f, 0.2f, 0.2f);
        rects[1] = new Rect(position.x - 0.45f, position.y - 0.1f, 0.2f, 0.2f);
        rects[2] = new Rect(position.x - 0.45f, position.y + 0.25f, 0.2f, 0.2f);
        rects[3] = new Rect(position.x - 0.1f, position.y - 0.45f, 0.2f, 0.2f);
        rects[4] = new Rect(position.x - 0.1f, position.y + 0.25f, 0.2f, 0.2f);
        rects[5] = new Rect(position.x + 0.25f, position.y - 0.45f, 0.2f, 0.2f);
        rects[6] = new Rect(position.x + 0.25f, position.y - 0.1f, 0.2f, 0.2f);
        rects[7] = new Rect(position.x + 0.25f, position.y + 0.25f, 0.2f, 0.2f);
        return rects;
    }


    void OnSceneGUI()
    {
        DataTilemap dataMap = target as DataTilemap;
        Vector3 position = dataMap.gameObject.transform.position;

        BoundsInt bounds = dataMap.getVisibleBounds();
        Tilemap tilemap = dataMap.GetComponent<Tilemap>();
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);
        Handles.color = Color.white;
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                DataTile dataTile = tilemap.GetTile(new Vector3Int(x,y,0)) as DataTile;
                DataEntry dataEntry = dataMap.boardData.Read(x,y);//dataMap.dataEntries[x,y];
                DrawDatatileSceneSymbols(new Vector2(x,y), dataEntry);
            }
        }
    }
    private void DrawDatatileSceneSymbols(Vector2 position,DataEntry dataEntry)
    {
        if (dataEntry == null)
        {
            return;
        }
        
        Rect[] statusRects = GetStatusRects(position);
        for (int i = 0; i < statusRects.Length; i++)
        {
            if (dataEntry.code.Equals("XXXXXXXX"))
            {
                Handles.DrawSolidRectangleWithOutline(statusRects[i], Color.white, Color.yellow);
            }
        }
    }
       
}
