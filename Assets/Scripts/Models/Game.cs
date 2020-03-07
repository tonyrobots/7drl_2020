using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game
{
    int turnCount = 0;
    int dungeonLevel = 1;
    int messageLogLimit = 8;

    public WorldController wc;

    public enum GameStates
    {
        PLAYER_TURN,
        ENEMY_TURN,
        PLAYER_DEAD,
        EXPLORE_MAP,
        INVENTORY_USE,
        INVENTORY_DROP,
        UPGRADE_MENU
    }

    public GameStates gamestate = GameStates.PLAYER_TURN;
    public bool allowDiagonalMovement=true;
    
    private Map currentMap;
    private Player player;
    private int level=1;
    public CombatSystem combat;

    public Queue<Message> messageLog;
    public Queue<Entity> entitiesToRender;
    List<UpgradeOption> currentlyOfferedOptions = null;

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

        currentMap = GenerateMap();

        // this is (should be) the ONLY reference to a unity object within the models layer
        wc = Object.FindObjectOfType<WorldController>();
        
        player = new Player(currentMap.GetAppropriateStartingTile());
        if (wc.useAscii) {
            player.Color = Color.white;
        }
        
    }

    public void AdvanceTurn(int t = 1)
    {

        //WC ProcessEntitiesQueue();

        TurnCount++;

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
            return;
        }
        // wC uiManager.UpdatePlayerStats(game);
        wc.FinalizeTurn();
        Player.Tick(TurnCount);
        gamestate = Game.GameStates.PLAYER_TURN;
        Player.ProcessUpgrades();
        // WC uiManager.UpdateMessageLog(game);
    }

    public void DoEntityTurns()
    {
        for (int i = 0; i < currentMap.Entities.Count; i++)
        {
            currentMap.Entities[i].DoTurn();
        }
    }

    public void OpenPlayerInventory(GameStates newState=GameStates.PLAYER_TURN) {
        if (newState == GameStates.PLAYER_TURN) {
            wc.uiManager.ToggleInventory(player);
        } else {
            gamestate = newState;
            wc.uiManager.ShowInventory(player);
        }
    }

    public void ClosePlayerInventory() {
        wc.uiManager.HideInventory();
        gamestate = GameStates.PLAYER_TURN;

    }

    public void PresentUpgradeOptions(List<UpgradeOption> options, string headerText = "Choose one of the following:") {
        currentlyOfferedOptions = options;
        ClosePlayerInventory();
        gamestate = GameStates.UPGRADE_MENU;
        wc.uiManager.ShowDecisionPanel(player, options, headerText);
    }

    public void CloseOptionsPanel() {
        wc.uiManager.HideDecisionPanel();
        gamestate = GameStates.PLAYER_TURN;
    }

    public void ChooseOption(int n) {
        Debug.Log($"you chose option {n+1}");
        if (currentlyOfferedOptions != null) {
            currentlyOfferedOptions[n].MyOptionEffect();
            CloseOptionsPanel();
        }
        currentlyOfferedOptions = null;
    }

    public void Log(string simpleMessage) {
        Message m = new Message(simpleMessage);
        messageLog.Enqueue(m);
        Debug.Log(m.messageText);

        if (messageLog.Count > messageLogLimit) {
            messageLog.Dequeue();
        }
    }

    public void GameOver()
    {   
        Log("Game is over. Hit space to restart.");
        wc.FinalizeTurn();

    }

    public void DescendToNextLevel() {
        ChangeDungeonLevel(dungeonLevel+1);
    }

    public void ChangeDungeonLevel(int newLevel) {
        // clear out old level
        // old map objects will just be handled by GC, we just have to handle the unity GOs

        // clear tile callbacks
        foreach (Tile t in currentMap.Tiles)
        {
            t.cbTileChanged = null;
        }
        
        wc.DestroyMapGameObjects(currentMap);

        currentMap = null;

        // generate new map
        currentMap = GenerateMap(newLevel);

        wc.GenerateMapGameObjects(currentMap);

        // Set level number
        dungeonLevel = newLevel;

        // place player
        player.Map = currentMap;
        player.Move(currentMap.GetAppropriateStartingTile());
        player.fovHelper.FOV(player.Tile);

        // refresh map game objects
        currentMap.SetAllDungeonItemsVisibility();
        wc.ProcessEntitiesQueue();
        wc.FinalizeTurn();
    }

    Map GenerateMap(int level=1){
        Map newMap;

        // big map
        newMap = new Map(width:50, height:25, this);
        newMap.GenerateRooms(maxWidth:12, maxHeight:12, maxRooms:18, minWidth:5, minHeight:3);

        // small map
        // newMap = new Map(width:25, height:15, this);
        // newMap.GenerateRooms(maxWidth:10, maxHeight:10, maxRooms:3, minWidth:3, minHeight:3);

        return newMap;

    }
}
