using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    public Monster(Tile startingTile, char symbol, Color color) {
        _map = startingTile.Map;
        _symbol = symbol;
        _tile = startingTile;
        _color = color;
        _isVisible = _tile.IsVisible;
    }

    public void DoTurn() {
        // Monster should take their turn
        int random_x = Random.Range(-1,2);
        int random_y = Random.Range(-1,2);
        Move(random_x,random_y);
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks
    }

    public void Move(int x, int y)
    {
        Tile targetTile;

        if (((targetTile = _map.GetTile(_tile.X + x, _tile.Y + y)) != null) && targetTile.IsPassable())
        { // if there is a tile there and it's passable
            _tile = targetTile;
            _isVisible = _tile.IsVisible;
        }

    }
}
