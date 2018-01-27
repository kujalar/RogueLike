using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class SolidWall1Tile : Tile, RogueLikeTile {
    [SerializeField]
    private Sprite[] wallSprites;
    [SerializeField]
    private Sprite preview;

    //this function is called every time we paint a tile into tilemap.
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //here we must check tiles neighbours so that we can paint a right kind of a water tile.
        //order does not matter much here... 
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (IsRefreshNeeded(tilemap, nPos))
                {
                    tilemap.RefreshTile(nPos);
                }
            }
        }
    }
    public string GetCode()
    {
        //S like a SolidWall, we can use this tile to interact with all S type tiles.
        return "S";
    }
    private bool IsRefreshNeeded(ITilemap tilemap, Vector3Int position)
    {
        //we must check refresh if tile in map is SolidWall1, same type as our tile.
        RogueLikeTile tile = tilemap.GetTile(position) as RogueLikeTile;
        if (tile != null && this.GetCode().Equals(tile.GetCode()))
        {
            return true;
        }
        return false;
        //return tilemap.GetTile(position) == this;
    }
    private bool IsMatch(string composition, string pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            char c = composition[i];
            char p = pattern[i];
            if (p != '*'&&p!=c)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Changes the tiles sprite to the correct sprites based on the situation
    /// </summary>
    /// <param name="location">The location of this sprite</param>
    /// <param name="tilemap">A reference to the tilemap, that this tile belongs to</param>
    /// <param name="tileData">A reference to the actual object, that this tile belongs to</param>
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        string composition = string.Empty;//Makes an empty string as compostion, we need this so that we change the sprite

        for (int x = -1; x <= 1; x++)//Runs through all neighbours 
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0) //Makes sure that we aren't checking our self
                {
                    //If the value is a watertile
                    if (IsRefreshNeeded(tilemap, new Vector3Int(location.x + x, location.y + y, location.z)))
                    {
                        composition += 'X';
                    }
                    else
                    {
                        composition += '_';
                    }


                }
            }
        }
        tileData.sprite = wallSprites[0];
        if (composition[1] == '_' && composition[3] == '_' && composition[4] == '_' && composition[6] == '_')
        {
            tileData.sprite = wallSprites[0];
        } else if (composition[1] == '_' && composition[3] == '_' && composition[4] == 'X' && composition[6] == '_')
        {
            tileData.sprite = wallSprites[1];
        } else if (composition[1] == 'X' && composition[3] == '_' && composition[4] == '_' && composition[6] == '_')
        {
            tileData.sprite = wallSprites[2];
        } else if (composition[1] == '_' && composition[3] == 'X' && composition[4] == '_' && composition[6] == '_')
        {
            tileData.sprite = wallSprites[3];
        }
        else if (composition[1] == '_' && composition[3] == '_' && composition[4] == '_' && composition[6] == 'X')
        {
            tileData.sprite = wallSprites[4];
        }
        else if (composition == "XXXXXXX_")
        {
            tileData.sprite = wallSprites[5];
        }
        else if (IsMatch(composition, "*XX_X*XX"))
        {
            tileData.sprite = wallSprites[6];
        }
        else if (IsMatch(composition, "XXXXX*_*"))
        {
            tileData.sprite = wallSprites[7];
        }
        else if (IsMatch(composition, "*_*XXXXX"))
        {
            tileData.sprite = wallSprites[8];
        }
        else if (IsMatch(composition, "XX*X_XX*"))
        {
            tileData.sprite = wallSprites[9];
        }
        else if (IsMatch(composition, "_X_XX_X_"))
        {
            tileData.sprite = wallSprites[10];
        }
        else if (IsMatch(composition, "XX_XXXXX"))
        {
            tileData.sprite = wallSprites[11];
        }
        else if (IsMatch(composition, "*_*_X*XX"))
        {
            tileData.sprite = wallSprites[12];
        }
        else if (IsMatch(composition, "XX*X_*_*"))
        {
            tileData.sprite = wallSprites[13];
        }
        else if (IsMatch(composition, "*XX_X*_*"))
        {
            tileData.sprite = wallSprites[14];
        }
        else if (IsMatch(composition, "*_*X_XX*"))
        {
            tileData.sprite = wallSprites[15];
        }
        else if (IsMatch(composition, "XX_XXXX_"))
        {
            tileData.sprite = wallSprites[16];
        }
        else if (IsMatch(composition, "_XXXXXXX"))
        {
            tileData.sprite = wallSprites[17];
        }
        else if (IsMatch(composition, "*_*_X*X_"))
        {
            tileData.sprite = wallSprites[18];
        }
        else if (IsMatch(composition, "_X*X_*_*"))
        {
            tileData.sprite = wallSprites[19];
        }
        else if (IsMatch(composition, "*X__X*_*"))
        {
            tileData.sprite = wallSprites[20];
        }
        else if (IsMatch(composition, "*_*X__X*"))
        {
            tileData.sprite = wallSprites[21];
        }
        else if (IsMatch(composition, "XXXXX_X_"))
        {
            tileData.sprite = wallSprites[22];
        }
        else if (IsMatch(composition, "XXXXX_XX"))
        {
            tileData.sprite = wallSprites[23];
        }
        else if (IsMatch(composition, "*X__X*X_"))
        {
            tileData.sprite = wallSprites[24];
        }
        else if (IsMatch(composition, "_X_XX*_*"))
        {
            tileData.sprite = wallSprites[25];
        }
        else if (IsMatch(composition, "*_*XX_X_"))
        {
            tileData.sprite = wallSprites[26];
        }
        else if (IsMatch(composition, "_X*X__X*"))
        {
            tileData.sprite = wallSprites[27];
        }
        else if (IsMatch(composition, "_XXXX_XX"))
        {
            tileData.sprite = wallSprites[28];
        }
        else if (IsMatch(composition, "_X_XXXXX"))
        {
            tileData.sprite = wallSprites[29];
        }
        else if (IsMatch(composition, "*XX_X*X_"))
        {
            tileData.sprite = wallSprites[30];
        }
        else if (IsMatch(composition, "*X_XX*_*"))
        {
            tileData.sprite = wallSprites[31];
        }
        else if (IsMatch(composition, "*_*XX_XX"))
        {
            tileData.sprite = wallSprites[32];
        }
        else if (IsMatch(composition, "_X*X_XX*"))
        {
            tileData.sprite = wallSprites[33];
        }
        else if (IsMatch(composition, "XX_XX_XX"))
        {
            tileData.sprite = wallSprites[34];
        }
        else if (IsMatch(composition, "_XXXXXX_"))
        {
            tileData.sprite = wallSprites[35];
        }
        else if (IsMatch(composition, "*X__X*XX"))
        {
            tileData.sprite = wallSprites[36];
        }
        else if (IsMatch(composition, "_XXXX*_*"))
        {
            tileData.sprite = wallSprites[37];
        }
        else if (IsMatch(composition, "*_*XXXX_"))
        {
            tileData.sprite = wallSprites[38];
        }
        else if (IsMatch(composition, "XX*X__X*"))
        {
            tileData.sprite = wallSprites[39];
        }
        else if (IsMatch(composition, "*X*__*X*"))
        {
            tileData.sprite = wallSprites[40];
        }
        else if (IsMatch(composition, "*_*XX*_*"))
        {
            tileData.sprite = wallSprites[41];
        }




        else if (IsMatch(composition, "_X_XXXX_"))
        {
            tileData.sprite = wallSprites[42];
        }
        else if (IsMatch(composition, "*X_XX_X_"))
        {
            tileData.sprite = wallSprites[43];
        }
        else if (IsMatch(composition, "_XXXX_X_"))
        {
            tileData.sprite = wallSprites[44];
        }
        else if (IsMatch(composition, "_X_XX_XX"))
        {
            tileData.sprite = wallSprites[45];
        }
        else if (IsMatch(composition, "XXXXXXXX"))
        {
            tileData.sprite = wallSprites[46];
        }

        else
        {
            tileData.sprite = wallSprites[0];
        }
    }
    //this could be used to animate my sprite, to make for example moving water
    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
    }
#if UNITY_EDITOR
    //This will place a new object into Unity Editors menu. Check Assets -> Create -> Tiles -> WaterTile
    [MenuItem("Assets/Create/Tiles/SolidWall1Tile")]
    public static void CreateSoliWall1Tile()
    {
        //This will execute when someone selects the WaterTile menu item.
        string path = EditorUtility.SaveFilePanelInProject("Save SolidWall1Tile", "NewSolidWall1Tile", "asset", "Saved SolidWall1Tile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SolidWall1Tile>(), path);

    }
#endif
}

public interface RogueLikeTile
{
    string GetCode();
}
