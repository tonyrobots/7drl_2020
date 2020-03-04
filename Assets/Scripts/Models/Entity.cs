using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity 
{
    private Tile tile;
    private string symbol;
    private Color color;
    private Map map;
    private string name;

    bool isVisible = false;
    public bool isPassable = false;

    protected Action<Entity> cbEntityChanged;

    public Tile Tile { get => tile; 
        set 
        {
            // exit previous tile if any
            if (tile != null) {
                tile.Exit(this);
            }

            tile = value;
            // enter new tile
            tile.Enter(this);
        }
    }
    public string Symbol { get => symbol; set => symbol = value; }
    public Color Color { get => color; set => color = value; }
    public Map Map { get => map; set => map = value; }
    public string Name { get => name; set => name = value; }
    public bool IsVisible { get => isVisible; set { 
        isVisible = value;
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        
        }
    }

    public void RegisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged += callback;
    }

    public void UnregisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged -= callback;
    }

    public virtual void DoTurn() {
    }



}
