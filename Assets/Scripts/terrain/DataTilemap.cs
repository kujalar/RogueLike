﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class DataTilemap : MonoBehaviour {
    public string[] test;
    [HideInInspector]
    Tilemap tilemap;
    private BoardData boardData;
	// Use this for initialization
	void Start () {
        tilemap = GetComponent<Tilemap>();
        //try if this works with local boardData object
        boardData = GetComponent<BoardData>();
	}
    public BoardData GetBoardData()
    {
        if (boardData == null)
        {
            boardData = GetComponent<BoardData>();
        }
        return boardData;
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
    public DataEntry GetDataEntryFromWorld(Vector3 worldPoint)
    {
        Vector3Int cell = tilemap.WorldToCell(worldPoint);
        return boardData.Read(cell.x,cell.y);
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
    Vector3[] visualisationPoints = new Vector3[8];

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
        //TODO need some intelligent boundaries where to find data, now they are 0 to 10 for x, and 0 to 10 for y values.

        BoardData boardData = dataMap.GetBoardData();
        int minX = 0;
        int maxX = 0;
        int minY = 0;
        int maxY = 0;
        if (boardData != null)
        {
            minX = boardData.getMinX();
            maxX = boardData.getMaxX();
            minY = boardData.getMinY();
            maxY = boardData.getMaxY();
        }


        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                DataTile dataTile = tilemap.GetTile(new Vector3Int(x,y,0)) as DataTile;
                DataEntry dataEntry = dataMap.GetBoardData().Read(x,y);//dataMap.dataEntries[x,y];
                DrawDatatileSceneSymbols(new Vector2(x,y), dataEntry);
            }
        }

        DrawBoardDataSceneVisualization(dataMap.GetBoardData());
    }
    private void DrawBoardDataSceneVisualization(BoardData boardData)
    {
        if (boardData == null)
        {
            return;
        }
        //we want to draw rectangle to board, which shows the area how big our boardData is.
        float minX = 0 - 0.5f + boardData.minXY.x;
        float minY = 0 - 0.5f + boardData.minXY.y;
        float maxX = boardData.dimX - 0.5f + boardData.minXY.x;
        float maxY = boardData.dimY - 0.5f + boardData.minXY.y;

        visualisationPoints[0] = new Vector3(minX, minY, 0);
        visualisationPoints[1] = new Vector3(minX, maxY, 0);

        visualisationPoints[2] = visualisationPoints[0];
        visualisationPoints[3] = new Vector3(maxX, minY, 0);

        visualisationPoints[4] = new Vector3(maxX, maxY, 0);
        visualisationPoints[5] = visualisationPoints[1];

        visualisationPoints[6] = visualisationPoints[4];
        visualisationPoints[7] = visualisationPoints[3];

        Handles.DrawDottedLines(visualisationPoints, 10.0f);
    }
    private void DrawRectangle(Rect rect, char code)
    {
        if (code == 'X')
        {
            Handles.DrawSolidRectangleWithOutline(rect, Color.black, Color.white);
        }
    }
    private void DrawDatatileSceneSymbols(Vector2 position,DataEntry dataEntry)
    {
        if (dataEntry == null)
        {
            return;
        }
        
        Rect[] statusRects = GetStatusRects(position);
        string code = dataEntry.code;
        if (code != null)
        {
            int length = code.Length;
            for (int i = 0; i < statusRects.Length && i < length; i++)
            {
                DrawRectangle(statusRects[i], code[i]);
            }
        }
        
    }
       
}
