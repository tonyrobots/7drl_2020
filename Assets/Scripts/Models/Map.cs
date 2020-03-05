using System.Collections.Generic;
using UnityEngine;

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

            // Temp: add a monster to the middle of the room, except for first room
            if (i>0) {
                //AddMonster( new Monster(GetTile(newRoom.Center().x, newRoom.Center().y),"M", Color.green, "scary monster") );
                //PlaceRandomMonsterForLevel(game.DungeonLevel, GetRandomFloorTile());

            } else {
                // temp
                Weapon w = new Weapon(this, "3d6+1");
                w.Initialize("Great Sword +1", "\\", Color.white);
                w.PlaceAtTile(GetTile(newRoom.Center().x+1, newRoom.Center().y));
            }

            // // Temp: add a healing potion to some places
            // if (Random.Range(0,100) > 70) {
            //     AddItem( new Item(GetTile(newRoom.Center().x-1, newRoom.Center().y),'!', Color.blue, "healing potion"  ));
            // }



        }
        GenerateHalls();

        // Temp: add some healing potions randomly around
        for (int j = 0; j < Random.Range(4, 10); j++)
        {
            // Item newItem = new Item(GetRandomFloorTile(), "!", Color.blue, "healing potion", (actor, item) => { ItemEffects.HealingPotion(actor, item, 10);});
            Item newItem = new Item(this);
            newItem.Initialize("healing potion", "!", Color.blue, (actor, item) => { ItemEffects.HealingPotion(actor, item, 10); });
            newItem.PlaceAtTile(GetRandomEmptyFloorTile());
        }

        // and a couple more monsters for good measure:
        for (int m = 0; m < 1; m++)
        {
            PlaceRandomMonsterForLevel(game.DungeonLevel, GetRandomEmptyFloorTile());
        }
        Weapon newWeapon = new Weapon(this, "3d6+1");
        newWeapon.Initialize("Great Sword +1", "\\", Color.white);
        newWeapon.PlaceAtTile(GetRandomEmptyFloorTile());
    }


    public Tile GetAppropriateStartingTile () {

        return GetTile(Rooms[0].Center().x, Rooms[0].Center().y);
        
    }

    Tile GetRandomEmptyFloorTile() {
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
            e.IsVisible = e.Tile.IsVisible;
        }
    }


    public void PlaceRandomMonsterForLevel(int level, Tile tile)
    {
        // get csv line of monster data
        string monsterString = Helpers.TextAssetHelper.GetRandomLinefromTextAsset("monsters",skipFirstLine:true).Trim();
        // separate it out into fields and assign them
        Monster newMonster = new Monster(this);

        if (monsterString == "")
        {
            Debug.LogError("Got a blank line from the monster csv...no monster for you!");
        }
        string[] lines = monsterString.Split(',');
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
        newMonster.level = System.Int32.Parse(lines[8]);
        newMonster.PlaceAtTile(tile);
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
