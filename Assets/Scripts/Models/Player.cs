using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player:Entity
{
    int _x; //maybe don't need these, just a tile?
    int _y;


    FOVHelper fovHelper = new FOVHelper();

    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }

    public Player(Tile startingTile) // init by Tile
    {
        _x = startingTile.X;
        _y = startingTile.Y;
        _map = startingTile.Map;
        _tile = startingTile;
        fovHelper.FOV(_tile);
        
    }

    public void Move(int x, int y) {
        Tile targetTile;

        if (((targetTile = _map.GetTile(_x + x, _y + y)) != null) && targetTile.IsPassable()) { // if there is a tile there and it's passable
                _x += x;
                _y += y;
                _tile = targetTile;
        }
        fovHelper.FOV(_tile);
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }


}
