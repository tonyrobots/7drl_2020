using System.Collections.Generic;
using UnityEngine;
using RogueSharp.DiceNotation;


public class Map
{

    private Game game;

    private Tile[,] tiles;
    private List<Tile> floorTiles;
    private List<Rect> rooms;

    private List<Entity> entities;

    private int width;
    private int height;

    public Game Game { get => game; set => game = value; }
    public Tile[,] Tiles { get => tiles; set => tiles = value; }
    public List<Rect> Rooms { get => rooms; set => rooms = value; }

    public List<Entity> Entities { get => entities; set => entities = value; }
    public List<Tile> FloorTiles { get => floorTiles; set => floorTiles = value; }

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }

    public Map(int width, int height, Game game) {
        this.Width = width;
        this.Height = height;
        Game = game;

        Tiles = new Tile[width,height];
        floorTiles = new List<Tile>();

        Entities = new List<Entity>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[x,y] = new Tile(this, x, y);
            }
        }
        Debug.Log("Map created with " + width*height + " tiles.");
    }

    public Tile GetTile(int x, int y) {
        if (x > Width || x < 0 || y < 0 || y > Height) {
            Debug.LogError("Tile (" +  x +"," + y + ") is out of bounds.");
            return null;
        } else {
            return Tiles[x,y];
        }
    }

    public void GenerateRooms(int maxWidth=10, int maxHeight=10, int maxRooms=25, int minWidth=3, int minHeight=3, bool allowRoomOverlap=false ) {
        Rooms = new List<Rect>();
        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(minWidth, maxWidth);
            int roomHeight = Random.Range(minHeight, maxHeight);
            int x1 = Random.Range(1, Width - roomWidth);
            int y1 = Random.Range(1, Height - roomHeight);

            Rect newRoom = new Rect(x1,y1,roomWidth,roomHeight);

            // check for overlaps
            if (!allowRoomOverlap && !IsRoomLocationValid(newRoom)) {
                continue;
            }

            Rooms.Add(newRoom);
            // Debug.Log(newRoom);
            // Debug.Log("center: " + newRoom.Center());

            // dig out the room:
            for (int x = newRoom.X1; x < newRoom.X2; x++)
            {
                for (int y = newRoom.Y1; y < newRoom.Y2; y++)
                {   
                    Tile t = GetTile(x,y);
                    t.Type = Tile.TileTypes.FLOOR;
                    floorTiles.Add(t);
                    
                }
            }

            //  add a monster to the middle of the room, except for first room
            if (i>0) {
                PlaceRandomMonsterForLevel(game.DungeonLevel, GetTile(newRoom.Center().x, newRoom.Center().y));
            }


        }
        GenerateHalls();

        // Temp: add some loot randomly around
        for (int j = 0; j < Random.Range(1, 5); j++)
        {
            new Item("healing potion").PlaceAtTile(GetRandomEmptyFloorTile()); // healing potion
            GenerateLoot(game.DungeonLevel).PlaceAtTile(GetRandomEmptyFloorTile()); //random loot
        }

        // and  couple more  monsters for good measure:
        for (int m = 0; m < Random.Range(0,4); m++)
        {
            PlaceRandomMonsterForLevel(game.DungeonLevel, GetRandomEmptyFloorTile());
        }
        GenerateWeapon().PlaceAtTile(GetRandomEmptyFloorTile()); // make sure there's a weapon somewhere

        // if this is the final level, don't add stairs, and add the BOSS, let's make this nicer later :D
        if (game.DungeonLevel == game.FinalDungeonLevel) {
            // create a boss man
            Monster boss = new Monster(this);
            boss.Name = "Lemon King";
            boss.Symbol = "<b>X</b>";
            boss.strength = 65;
            boss.agility=50;
            boss.armor=4;
            boss.health = new Health(120, boss);
            // boss.health = new Health(1, boss);
            boss.DamageDice = "6d6";
            // boss.DamageDice = "1d1";
            boss.Color = Color.black;
            boss.gold = 10000;
            boss.PlaceAtTile(GetRandomEmptyFloorTile());
            Debug.Log("adding boss man at " + boss.Tile);
        } else {
            GenerateAndPlaceStairsDown(); // add some stairs
        }
    }


    public Tile GetAppropriateStartingTile () {

        return GetTile(Rooms[0].Center().x, Rooms[0].Center().y);
        
    }

    public Tile GetRandomEmptyFloorTile() {
        while (true){
            Tile t = floorTiles[Random.Range(0, floorTiles.Count)];
            if (t.entities.Count == 0) return t;
        }
    }

    public int GetManhattanDistanceBetweenTiles(Tile t1, Tile t2)
    {
        return (Mathf.Abs(t1.X - t2.X) + Mathf.Abs(t1.Y - t2.Y));
    }

    public void AddEntity(Entity e) {
        Entities.Add(e);
        Game.entitiesToRender.Enqueue(e);
    }

    public void RemoveEntity(Entity e) {
        Entities.Remove(e);
    }


    public void SetAllDungeonItemsVisibility() {
        foreach (Entity e in Entities) {
            if (e.isRemembered) {
                e.IsVisible = e.Tile.IsExplored;
            } else {
                e.IsVisible = e.Tile.IsVisible;
            }
        }
    }


    public void PlaceRandomMonsterForLevel(int level, Tile tile)
    {
        // get csv line of monster data
        string monsterString = Helpers.TextAssetHelper.GetRandomLinefromTextAsset("monsters");
        // separate it out into fields and assign them
        Monster newMonster = new Monster(this);

        if (monsterString == "")
        {
            Debug.LogError("Got a blank line from the monster csv...no monster for you!");
            return;
        }
        string[] lines = monsterString.Split(',');
        newMonster.charLevel = System.Int32.Parse(lines[8]);

        if (newMonster.charLevel > level) { // this is SLOPPY
            PlaceRandomMonsterForLevel(level, tile);
            return;
        } 


        newMonster.Name = lines[0];
        newMonster.Symbol = lines[1];
        Color c;
        if (ColorUtility.TryParseHtmlString(lines[2], out c))
        {
            newMonster.Color = c;
        }
        else
        {
            newMonster.Color = Color.black;
        }
        newMonster.armor = System.Int32.Parse(lines[3]);
        newMonster.strength = System.Int32.Parse(lines[4]);
        newMonster.agility = System.Int32.Parse(lines[5]);
        newMonster.gold = System.Int32.Parse(lines[6]);
        newMonster.health = new Health(System.Int32.Parse(lines[7]), newMonster);
        newMonster.DamageDice = lines[9];
        newMonster.PlaceAtTile(tile);
    }

    public Weapon GenerateWeapon(int Level=1) {
        string weaponString = Helpers.TextAssetHelper.GetRandomLinefromTextAsset("weapons");
        Weapon newWeapon = new Weapon();

        string[] lines = weaponString.Split(',');
        newWeapon.Name = lines[0];
        newWeapon.Symbol = lines[1];
        if (ColorUtility.TryParseHtmlString(lines[2], out Color c))
        {
            newWeapon.Color = c;
        }
        else
        {
            newWeapon.Color = Color.black;
        }        
        
        newWeapon.DamageDice = lines[3];
        newWeapon.Weight = System.Int32.Parse(lines[4]);

        if (Dice.Roll("d100") < 5) {
            // make it SHARP
            newWeapon.Sharpen();
        }

        //magical bonus?
        int roll = Dice.Roll("d100");
        if (roll < 5 ) {
            newWeapon.Name += $" +{roll}";
            newWeapon.DamageDice += $"+{roll}";
        }

        return newWeapon;
    }

    public void RevealAll() {
        foreach (Tile t in Tiles) {
            t.IsVisible = true;
        }
        SetAllDungeonItemsVisibility();
    }

    // Private methods

    void GenerateHalls()
    {
        for (int i = 1; i < Rooms.Count; i++)
        {
            Rect room = Rooms[i - 1];
            Rect nextRoom = Rooms[i];

            // if (i == 0)

            Tile startTile = GetTile(room.Center().x, room.Center().y);
            Tile endTile = GetTile(nextRoom.Center().x, nextRoom.Center().y);
            DigTunnel(startTile, endTile);
        }
    }

    public Item GenerateLoot(int level=1, int goldMax=25){
        string[] lootTypes = {"weapon","gold","item"}; // should use enum here
        string myLootType = lootTypes[Random.Range(0,lootTypes.Length )];
        Item lootItem = null;
        switch (myLootType)
        {
            case "weapon":
                lootItem = GenerateWeapon(level);
            break;

            case "gold":
                int gold = Random.Range(5,goldMax);
                lootItem = Item.GenerateGold(gold);
            break;

            case "item":
                // hardcode for now, fix later!
                string[] itemTypes = { "healing potion", "refined healing potion", "scroll of mapping", "scroll of teleportation", "wand of teleportation", "wand of healing" }; // should use enum here
                string myItemType = itemTypes[Random.Range(0, itemTypes.Length)];
                lootItem = new Item(myItemType);
            break;
            
            default:
                Debug.LogError("generate loot fell through to default, something is wrong.");
            break;
        }         

        return lootItem;
    }

    public Tile GetNextTileTowardDestination(Tile currentTile, Tile destTile) // bootleg pathing algo b/c I don't have time for A*!
    {
        // int dy = (currentTile.Y - destTile.Y);
        // int dx = (currentTile.X - destTile.X);
        int dist = GetManhattanDistanceBetweenTiles(currentTile, destTile);

        List<Tile> walkableTiles = currentTile.GetAdjacentPassableTiles();
        Tile chosenTile = null;
        int bestDist = 1000;

        foreach (Tile t in walkableTiles) {
            int newDist = GetManhattanDistanceBetweenTiles(t, destTile);
            if (newDist < bestDist) {
                bestDist=newDist;
                chosenTile = t;
            }
        }
        return chosenTile;

    }

    void GenerateAndPlaceStairsDown()
    {
        Item newStairs = new Item("down stairs"); // down stairs are up stairs for now, deal with it
        // newStairs.PlaceAtTile(GetRandomEmptyFloorTile());
        newStairs.PlaceAtTile(GetTile(rooms[1].Center().x,rooms[1].Center().y));
    }

    void DigTunnel(Tile startTile, Tile endTile) {
        // dig horiz first?

        if (Random.Range(0,2) == 1){
            DigHorizTunnel(startTile.X, endTile.X, startTile.Y);
            DigVertTunnel(startTile.Y, endTile.Y, endTile.X);
        } else {
            DigVertTunnel(startTile.Y, endTile.Y, endTile.X);
            DigHorizTunnel(startTile.X, endTile.X, startTile.Y);
        }


    }
    
    void DigHorizTunnel(int x1, int x2, int y) {

        int minX = Mathf.Min(x1,x2);
        int maxX = Mathf.Max(x1,x2);

        for (int x = minX; x <= maxX; x++)
        {
            if (GetTile(x,y) != null) {
                GetTile(x, y).Dig();
            }
        }
    }

    void DigVertTunnel (int y1, int y2, int x) {

        int minY = Mathf.Min(y1, y2);
        int maxY = Mathf.Max(y1, y2);

        for (int y = minY; y <= maxY; y++)
        {
            if (GetTile(x, y) != null)
            {
                GetTile(x, y).Dig();
            }
        }
    }

    bool IsRoomLocationValid(Rect room) {
        foreach (Rect existingRoom in Rooms)
        {
            if (room.Intersects(existingRoom)) {
                return false;
            }
        }
        return true;
    }





}
