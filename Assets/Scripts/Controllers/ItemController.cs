using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ItemController : MonoBehaviour
{
    Item item_data;

    public Item Item_data { get => item_data; set => item_data = value; }


    // Start is called before the first frame update
    void Start()
    {
        Item_data.RegisterEntityChangedCallback((entity) => { Refresh(entity, gameObject); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh(Entity entity, GameObject go)
    {
        if (entity.isVisible)
        {
            transform.position = new Vector3(entity.Tile.X, entity.Tile.Y, 0);
            transform.localScale = new Vector3(1, 1, 1);
            TextMeshPro tmp = GetComponent<TextMeshPro>();
            tmp.text = entity.Symbol.ToString();
            tmp.color = entity.Color;
            tmp.alignment = TextAlignmentOptions.Center;        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
        }

    }


}
