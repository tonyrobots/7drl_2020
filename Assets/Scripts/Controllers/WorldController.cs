using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldController : MonoBehaviour
{

    public Sprite wallSprite;
    public Sprite floorSprite;
    public Color lightColor;
    public Color dimColor;

    Map map;
    Player player;
    Game game;

    public GameObject player_go;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();

        map = new Map(50,25, game); 
        map.GenerateRooms(10,10,20,3,3);

        game.CurrentMap = map;

        player = new Player(map.GetAppropriateStartingTile());
        playerController = player_go.GetComponent<PlayerController>();
        playerController.Player_data = player;
        playerController.Refresh(player, player_go);
        
        GenerateMapGameObjects(map);

        // this is also probably wrong :P
        GenerateMonsterGameObjects();

        // Listen for player move event and end turn when it happens (this is probably very wrong way to go)
        player.RegisterEntityChangedCallback((entity) => { AdvanceTurn(); });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceTurn(int t = 1)
    {
        game._gamestate = Game.GameState.ENEMY_TURN; 
        for (int i = 0; i < t; i++)
        {
            DoMonsterTurns();
        }
        game.TurnCount++;
        // Debug.Log($"turn: {game.TurnCount}");
        game._gamestate = Game.GameState.PLAYER_TURN;
    }

    public void DoMonsterTurns()
    {
        foreach (Monster monster in map._monsters)
        {
            monster.DoTurn();
            // Debug.Log("doing monster turn for monster at " + monster._tile);
        }
    }

    void GenerateMapGameObjects(Map map) {

        Transform parent = GameObject.Find("Tiles").GetComponent<Transform>();
        for (int x = 0; x < map._width; x++)
        {
            for (int y = 0; y < map._height; y++)
            {
                GameObject tile_go = new GameObject("Tile_" + x + "_" + y);
                Tile tile_data = map.GetTile(x,y);
                tile_data.RegisterTileChangedCallback( (tile) => { RenderTile(tile, tile_go);});

                tile_go.transform.position = new Vector3(x,y,0);  
                tile_go.transform.parent = parent;
                SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
                sr.sortingLayerName = "Floor";
                RenderTile(tile_data, tile_go);
            }
            
        }
    }

    void RenderTile(Tile tile, GameObject tile_go){
        //Debug.Log("update tile game object: " + tile_go.name);
        SpriteRenderer sr = tile_go.GetComponent<SpriteRenderer>();
        if (tile.IsVisible)  {
            sr.color = lightColor;
        } else if (tile.IsRevealed) {
            sr.color = dimColor;
        } else {
            sr.color = Color.black;
        }
        switch (tile.Type)
        {
            case Tile.TileType.WALL:
                sr.sprite = wallSprite;
                break;


            case Tile.TileType.FLOOR:
                sr.sprite = floorSprite;
                break;

            default:
                break;
        }
    }

    void GenerateMonsterGameObjects() {
        foreach (Monster monster in map._monsters) {
            CreateMonsterGO(monster);
        }
    }

    void CreateMonsterGO(Monster monster)
    {
        GameObject monster_go = new GameObject("Monster_X");
        MonsterController mc = monster_go.AddComponent<MonsterController>();

        mc.Monster_data = monster;

        Transform parent = GameObject.Find("Monsters").GetComponent<Transform>();


        monster_go.transform.position = new Vector3(monster._tile.X, monster._tile.Y, 0);
        monster_go.transform.parent = parent;
        TextMeshPro tmp = monster_go.AddComponent<TextMeshPro>();
        tmp.text = monster._symbol.ToString();
        tmp.fontSize = 9;
        tmp.color = monster._color;
        monster_go.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);


        // mc.Refresh(monster, monster_go);
    }
}
