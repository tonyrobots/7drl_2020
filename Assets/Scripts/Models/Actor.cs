﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : Entity
{

    public bool isAlive = true;

    //stats
    public int strength = 10;
    public int agility = 10;
    public int armor = 0;
    public int gold = 0;
    public int charLevel = 1;
    string damageDice;
    public string DamageDice { get => damageDice; set => damageDice = value; }

    public Weapon myWeapon;

    List<Item> inventory = new List<Item>();
    public int INVENTORY_LIMIT = 4;

    public List<Item> Inventory { get => inventory; set => inventory = value; }


    // TODO Will need to develop this system, probably also using components? Wish I were using unity more. Too late?
    public List<string> statuses = new List<string>();
    
    // components
    public Health health;


    public void Wait()
    {
        Map.Game.Log($"{Name} takes a beat to assess the situation.");

        //if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        
        Map.Game.AdvanceTurn();

    }

    public bool PickUpItemAtCurrentLocation() {
        Item i = Tile.GetItemOnTile();
        if (i != null)
        {

            if (AddToInventory(i)) 
            {
                i.RemoveFromMap();
            }
            if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

            if (this.GetType() == typeof(Player)) // if it's a player doing the picking up, that's the end of the turn
            {
                Map.Game.AdvanceTurn();
            }

            return true;
        } else {
            return false;
        }

    }

    bool AddToInventory(Item i) {
        if (i.GetType() == typeof(Weapon)) {
            WieldWeapon(i as Weapon);
            i.CarriedBy = this;
            return true;
        } else if (inventory.Count < INVENTORY_LIMIT) {
            Map.Game.Log($"{Name} picks up a {i.Name}.");
            Inventory.Add(i);
            i.CarriedBy = this;
            return true;
        } else {
            Map.Game.Log("Can't pick that up because your inventory is full.");
            return false;
        }
    }

    public void RemoveFromInventory(Item i) {
        if (Inventory.Contains(i)){
            Inventory.Remove(i);
            i.CarriedBy = null;
        }
    }

    public virtual void Die() { 
        if (isAlive) // can't die if you're already dead!
        {    
            Map.Game.Log($"{Name} dies, theoretically.");
            if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks
        }        
    }

    public virtual float EvadeChance() {
        // this is temporary
        return (agility/100f);
    }

    public virtual int XPvalue() {
        return 0;
    }

    // Note: currently there's no way to unwield a weapon without wielding another one. 
    // Will probably want to change that, and when I do, will have to deal with re-equiping 'bare hands'

    public void WieldWeapon(Weapon w) { 
        if (myWeapon != null && myWeapon.isCarryable ) {
            // drop current weapon if it's carryable, which should be all weapons besides 'bare hands'
            DropItem(myWeapon);
            Map.Game.Log($"You drop the {myWeapon.Name}.");
        }
        myWeapon = w;
        Map.Game.Log($"You wield the {myWeapon.Name}."); 
        DamageDice = w.DamageDice;
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }

    public string ListInventoryAsString(){
        string inv = "";
        foreach (Item i in Inventory)
        {
            inv += i.Name + ", ";
        }
        return inv;
    }

}
