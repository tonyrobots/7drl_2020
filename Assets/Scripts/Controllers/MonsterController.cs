using System.Collections;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh(Entity entity, GameObject go)
    {
        if (entity._isVisible) {
            transform.position = new Vector3(entity._tile.X, entity._tile.Y, 0);
            transform.localScale = new Vector3(1, 1, 1);

        } else {
            transform.localScale = new Vector3(0,0,0);
        }

    }


}
