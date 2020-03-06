using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : Actor

{
    // if alert countdown > 0, monster is aware of player and moving toward them (not implemented)
    int alertCountdown = 0;

    public Monster(Map map) {
        Map=map;
    }


    public void Initialize(char symbol, string hexcolor, string name) {
        // set up with params
        // place at tile
    }


    public override void DoTurn() {
        // Monster should take their turn (even dead ones!)

        if (isAlive) {

        // replace this something that iterates through all registered "tick" objects (or the other way 'round, somehow)
        health.Tick();

        // replace this with some actual "AI" soon:
            if (alertCountdown > 0) {
                MoveTowardPlayer();
            } else {
                MoveRandomly();
            }


        }

        // if you can see its tile, you can see the monster
        IsVisible = Tile.IsVisible;
        // and it can see you, so it's alerted
        if (IsVisible) alertCountdown = 12;
        if (!IsVisible) alertCountdown--;

        // call callbacks
        if (cbEntityChanged != null) cbEntityChanged(this); 

    }

    public void Move(int x, int y)
    {
        Tile targetTile;
        if (((targetTile = Map.GetTile(Tile.X + x, Tile.Y + y)) != null) && targetTile.IsPassable())
        { // if there is a tile there and it's passable
            //Tile.Exit(this);
            // Tile = targetTile;
            //Tile.Enter(this);
            PlaceAtTile(targetTile);           
        }

    }

    public void MoveRandomly() {
        int random_x;
        int random_y;
        if (UnityEngine.Random.Range(0,1) == 0) {
             random_x = Random.Range(-1, 2);
             random_y =0;
        } else {
             random_y = Random.Range(-1, 2);
             random_x =0;
        }
        Move(random_x, random_y);
    }

    public void MoveTowardPlayer() {
        Tile playerTile = Map.Game.Player.Tile;
        if (Map.GetManhattanDistanceBetweenTiles(Tile, playerTile) == 1) {
            Map.Game.combat.Attack(this,Map.Game.Player);
        } else {
            MoveRandomly();
        }
    }

    void Attack(Actor target) {
        Map.Game.Log ($"{Name} bites {target.Name}");
        // determine if it's a hit
        // determine damage
        // inflict damage
        target.health.TakeDamage(4);
        if (Random.Range(0,3) == 2) target.health.InflictBleeding(3);    
    }

    public override void Die() {
        Map.Game.Log($"{Name} dies.");
        Symbol = "%";
        Color = new Color(.6f,.4f,.2f,.4f);
        isAlive = false;
        isPassable = true;
        Name += " corpse";
        DropGold();
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }

    public void DropGold() {
        if (gold > 0) {
            DropItem(Item.GenerateGold(gold));
        }
    }

    public override int XPvalue() {
        return (health.MaxHitpoints + strength + agility); //this is a terrible XP formula, TODO fix it!
    }

}
