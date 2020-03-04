using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Entity
{

    public delegate void ItemEffectFunction(Actor target, Item self);
    ItemEffectFunction myEffectFunction;

    public Item(Tile startingTile, string symbol, Color color, string name, ItemEffectFunction itemEffectFunction=null)
    {
        Map = startingTile.Map;
        Symbol = symbol;
        Tile = startingTile;
        Color = color;
        Name = name;
        IsVisible = Tile.IsVisible;
        isPassable = true;
        myEffectFunction = itemEffectFunction;
        Map.AddEntity(this);
        Map.Game.entitiesToRender.Enqueue(this);

    }

    public override void DoTurn() {
        IsVisible = Tile.IsVisible;
        // call callbacks
        if (cbEntityChanged != null) cbEntityChanged(this);
    }

    public void ActivateItem(Actor target){
        Debug.Log($"activating {this.Name} ");
        myEffectFunction(target, this);
    }

    public void Consume() {
        Tile.Exit(this);
        // Map.Items.Remove(this);
        Map.Entities.Remove(this);
        IsVisible=false;
        if (cbEntityChanged != null) cbEntityChanged(this);
    }

    public void OnEntityChangedCallbacks() {
        if (cbEntityChanged != null) cbEntityChanged(this);
    }
}
