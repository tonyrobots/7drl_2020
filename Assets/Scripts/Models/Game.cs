using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game
{
    int turnCount = 0;
    int dungeonLevel = 1;

    public enum GameStates
    {
        PLAYER_TURN,
        ENEMY_TURN,
        PLAYER_DEAD
    }

    public GameStates gamestate = GameStates.PLAYER_TURN;
    
    public Map currentMap;
    private Player player;
    private int level=1;

    public Map CurrentMap { get => currentMap; set => currentMap = value; }
    public int TurnCount { get => turnCount; set => turnCount = value; }
    public Player Player { get => player; set => player = value; }
    public int Level { get => level; set => level = value; }
    public int DungeonLevel { get => dungeonLevel; set => dungeonLevel = value; }

    public Queue<Message> messageLog;
    public Queue<Entity> entitiesToRender;

    public Game() {
        // messageLog = new MessageLog(this);
        messageLog = new Queue<Message>();
        entitiesToRender = new Queue<Entity>();
    }

    public void Log(string simpleMessage) {
        //Debug.Log(simpleMessage);
        Message m = new Message(simpleMessage);
        messageLog.Enqueue(m);
    }
}
