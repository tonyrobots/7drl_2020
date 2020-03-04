using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class Player:Actor
{

    public FOVHelper fovHelper = new FOVHelper();

    public Player(Tile startingTile) // init by Tile
    {
        Map = startingTile.Map;
        Tile = startingTile;
        fovHelper.FOV(Tile);
        //Tile.Enter(this);
        health = new Health(20, this);
        Name = "Player";
        Name = TextAssetHelper.GetRandomLinefromTextAsset("names");
        Symbol = "@";
        Color = Color.black;
        // fovHelper.FOV(Tile);
    }

    public void AttemptMove(int x, int y) {
        Tile targetTile = Map.GetTile(Tile.X + x, Tile.Y + y);

        // if there is a tile there and it's passable, move there
        if (targetTile != null && targetTile.IsPassable()) {
            Move(targetTile);
        } else if (targetTile.GetActorOnTile() != null ) {
            Actor targetEnemy = targetTile.GetActorOnTile();
            Map.Game.combat.Attack(this, targetEnemy);
            if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks
        }
    
    }

    void Move(Tile targetTile) {
        // de-register from current tile (is there a nicer way to handle this?)
        //Tile.Exit(this);
        Tile = targetTile;
        //Tile.Enter(this);
        fovHelper.FOV(Tile);
        if (targetTile.GetItemOnTile() != null) {
            Item i = targetTile.GetItemOnTile();
            // Map.Game.Log($"You see a {i.Name} here.");
            i.ActivateItem(this);
        }
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }

    // void Attack(Actor targetEnemy) {
    //     Map.Game.Log($"You swing at {targetEnemy.Name}");
    //     targetEnemy.health.TakeDamage(Random.Range(4,10));
    //     if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks
    // }

    public void Tick(int currentTurn) {
        // do all "tick" actions that have registered. Need to figure out how to handle.
        health.Tick();
    }

    public override void Die() {
        Map.Game.Log($"{Name} dies unceremoniously.");
        Symbol = "%";
        Color = new Color(.4f, .2f, .2f);
        isAlive = false;
        Map.Game.gamestate = Game.GameStates.PLAYER_DEAD;
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks

    }
}
