using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class Player:Actor
{

    public FOVHelper fovHelper = new FOVHelper();
    public int XP = 0;
    public float experienceModifier = 1f;

    // This is a little tricky. It is a queue of a list of upgrade options. Each list is the 3 options presented (e.g. on level up). Since a player
    // can potentially advance multiple levels at once, this queue will store the option sets, allow the player to work through them all and select one 
    // at a time. Also useful because we want to let enemy turns finish before showing the options panel. Could be overkill.

    Queue<List<UpgradeOption>> upgradesOnDeck = new Queue<List<UpgradeOption>>();

    public Player(Tile startingTile) // init by Tile
    {
        Map = startingTile.Map;
        Tile = startingTile;
        // PlaceAtTile(startingTile);
        fovHelper.FOV(Tile);
        //Tile.Enter(this);
        health = new Health(25, this);
        Name = LoadName();
        // Name = TextAssetHelper.GetRandomLinefromTextAsset("names");
        Symbol = "@";
        Color = Color.black;
        // fovHelper.FOV(Tile);
        myWeapon = new Weapon();
        myWeapon.Initialize("Bare Hands", ".",Color.white, "1d3",2);
        DamageDice = "1d3";
        myWeapon.isCarryable = false;
        INVENTORY_LIMIT = 12;
    }

    public static string LoadName() {
        if (PlayerPrefs.HasKey("name") && PlayerPrefs.GetString("name") != "" ) {
            return PlayerPrefs.GetString("name");
        } else {
            return TextAssetHelper.GetRandomLinefromTextAsset("names");
        }
    }

    public void AttemptMove(int x, int y) {
        Tile targetTile = Map.GetTile(Tile.X + x, Tile.Y + y);

        // if there is a tile there and it's passable, move there
        if (targetTile != null && targetTile.IsPassable()) {
            Move(targetTile);
        } else if (targetTile.GetActorOnTile() != null ) {
            Actor targetEnemy = targetTile.GetActorOnTile();
            Map.Game.combat.Attack(this, targetEnemy);
            Map.Game.wc.uiManager.ShowMonsterInfo(targetEnemy as Monster);
            Map.Game.AdvanceTurn();
            if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks
        }
    
    }

    public void Move(Tile targetTile) {

        PlaceAtTile(targetTile);

        fovHelper.FOV(Tile);
        if (targetTile.GetItemOnTile() != null) {
            Item i = targetTile.GetItemOnTile();
            if (i.AutoActivate) {
                i.ActivateItem(this);
            } else {
                Map.Game.Log($"You see a {i.Name} here. (Press 'g' to pick up)");
                if (i.GetType() == typeof(Weapon)) {
                    Map.Game.wc.uiManager.ShowWeaponInfo(i as Weapon);
                }
            }
        } else {
            Map.Game.wc.uiManager.HideInfoPanel();
        }
        Map.Game.AdvanceTurn();
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }

    public void Tick(int currentTurn) {
        // do all "tick" actions that have registered. Need to figure out how to handle.
        health.Tick();
        TickConditions(); 

    }

    public override void Die() {
        if (isAlive) // can't die if you're already dead!
        {
            Map.Game.Log($"{Name} dies unceremoniously."); 
            Symbol = "%";
            Color = new Color(.4f, .2f, .2f);
            isAlive = false;
            if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks
            Map.Game.GameOver();
        }
    }

    public override void DropItem(Item i) {
        Inventory.Remove(i);
        base.DropItem(i);
        //Map.Game.AdvanceTurn();
    }

    public void UseItem(Item i) {
        i.ActivateItem(this);
        Map.Game.AdvanceTurn();
    }

    public void GainXP(int xp) {
        XP += xp;

        // gained a level?
        while (XP >= XPNeededForNextLevel()) {
            AdvanceLevel(charLevel+1);
        }
    }

    public int XPNeededForNextLevel() {
        return XPNeededForLevel(charLevel+1);
    }

    public int XPNeededForLevel(int level)
    {
        return 15 * (level-1) * level;
        // 30 - 75 - 180 - 300 - 450
        // return (5 * charLevel);
    }

    void AdvanceLevel(int newLevel) {
        charLevel = newLevel;
        Map.Game.Log($"<#448622>You are now level {charLevel}!</color>");

        health.Hitpoints = health.MaxHitpoints;

        // present some advancement options
        // let's try this decision panel thing
        upgradesOnDeck.Enqueue(UpgradeOption.GenerateUpgradeOptions(this, newLevel));
    }

    public void ProcessUpgrades() {
        if (upgradesOnDeck.Count > 0) {
            // this will just present one each turn, but at least you get all of your upgrades if you somehow advance more than one level at once.
            Map.Game.PresentUpgradeOptions(upgradesOnDeck.Dequeue(), $"<b>Welcome to level {charLevel}!</b>\nChoose one of the following:");
        }
    }

}
