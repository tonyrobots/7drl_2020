using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity 
{
    public Tile _tile;
    public char _symbol;
    public Color _color;
    protected Map _map;

    public bool _isVisible = false;


    protected Action<Entity> cbEntityChanged;

    public void RegisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged += callback;
    }

    public void UnregisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged -= callback;
    }


}
