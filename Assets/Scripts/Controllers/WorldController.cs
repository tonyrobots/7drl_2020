﻿using System.Collections;
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

    Map map;
    Player player;
    Game game;

    public GameObject player_go;
    PlayerController playerController;
    RogueSharp.PathFinder pf;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();

        map = new Map(60,40, game); 
        map.GenerateRooms(10,10,25,4,3);

        game.CurrentMap = map;

        player = new Player(map.GetAppropriateStartingTile());
        playerController = player_go.GetComponent<PlayerController>();
        playerController.Player_data = player;
        playerController.Refresh(player, player_go);

        game._player = player;


        GenerateMapGameObjects(map);

        // this is also probably wrong :O
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
        if (game.gamestate == Game.GameStates.PLAYER_DEAD) {
            GameOver();
            return;
        }
        game.gamestate = Game.GameStates.ENEMY_TURN; 
        for (int i = 0; i < t; i++)
        {
            DoMonsterTurns();
        }
        game.TurnCount++;
        game.gamestate = Game.GameStates.PLAYER_TURN;
        game._player.Tick(game.TurnCount);
    }

    public void DoMonsterTurns()
    {
        foreach (Monster monster in map.Monsters)
        {
            monster.DoTurn();
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

    void GenerateMonsterGameObjects() {
        foreach (Monster monster in map.Monsters) {
            CreateMonsterGO(monster);
        }
    }

    void CreateMonsterGO(Monster monster)
    {
        GameObject monster_go = new GameObject("Monster_X");
        MonsterController mc = monster_go.AddComponent<MonsterController>();

        mc.Monster_data = monster;

        Transform parent = GameObject.Find("Monsters").GetComponent<Transform>();

        monster_go.transform.position = new Vector3(monster.Tile.X, monster.Tile.Y, 0);
        monster_go.transform.parent = parent;
        TextMeshPro tmp = monster_go.AddComponent<TextMeshPro>();
        tmp.text = monster.Symbol.ToString();
        tmp.fontSize = 9;
        tmp.color = monster.Color;
        monster_go.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        mc.Refresh(monster, monster_go);
    }

    void GameOver() {
        player.UnregisterEntityChangedCallback((entity) => { AdvanceTurn(); });

        game.Log("Game is over. Hit space to restart.");

    }
}
