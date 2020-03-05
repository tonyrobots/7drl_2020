using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game
{
    int turnCount = 0;
    int dungeonLevel = 1;

    WorldController wc;

    public enum GameStates
    {
        PLAYER_TURN,
        ENEMY_TURN,
        PLAYER_DEAD,
        EXPLORE_MAP,
        INVENTORY_USE,
        INVENTORY_DROP
    }

    public GameStates gamestate = GameStates.PLAYER_TURN;
    
    public Map currentMap;
    private Player player;
    private int level=1;
    public CombatSystem combat;

    public Queue<Message> messageLog;
    public Queue<Entity> entitiesToRender;

    public Map CurrentMap { get => currentMap; set => currentMap = value; }
    public int TurnCount { get => turnCount; set => turnCount = value; }
    public Player Player { get => player; set => player = value; }
    public int Level { get => level; set => level = value; }
    public int DungeonLevel { get => dungeonLevel; set => dungeonLevel = value; }


    public Game() {
        // messageLog = new MessageLog(this);
        messageLog = new Queue<Message>();
        entitiesToRender = new Queue<Entity>();
        combat = new CombatSystem(this);

        // this is (should be) the ONLY reference to a unity object within the models layer
        wc = Object.FindObjectOfType<WorldController>();
        
    }

    public void AdvanceTurn(int t = 1)
    {

        //WC ProcessEntitiesQueue();

        TurnCount++;
        Player.Tick(TurnCount);

        // WC uiManager.UpdatePlayerStats(game);

        gamestate = Game.GameStates.ENEMY_TURN;
        for (int i = 0; i < t; i++)
        {
            // DoMonsterTurns();
            // DoItemTurns();
            DoEntityTurns();
        }
        if (gamestate == Game.GameStates.PLAYER_DEAD)
        {
            GameOver();
            return;
        }
        // wC uiManager.UpdatePlayerStats(game);
        wc.FinalizeTurn();
        gamestate = Game.GameStates.PLAYER_TURN;
        // WC uiManager.UpdateMessageLog(game);
    }

    public void DoEntityTurns()
    {
        foreach (Entity e in currentMap.Entities)
        {
            e.DoTurn();
        }
    }

    public void OpenPlayerInventory(GameStates newState=GameStates.PLAYER_TURN) {
        if (newState == GameStates.PLAYER_TURN) {
            wc.uiManager.ToggleInventory(player);
        } else {
            wc.uiManager.ShowInventory(player);
            gamestate = newState;
        }
    }

    public void ClosePlayerInventory() {
        wc.uiManager.HideInventory();
        gamestate = GameStates.PLAYER_TURN;

    }

    public void Log(string simpleMessage) {
        //Debug.Log(simpleMessage);
        Message m = new Message(simpleMessage);
        messageLog.Enqueue(m);
    }

    public void GameOver()
    {   
        Log("Game is over. Hit space to restart.");

    }
}
