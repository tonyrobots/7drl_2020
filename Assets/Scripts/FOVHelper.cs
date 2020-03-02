using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVHelper 
{

    public int VIEW_RADIUS = 7;
    public int FOV_PRECISION = 2; // 1-360, lower is better but slower

    public void FOV(Tile originTile)
    {
        float x, y;
        int i;

        //Initially set all tiles to not visible.
        HideTiles(originTile.Map);

        for (i = 0; i < 360; i+=FOV_PRECISION)
        {
            x = Mathf.Cos(i * 0.01745f);
            y = Mathf.Sin(i * 0.01745f);
            DoFov(originTile, x, y);
        }
    }

    void DoFov(Tile originTile, float x, float y)
    {
        int i;
        float ox, oy;

        Map map = originTile.Map;

        ox = originTile.X + 0.5f;
        oy = originTile.Y + 0.5f;
        // Debug.Log("doing fov for " + ox + ", " + oy);

        for (i = 0; i < VIEW_RADIUS; i++)
        {   
            Tile myTile = map.GetTile((int)ox, (int)oy);
            myTile.IsVisible = true;
            // Debug.Log("setting " + myTile + " to visible");
            // if (MAP[(int)ox][(int)oy] == BLOCK)
            if (myTile.Type == Tile.TileType.WALL) { // if blocks vision, should generalize this
                return;
            }
            ox += x;
            oy += y;
        }
    }

    void HideTiles(Map map) {
        foreach (Tile tile in map._tiles) {
            tile.IsVisible = false;
        }
    }

}
