using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Entity
{
    
    public delegate void ItemEffectFunction(Actor target, Item self);
    public ItemEffectFunction myEffectFunction;

    Actor carriedBy = null;
    public Actor CarriedBy { get => carriedBy; set => carriedBy = value; }

    bool autoActivate = false; // set true for items that should activate when stepped on, like gold (or traps?)
    public bool AutoActivate {get => autoActivate; set => autoActivate = value; }


    public Item() {
        isCarryable = true;
        isPassable = true;
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


    public void Initialize(string name, string symbol, Color color, ItemEffectFunction itemEffectFunction=null, bool autoActivate = false)
    {
        Symbol = symbol;
        Color = color;
        Name = name;
        myEffectFunction = itemEffectFunction;
        AutoActivate = autoActivate;
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
        this.RemoveFromMap();
        if (CarriedBy != null) CarriedBy.RemoveFromInventory(this);
        Name += " (consumed)";
        OnEntityChangedCallbacks();
    }

    public void OnEntityChangedCallbacks() {
        if (cbEntityChanged != null) cbEntityChanged(this);
    }
}
