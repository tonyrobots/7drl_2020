using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Entity
{

    public delegate void ItemEffectFunction(Actor target, Item self);
    ItemEffectFunction myEffectFunction;

    public Item(Tile startingTile, string symbol, Color color, string name, ItemEffectFunction itemEffectFunction)
    {
        Map = startingTile.Map;
        Symbol = symbol;
        Tile = startingTile;
        Color = color;
        Name = name;
        IsVisible = Tile.IsVisible;
        isPassable = true;
        myEffectFunction = itemEffectFunction;
        Map.AddItem(this); // add item to the list the map maintains
        Tile.Enter(this); // register the item with the tile
        Map.Game.entitiesToRender.Enqueue(this);

    }

    public void DoTurn() {
        IsVisible = Tile.IsVisible;
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
        IsVisible=false;
        if (cbEntityChanged != null) cbEntityChanged(this);
    }

    public void OnEntityChangedCallbacks() {
        if (cbEntityChanged != null) cbEntityChanged(this);
    }
}
