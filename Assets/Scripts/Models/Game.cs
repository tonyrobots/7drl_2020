using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game
{
    int _turnCount = 0;

    public enum GameStates
    {
        PLAYER_TURN,
        ENEMY_TURN
    }

    public GameStates _gamestate = GameStates.PLAYER_TURN;
    
    public Map _currentMap;
    public Player _player;

    public Map CurrentMap { get => _currentMap; set => _currentMap = value; }
    public int TurnCount { get => _turnCount; set => _turnCount = value; }

    public void Log(string message) {
        // probably a temporary function until we implement UI and stuff
        // just want to have a single place to handle instead of calling debug.log everywhere
        Debug.Log(message);
    }
}
