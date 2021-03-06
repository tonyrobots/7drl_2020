﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MonsterController : MonoBehaviour
{
    Monster monster_data;

    public Monster Monster_data { get => monster_data; set => monster_data = value; }

    // Start is called before the first frame update
    void Start()
    {
        Monster_data.RegisterEntityChangedCallback((entity) => { Refresh(entity, gameObject); });
        Refresh(monster_data, gameObject);
        gameObject.AddComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh(Entity entity, GameObject go)
    {
        gameObject.name = monster_data.Name;

        if (entity.IsVisible)
        {
            transform.position = new Vector3(entity.Tile.X, entity.Tile.Y, 0);
            transform.localScale = new Vector3(1, 1, 1);
            TextMeshPro tmp = GetComponent<TextMeshPro>();
            tmp.text = entity.Symbol.ToString();
            // tmp.fontSize = 9;
            tmp.color = entity.Color;
            tmp.alignment = TextAlignmentOptions.Center;
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
        }

    }

    void OnMouseEnter()
    {
        if (monster_data.isAlive) {
            Debug.Log($"{monster_data.Name} has {monster_data.health.Hitpoints}");
            monster_data.Map.Game.wc.uiManager.ShowMonsterInfo(monster_data); // this is a long path!
        }
    }

    void OnMouseExit()
    {
        Debug.Log($"mouse exit from {monster_data.Name} ");
        monster_data.Map.Game.wc.uiManager.HideInfoPanel();

    }

    void OnDestroy()
    {
        monster_data.UnregisterEntityChangedCallback((entity) => { Refresh(entity, gameObject); });
    }


}
