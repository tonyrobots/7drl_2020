using System.Collections;
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
    public int level = 1;

    public Weapon myWeapon;

    List<Item> inventory = new List<Item>();

    // TODO Will need to develop this system, probably also using components? Wish I were using unity more. Too late?
    public List<string> statuses = new List<string>();
    
    // components
    public Health health;

    public void Wait()
    {
        Map.Game.Log($"{Name} takes a beat to assess the situation.");

        // using this callback to represent turn end, there's probably a smarter way to handle
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }

    public bool PickUpItemAtCurrentLocation() {
        Item i = Tile.GetItemOnTile();
        if (i != null)
        {
            AddToInventory(i);  
            i.RemoveFromMap();
            Map.Game.Log($"{Name} picked up a {i.Name}");
            return true;
        } else {
            return false;
        }
    }


    bool AddToInventory(Item i) {
        if (i.GetType() == typeof(Weapon)) {
            WieldWeapon(i as Weapon);
        } else {
            inventory.Add(i);
        }
        return true;
    }

    public virtual void Die() {
        Map.Game.Log($"{Name} dies, theoretically.");
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        
    }

    public virtual float EvadeChance() {
        // this is temporary
        return (agility/100f);
    }

    public void WieldWeapon(Weapon w) {
        if (myWeapon != null && myWeapon.isCarryable ) {
            // drop current weapon if it's carryable, which should be all weapons besides 'bare hands'
            DropItem(myWeapon);
            Map.Game.Log("you drop the " + myWeapon.Name);
        }
        myWeapon = w;
        Map.Game.Log("you wield the " + myWeapon.Name); // why isn't this working?
        Debug.Log("you wield the " + myWeapon.Name);


    }

    public string ListInventoryAsString(){
        string inv = "";
        foreach (Item i in inventory)
        {
            inv += i.Name + ", ";
        }
        return inv;
    }

}
