using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : Actor
{
    // if alert countdown > 0, monster is aware of player and moving toward them
    int alertCountdown = 0;

    public Monster(Tile startingTile, char symbol, Color color, string name) {
        Map = startingTile.Map;
        Symbol = symbol;
        Tile = startingTile;
        Color = color;
        Name = name;
        isVisible = Tile.IsVisible;
        isAlive = true;
        health = new Health(Random.Range(2,15), this);
    }



    public void DoTurn() {
        // Monster should take their turn (even dead ones!)

        if (isAlive) {
        // replace this with some actual "AI" soon:
            if (alertCountdown > 0) {
                MoveTowardPlayer();
            } else {
                MoveRandomly();
            }


            // replace this something that iterates through all registered "tick" objects (or the other way 'round, somehow)
            health.Tick();
        }

        // if you can see its tile, you can see the monster
        isVisible = Tile.IsVisible;
        // and it can see you, so it's alerted
        if (isVisible) alertCountdown = 12;
        if (!isVisible) alertCountdown--;

        // call callbacks
        if (cbEntityChanged != null) cbEntityChanged(this); 

    }

    public void Move(int x, int y)
    {
        Tile targetTile;
        if (((targetTile = Map.GetTile(Tile.X + x, Tile.Y + y)) != null) && targetTile.IsPassable())
        { // if there is a tile there and it's passable
            Tile.Exit(this);
            Tile = targetTile;
            Tile.Enter(this);
           
        }

    }

    public void MoveRandomly() {
        int random_x = Random.Range(-1, 2);
        int random_y = Random.Range(-1, 2);
        Move(random_x, random_y);
    }

    public void MoveTowardPlayer() {
        Tile playerTile = Map.Game.Player.Tile;
        if (Map.GetManhattanDistanceBetweenTiles(Tile, playerTile) == 1) {
            Attack(Map.Game.Player);
        } else {
            // replace this with some sort of pathfinding to player (a*?)
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
        Symbol = '%';
        Color = new Color(.4f,.2f,.1f);
        isAlive = false;
        isPassable = true;
        Name += " corpse";

        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }
}
