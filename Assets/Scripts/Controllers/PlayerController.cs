using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Player player_data;

    public Player Player_data { get => player_data; set => player_data = value; }

    // Start is called before the first frame update
    void Start()
    {
        // register to call Refresh() on player change events
        player_data.RegisterEntityChangedCallback((entity) => { Refresh(entity, gameObject); });

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sortingOrder=3;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Refresh(Entity entity, GameObject go) {
        transform.position = new Vector3(entity.Tile.X, entity.Tile.Y, 0);
        TextMeshPro tmp = GetComponent<TextMeshPro>();
        tmp.text = entity.Symbol.ToString();
        tmp.color = entity.Color;
    }
}
