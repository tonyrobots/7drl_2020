using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map 
{

    public Game _game;

    public Tile[,] _tiles;
    public List<Rect> _rooms;
    public List<Monster> _monsters;

    public int _width;
    public int _height;

    public Map(int width, int height, Game game) {
        this._width = width;
        this._height = height;
        _game = game;

        _tiles = new Tile[width,height];
        _monsters = new List<Monster>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tiles[x,y] = new Tile(this, x, y);
            }
        }
        Debug.Log("Map created with " + width*height + " tiles.");
    }

    public Tile GetTile(int x, int y) {
        if (x > _width || x < 0 || y < 0 || y > _height) {
            Debug.LogError("Tile (" +  x +"," + y + ") is out of bounds.");
            return null;
        } else {
            return _tiles[x,y];
        }
    }

    public void GenerateRooms(int maxWidth=10, int maxHeight=10, int maxRooms=25, int minWidth=3, int minHeight=3, bool allowRoomOverlap=false ) {
        _rooms = new List<Rect>();
        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(minWidth, maxWidth);
            int roomHeight = Random.Range(minHeight, maxHeight);
            int x1 = Random.Range(1, _width - roomWidth);
            int y1 = Random.Range(1, _height - roomHeight);

            Rect newRoom = new Rect(x1,y1,roomWidth,roomHeight);

            // check for overlaps
            if (!allowRoomOverlap && !IsRoomLocationValid(newRoom)) {
                continue;
            }

            _rooms.Add(newRoom);
            // Debug.Log(newRoom);
            // Debug.Log("center: " + newRoom.Center());

            // dig out the room:
            for (int x = newRoom.X1; x < newRoom.X2; x++)
            {
                for (int y = newRoom.Y1; y < newRoom.Y2; y++)
                {
                    GetTile(x,y).Type = Tile.TileType.FLOOR;
                }
            }

            // Temp: add a monster to the middle of the room, except for first room
            if (i>0) {
                AddMonster( new Monster(GetTile(newRoom.Center().x, newRoom.Center().y),'M', Color.green) );
            }
        }
        GenerateHalls();
    }

    void GenerateHalls() {
        for (int i = 1; i < _rooms.Count; i++)
        {
            Rect room = _rooms[i-1];
            Rect nextRoom = _rooms[i];

            // if (i == 0)

            Tile startTile = GetTile(room.Center().x, room.Center().y);
            Tile endTile = GetTile(nextRoom.Center().x, nextRoom.Center().y);
            DigTunnel(startTile, endTile);
        }
    }

    public Tile GetAppropriateStartingTile () {

        return GetTile(_rooms[0].Center().x, _rooms[0].Center().y);
        
    } 

    void DigTunnel(Tile startTile, Tile endTile) {
        // dig horiz first
        DigHorizTunnel(startTile.X, endTile.X, startTile.Y);
        DigVertTunnel(startTile.Y, endTile.Y, endTile.X);
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
        foreach (Rect existingRoom in _rooms)
        {
            if (room.Intersects(existingRoom)) {
                return false;
            }
        }
        return true;
    }


    void AddMonster(Monster monster){
        _monsters.Add(monster);
        Debug.Log("adding monster at " + monster._tile);
    }

}
