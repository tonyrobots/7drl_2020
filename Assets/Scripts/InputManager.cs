using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    Game game;

    public Game Game { get => game; set => game = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (Game.Player.isAlive && Game.Player.Map.Game.gamestate == Game.GameStates.PLAYER_TURN)
    //     {
    //         // should use a separate input manager
    //         if (Input.GetKeyDown(KeyCode.UpArrow))
    //         {
    //             Game.Player.AttemptMove(0, 1);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.DownArrow))
    //         {
    //             Game.Player.AttemptMove(0, -1);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //         {
    //             Game.Player.AttemptMove(-1, 0);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.RightArrow))
    //         {
    //             Game.Player.AttemptMove(1, 0);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Space))
    //         {
    //             Game.Player.Wait();
    //         }
    //         else if (Input.GetKeyDown(KeyCode.G))
    //         {
    //             // get item
    //             Game.Player.PickUpItemAtCurrentLocation();
    //         }
    //         else if (Input.GetKeyDown(KeyCode.I))
    //         {
    //             // show inventory
    //             Debug.Log(Game.Player.ListInventoryAsString());
    //             Game.Player.Map.Game.OpenPlayerInventory();
    //         }
    //         else if (Input.GetKeyDown(KeyCode.M))
    //         // explore map mode
    //         {
    //             Game.Player.Map.Game.gamestate = Game.GameStates.EXPLORE_MAP;
    //         }
    //         else if (Input.GetKeyDown(KeyCode.KeypadMinus))
    //         {
    //             // reveal map (TODO disable later)
    //             Game.Player.Map.RevealAll();

    //         }

    //     }
    //     else if (Game.Player.isAlive == false)
    //     {
    //         // if player is dead, hitting space will restart the game completely
    //         if (Input.GetKeyDown(KeyCode.Space))
    //         {
    //             SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //         }
    //     }
    //     else if (Game.Player.Map.Game.gamestate == Game.GameStates.EXPLORE_MAP)
    //     {
    //         // stub
    //         if (Input.GetKeyDown(KeyCode.Escape))
    //         {
    //             Game.Player.Map.Game.gamestate = Game.GameStates.PLAYER_TURN;
    //         }
    //     }
    //     else if (Game.Player.Map.Game.gamestate == Game.GameStates.INVENTORY)
    //     {
    //         if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
    //         {
    //             Game.Player.Map.Game.ClosePlayerInventory();
    //         }
    //     }
    // }

    void Update() {
        switch (game.gamestate)
        {
            // Player Moves
            case Game.GameStates.PLAYER_TURN:

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Game.Player.AttemptMove(0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Game.Player.AttemptMove(0, -1);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Game.Player.AttemptMove(-1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Game.Player.AttemptMove(1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Game.Player.Wait();
                }
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    // get item
                    Game.Player.PickUpItemAtCurrentLocation();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    // open drop item menu
                    Game.OpenPlayerInventory(Game.GameStates.INVENTORY_DROP);
                }

                else if (Input.GetKeyDown(KeyCode.I))
                {
                    // show inventory
                    // Debug.Log(Game.Player.ListInventoryAsString());
                    
                    Game.OpenPlayerInventory();
                }
                else if (Input.GetKeyDown(KeyCode.M))
                // explore map mode
                {
                    Game.gamestate = Game.GameStates.EXPLORE_MAP;
                }
                else if (Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    // reveal map (TODO disable later)
                    Game.Player.Map.RevealAll();

                }
            break;

            case Game.GameStates.INVENTORY_DROP:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    // hide inventory
                    // Debug.Log(Game.Player.ListInventoryAsString());
                    Game.ClosePlayerInventory();
                }
                // if (Input.GetKeyDown(KeyCode.A))
                // {
                //     // drop item 
                //     Game.Player.DropItem(Game.Player.Inventory[0]);
                // }
                foreach (char c in Input.inputString) {
                    Debug.Log(c);
                }
            break;


            case Game.GameStates.PLAYER_DEAD:
                    // if player is dead, hitting space will restart the game completely
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            break;

            case Game.GameStates.EXPLORE_MAP:
                if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
                {
                    Game.gamestate = Game.GameStates.PLAYER_TURN;
                }
            break;

            default:
            break;
        }
    }
}
