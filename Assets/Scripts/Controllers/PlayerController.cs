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
        if (player_data.isAlive) {
        // should use a separate input manager
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                player_data.AttemptMove(0,1);
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                player_data.AttemptMove(0, -1);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                player_data.AttemptMove(-1, 0);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                player_data.AttemptMove(1, 0);
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                player_data.Wait();
            }
        } else {
            // if player is dead, hitting space will restart the game completely
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void Refresh(Entity entity, GameObject go) {
        transform.position = new Vector3(entity.Tile.X, entity.Tile.Y, 0);
        TextMeshPro tmp = GetComponent<TextMeshPro>();
        tmp.text = entity.Symbol.ToString();
        tmp.color = entity.Color;
    }
}
