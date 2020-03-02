using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game
{
    int _turnCount = 0;

    public enum GameState
    {
        PLAYER_TURN,
        ENEMY_TURN
    }

    public GameState _gamestate = GameState.PLAYER_TURN;
    
    Map _currentMap;

    public Map CurrentMap { get => _currentMap; set => _currentMap = value; }
    public int TurnCount { get => _turnCount; set => _turnCount = value; }
}
