using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Player player_data;

    public Player Player_data { get => player_data; set => player_data = value; }

    // Start is called before the first frame update
    void Start()
    {
        // register to call Refresh() on player change events
        player_data.RegisterEntityChangedCallback((entity) => { Refresh(entity, gameObject); });

    }

    // Update is called once per frame
    void Update()
    {        
        // should use a separate input manager
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                player_data.Move(0,1);
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                player_data.Move(0, -1);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                player_data.Move(-1, 0);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                player_data.Move(1, 0);
            }
    }

    public void Refresh(Entity entity, GameObject go) {
        transform.position = new Vector3(entity._tile.X, entity._tile.Y, 0); 
    }
}
