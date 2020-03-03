using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Entity
{

    public delegate void ItemEffectFunction(Actor target, Item self);
    ItemEffectFunction myEffectFunction;

    public Item(Tile startingTile, char symbol, Color color, string name, ItemEffectFunction itemEffectFunction)
    {
        Map = startingTile.Map;
        Symbol = symbol;
        Tile = startingTile;
        Color = color;
        Name = name;
        isVisible = Tile.IsVisible;
        isPassable = true;
        myEffectFunction = itemEffectFunction;
        Tile.Enter(this); // register the item with the tile
    }

    public void DoTurn() {
        isVisible = Tile.IsVisible;
        // call callbacks
        if (cbEntityChanged != null) cbEntityChanged(this);
    }

    public void ActivateItem(Actor target){
        Debug.Log("activating item!");
        myEffectFunction(target, this);
    }

    public void Consume() {
        Tile.Exit(this);
        Map.Items.Remove(this);
        isVisible=false;
        if (cbEntityChanged != null) cbEntityChanged(this);
    }
}
