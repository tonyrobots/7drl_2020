using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{

    public Sprite wallSprite;
    public Sprite floorSprite;
    public Color lightColor;
    public Color dimColor;

    public bool useAscii = false;

    Map map;
    Player player;
    Game game;

    public GameObject player_go;
    public UIManager uiManager;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();

        // big map
        map = new Map(60,40, game); 
        map.GenerateRooms(10,10,25,4,3);

        // small map
        // map = new Map(25,15, game); 
        // map.GenerateRooms(10,10,3,3,3);

        game.CurrentMap = map;

        player = new Player(map.GetAppropriateStartingTile());
        playerController = player_go.GetComponent<PlayerController>();
        playerController.Player_data = player;
        playerController.Refresh(player, player_go);

        game.Player = player;


        GenerateMapGameObjects(map);

        // why is this necessary?
        map.SetAllDungeonItemsVisibility();

        // this is also probably wrong :O
        GenerateMonsterGameObjects();
        GenerateItemGameObjects();

        uiManager.UpdatePlayerStats(game);
        uiManager.UpdateMessageLog(game);


        // Listen for player move event and end turn when it happens (this is probably very wrong way to go)
        player.RegisterEntityChangedCallback((entity) => { AdvanceTurn(); });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrepGame(){
        uiManager.UpdatePlayerStats(game);

        game.gamestate = Game.GameStates.PLAYER_TURN;
        uiManager.UpdateMessageLog(game);

    }

    public void AdvanceTurn(int t = 1)
    {
        game.TurnCount++;
        game.Player.Tick(game.TurnCount);

        uiManager.UpdatePlayerStats(game);
        if (game.gamestate == Game.GameStates.PLAYER_DEAD) {
            GameOver();
            return;
        }
        game.gamestate = Game.GameStates.ENEMY_TURN; 
        for (int i = 0; i < t; i++)
        {
            DoMonsterTurns();
            DoItemTurns();
        }
        uiManager.UpdatePlayerStats(game);

        game.gamestate = Game.GameStates.PLAYER_TURN;
        uiManager.UpdateMessageLog(game);
    }

    public void DoMonsterTurns()
    {
        foreach (Monster monster in map.Monsters)
        {
            monster.DoTurn();
        }
    }

    public void DoItemTurns()
    {
        foreach (Item item in map.Items)
        {
            item.DoTurn();
        }
    }

    void GenerateMapGameObjects(Map map) {

        Transform parent = GameObject.Find("Tiles").GetComponent<Transform>();
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                GameObject tile_go = new GameObject("Tile_" + x + "_" + y);
                Tile tile_data = map.GetTile(x,y);
                tile_data.RegisterTileChangedCallback( (tile) => { RenderTile(tile, tile_go);});

                tile_go.transform.position = new Vector3(x,y,0);  
                tile_go.transform.parent = parent;

                if (useAscii) {
                    TextMeshPro tmp = tile_go.AddComponent<TextMeshPro>();
                    tmp.sortingOrder=-1;
                } else {
                    SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
                    sr.sortingLayerName = "Floor";
                }

                RenderTile(tile_data, tile_go);
            }
            
        }
    }

    void RenderTile(Tile tile, GameObject tile_go){
        if (useAscii) {
            TextMeshPro tmp = tile_go.GetComponent<TextMeshPro>();
            if (tile.IsVisible)
            {
                tmp.color = lightColor;
            }
            else if (tile.IsExplored)
            {
                tmp.color = dimColor;
            }
            else
            {
                tmp.color = Color.black;
            }
            switch (tile.Type) {
                case Tile.TileTypes.WALL:
                    tmp.text = "#";
                    tmp.alignment = TextAlignmentOptions.Center;

                    break;
                case Tile.TileTypes.FLOOR:
                    tmp.text = ".";
                    tmp.alignment = TextAlignmentOptions.Center;

                    break;
                default:
                    break;
            }

        } else    
        {        //Debug.Log("update tile game object: " + tile_go.name);
            SpriteRenderer sr = tile_go.GetComponent<SpriteRenderer>();
            if (tile.IsVisible)  {
                sr.color = lightColor;
            } else if (tile.IsExplored) {
                sr.color = dimColor;
            } else {
                sr.color = Color.black;
            }
            switch (tile.Type)
            {
                case Tile.TileTypes.WALL:
                    sr.sprite = wallSprite;
                    break;


                case Tile.TileTypes.FLOOR:
                    sr.sprite = floorSprite;
                    break;

                default:
                    break;
            }
        }
    }

    void GenerateMonsterGameObjects() {
        foreach (Monster monster in map.Monsters) {
            CreateMonsterGO(monster);
        }
    }

    void GenerateItemGameObjects() {
        foreach (Item item in map.Items) {
            CreateItemGO(item);
        }
    }

    void CreateItemGO(Item item) {

        // again is there some way to reduce duplication between this and monster equivalents?
        GameObject go = new GameObject(item.Name);
        ItemController ic = go.AddComponent<ItemController>();

        ic.Item_data = item;

        Transform parent = GameObject.Find("Items").GetComponent<Transform>();

        go.transform.position = new Vector3(item.Tile.X, item.Tile.Y, 0);
        go.transform.parent = parent;
        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = item.Symbol.ToString();
        tmp.fontSize = 9;
        tmp.color = item.Color;
        tmp.alignment = TextAlignmentOptions.Center;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(1,1);
        ic.Refresh(item, go);
    
    }

    void CreateMonsterGO(Monster monster)
    {
        GameObject monster_go = new GameObject(monster.Name);
        MonsterController mc = monster_go.AddComponent<MonsterController>();

        mc.Monster_data = monster;

        Transform parent = GameObject.Find("Monsters").GetComponent<Transform>();

        monster_go.transform.position = new Vector3(monster.Tile.X, monster.Tile.Y, 0);
        monster_go.transform.parent = parent;
        TextMeshPro tmp = monster_go.AddComponent<TextMeshPro>();
        tmp.text = monster.Symbol.ToString();
        tmp.fontSize = 9;
        tmp.color = monster.Color;
        tmp.alignment = TextAlignmentOptions.Center;
        monster_go.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        mc.Refresh(monster, monster_go);
    }

    void GameOver() {
        player.UnregisterEntityChangedCallback((entity) => { AdvanceTurn(); });

        game.Log("Game is over. Hit space to restart.");

    }
}
