using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity 
{
    private Tile tile;
    private char symbol;
    private Color color;
    private Map map;
    private string name;

    public bool isVisible = false;
    public bool isPassable = false;

    protected Action<Entity> cbEntityChanged;

    public Tile Tile { get => tile; set => tile = value; }
    public char Symbol { get => symbol; set => symbol = value; }
    public Color Color { get => color; set => color = value; }
    public Map Map { get => map; set => map = value; }
    public string Name { get => name; set => name = value; }



    public void RegisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged += callback;
    }

    public void UnregisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged -= callback;
    }


}
