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

    public int numberOfUses = 1;
    public bool isConsumable = false;

    public Item() {
        isCarryable = true;
        isPassable = true;
    }

    // convenience constuctor....This is pretty dumb, should just use little classes or something...or at least an enum of item types
    public Item(string type)  {
        isCarryable = true;
        isPassable = true;

        switch (type)
        {
            case "healing potion":
                Initialize("Healing Potion", "!", Color.blue, (actor, item) => { ItemEffects.HealingPotion(actor, item, 10); });
                isConsumable = true;
                break;

            case "down stairs":
                Initialize("Stairs Down", ">", Color.black, (actor, item) => { ItemEffects.DescendLevel(actor, item, 0); });
                isCarryable=false;
                autoActivate=true;
                break;

            case "scroll of mapping":
                Initialize("Scroll of Magic Map", "?", Color.cyan, (actor, item) => { ItemEffects.RevealMap(actor, item, 0); });
                isConsumable = true;
                break;
            
            case "scroll of teleportation":
                Initialize("Scroll of Teleportation", "?", Color.magenta, (actor, item) => { ItemEffects.Teleport(actor, item, 0); });
                isConsumable = true;
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
            if (!isConsumable || numberOfUses > 0) {
                myEffectFunction(target, this);
                if (isConsumable) {
                    numberOfUses--;
                    if (numberOfUses == 0) this.Consume();
                }
            }
        } 
    }

    // public void ActivateItem(Actor target){
    //     if (myEffectFunction != null) {
    //         myEffectFunction(target, this);
    //     } 
    // }

    public void Consume() {
        Debug.Log($"consuming {Name}");
        this.RemoveFromMap();
        if (CarriedBy != null) CarriedBy.RemoveFromInventory(this);
        Name += " (consumed)";
        // kill gameobject somehow?
        OnEntityChangedCallbacks();
    }

    public void OnEntityChangedCallbacks() {
        if (cbEntityChanged != null) cbEntityChanged(this);
    }

    public static Item GenerateGold(int amount) {
        Item newGold = new Item();
        newGold.Initialize($"{amount} gold", "$", new Color(.4f, .4f, .2f, .5f), (actor, item) => { ItemEffects.Gold(actor, item, amount); }, true);
        newGold.isConsumable=true;
        newGold.numberOfUses=1;
        return newGold;
    }
}
