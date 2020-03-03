using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game
{
    int _turnCount = 0;

    public enum GameStates
    {
        PLAYER_TURN,
        ENEMY_TURN,
        PLAYER_DEAD
    }

    public GameStates gamestate = GameStates.PLAYER_TURN;
    
    public Map _currentMap;
    private Player player;

    public Map CurrentMap { get => _currentMap; set => _currentMap = value; }
    public int TurnCount { get => _turnCount; set => _turnCount = value; }
    public Player Player { get => player; set => player = value; }

    public Queue<Message> messageLog;

    public Game() {
        // messageLog = new MessageLog(this);
        messageLog = new Queue<Message>();
    }

    public void Log(string simpleMessage) {
        // probably a temporary function until we implement UI and stuff
        // just want to have a single place to handle instead of calling debug.log everywhere
        Debug.Log(simpleMessage);
        Message m = new Message(simpleMessage);
        messageLog.Enqueue(m);
    }
}
