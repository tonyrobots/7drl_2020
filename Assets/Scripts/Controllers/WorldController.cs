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
    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();

        // big map
        // map = new Map(60,40, game); 
        // map.GenerateRooms(10,10,25,4,3);

        // small map
        map = new Map(25,15, game); 
        map.GenerateRooms(10,10,3,3,3);

        game.CurrentMap = map;

        player = new Player(map.GetAppropriateStartingTile());
        playerController = player_go.GetComponent<PlayerController>();
        playerController.Player_data = player;
        playerController.Refresh(player, player_go);

        game.Player = player;


        GenerateMapGameObjects(map);

        // why is this necessary?
        map.SetAllDungeonItemsVisibility();

        // uiManager.InitializeUIManager(game);
        uiManager.UpdatePlayerStats(game);
        uiManager.UpdateMessageLog(game);
        uiManager.HideInventory();

        inputManager = gameObject.AddComponent<InputManager>();
        inputManager.Game = game;

        // generate gameobjects from the entities to render queue
        ProcessEntitiesQueue();

        // Listen for player move event and end turn when it happens (this is probably very wrong way to go)
        //player.RegisterEntityChangedCallback((entity) => { AdvanceTurn(); });
        
    }

    // Much of this (AdvanceTurn, Do Monster & Item turns) should be in Game object probably
    public void FinalizeTurn()
    {
        ProcessEntitiesQueue();
        uiManager.UpdatePlayerStats(game);
        uiManager.UpdateMessageLog(game);
    }




    void ProcessEntitiesQueue() {
        while (game.entitiesToRender.Count > 0)
        {
            CreateEntityGO(game.entitiesToRender.Dequeue());
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

    void CreateEntityGO(Entity e)
    {

        GameObject go = new GameObject(e.Name);
        Transform parent;

        if ((e.GetType() == typeof(Item)) || (e.GetType() == typeof(Weapon) )) {
            ItemController controller = go.AddComponent<ItemController>();
            controller.Item_data =  e as Item;
            parent = GameObject.Find("Items").GetComponent<Transform>();
            go.transform.parent = parent;
        } else if (e.GetType() == typeof(Monster)) {
            MonsterController controller = go.AddComponent<MonsterController>();
            controller.Monster_data = e as Monster;
            parent = GameObject.Find("Monsters").GetComponent<Transform>();
            go.transform.parent = parent;
        }

        go.transform.position = new Vector3(e.Tile.X, e.Tile.Y, 0);
        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = e.Symbol.ToString();
        tmp.fontSize = 9;
        tmp.color = e.Color;
        tmp.alignment = TextAlignmentOptions.Center;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        // controller.Refresh(e, go);


    }


}
