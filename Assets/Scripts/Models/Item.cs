using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Entity
{
    
    public delegate void ItemEffectFunction(Actor target, Item self);
    ItemEffectFunction myEffectFunction;


    public Item(Map map) {
        Map = map;
        isCarryable = true;

    }

    // convenience constuctor, should store this data in a text file or something
    public Item(Map map, string type)  {
        Map = map;
        isCarryable = true;


        switch (type)
        {
            case "healing potion":
                Initialize("healing potion", "!", Color.blue, (actor, item) => { ItemEffects.HealingPotion(actor, item, 10); });
                break;
            default:
                break;
        }
    }


    public void Initialize(string name, string symbol, Color color,  ItemEffectFunction itemEffectFunction=null)
    {
        Symbol = symbol;
        Color = color;
        Name = name;
        isPassable = true;
        myEffectFunction = itemEffectFunction;
    }

    // PlaceAtTile() is defined in Entity.cs

    public override void DoTurn() {
        IsVisible = Tile.IsVisible;
        // call callbacks
        OnEntityChangedCallbacks();
    }

    public void ActivateItem(Actor target){
        if (myEffectFunction != null) {
            Debug.Log($"activating {this.Name} ");
            myEffectFunction(target, this);
        }
    }

    public void Consume() {
        Debug.Log($"consuming {Name}");
        Tile.Exit(this);
        Map.Entities.Remove(this);
        Name += " (consumed)";
        IsVisible=false;
        OnEntityChangedCallbacks();
    }

    public void OnEntityChangedCallbacks() {
        if (cbEntityChanged != null) cbEntityChanged(this);
    }
}
