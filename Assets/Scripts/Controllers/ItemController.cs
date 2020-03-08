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
        Refresh(item_data, gameObject);
        if (item_data.GetType() == typeof(Weapon)){
            // add collider for mouseover info
            gameObject.AddComponent<BoxCollider2D>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh(Entity entity, GameObject go)
    {
        gameObject.name = item_data.Name;
        if (entity.IsVisible)
        {
            transform.position = new Vector3(entity.Tile.X, entity.Tile.Y, 0);
            transform.localScale = new Vector3(1, 1, 1);
            TextMeshPro tmp = GetComponent<TextMeshPro>();
            tmp.text = entity.Symbol.ToString();
            tmp.color = entity.Color;
            tmp.alignment = TextAlignmentOptions.Center;
            if (!entity.Tile.IsVisible) {
                tmp.color = new Color(.2f, .2f, .2f); // items that are "remembered" but not currently seen are grey
            }       
        } else {
            transform.localScale = new Vector3(0, 0, 0);
        }

    }

    void OnMouseEnter()
    {
        item_data.Map.Game.wc.uiManager.ShowWeaponInfo(item_data as Weapon); // this is a long path!
    }

    void OnMouseExit()
    {
        item_data.Map.Game.wc.uiManager.HideInfoPanel();

    }
    void OnDestroy() {
        item_data.UnregisterEntityChangedCallback((entity) => { Refresh(entity, gameObject); });
    }


}
