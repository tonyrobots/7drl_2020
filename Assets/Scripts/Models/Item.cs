using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public Item(Tile startingTile, char symbol, Color color, string name)
    {
        Map = startingTile.Map;
        Symbol = symbol;
        Tile = startingTile;
        Color = color;
        Name = name;
        isVisible = Tile.IsVisible;
    }

    public void DoTurn() {
        isVisible = Tile.IsVisible;
        // call callbacks
        if (cbEntityChanged != null) cbEntityChanged(this);
    }
}
