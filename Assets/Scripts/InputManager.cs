using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class InputManager : MonoBehaviour
{

    Game game;

    public Game Game { get => game; set => game = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update() {
        switch (game.gamestate)
        {
            case Game.GameStates.WELCOME:
                if (Input.inputString == "?") {
                    Game.wc.uiManager.HideWelcomePanel();
                    Game.wc.uiManager.ShowHelpPanel();
                }

                else if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) 
                {
                    Game.wc.uiManager.HideWelcomePanel();
                }
            break;

            case Game.GameStates.HELP:
                if (Input.inputString == "?" || Input.GetKeyDown(KeyCode.Escape))
                {
                    Game.wc.uiManager.HideHelpPanel();
                }
            break;

            // Player Moves
            case Game.GameStates.PLAYER_TURN:

                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
                {
                    Game.Player.AttemptMove(0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.Keypad2))
                {
                    Game.Player.AttemptMove(0, -1);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.Keypad4))
                // else if (Input.GetAxis("Horizontal") <  -.1)
                {
                    Game.Player.AttemptMove(-1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.Keypad6))
                {
                    Game.Player.AttemptMove(1, 0);
                }

               if (Game.allowDiagonalMovement) {
                    if (Input.GetKeyDown(KeyCode.Keypad7)) {
                        Game.Player.AttemptMove(-1, 1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Keypad9))
                    {
                        Game.Player.AttemptMove(1, 1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Keypad1))
                    {
                        Game.Player.AttemptMove(-1, -1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Keypad3))
                    {
                        Game.Player.AttemptMove(1, -1);
                    }
                
                
                }

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad5))
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
                else if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.R))
                {
                    // open use item menu
                    Game.OpenPlayerInventory(Game.GameStates.INVENTORY_USE);
                }

                else if (Input.GetKeyDown(KeyCode.I))
                {
                    // show inventory
                    // Debug.Log(Game.Player.ListInventoryAsString());
                    
                    Game.OpenPlayerInventory();
                }

                else if (Input.GetKeyDown(KeyCode.Escape)) {
                    Game.ClosePlayerInventory();
                }
                else if (Input.inputString == "?")
                {
                    Game.wc.uiManager.ShowHelpPanel();
                }

#if UNITY_EDITOR
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    Game.DescendToNextLevel();
                    // Debug.Log("descending?");
                }
#endif
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
            case Game.GameStates.INVENTORY_USE:
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
                    if (c >= 'a' && c <= 'z') {
                        ActOnItem(c);
                    }
                }
            break;

            case Game.GameStates.UPGRADE_MENU:
                // if (Input.GetKeyDown(KeyCode.Escape))
                // {
                //     // hide panel. probably shouldn't allow this?
                //     // Debug.Log(Game.Player.ListInventoryAsString());
                //     Game.CloseOptionsPanel();
                // }
                foreach (char c in Input.inputString)
                {
                    if (c >= '1' && c <= '3')
                    {
                        //Game.ChooseOption(c - '1'); // returns 0 for "1", 1, for "2", and 2 for "3", thanks to some ascii math
                        Game.wc.uiManager.ChooseOption(c - '1'); // returns 0 for "1", 1, for "2", and 2 for "3", thanks to some ascii math
                    }
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


    void ActOnItem(char c) 
    {
        int itemIndex = c - 'a';
        if (itemIndex < game.Player.Inventory.Count) {
            switch (game.gamestate)
            {
                case Game.GameStates.INVENTORY_USE:
                    // use item
                    game.Player.UseItem(game.Player.Inventory[itemIndex]);
                    game.ClosePlayerInventory();
                break;

                case Game.GameStates.INVENTORY_DROP:
                    game.Player.DropItem(game.Player.Inventory[itemIndex]);
                    game.OpenPlayerInventory(Game.GameStates.INVENTORY_DROP);
                break;
                
                default:
                break;
            }
        }
    
    }
    // // attempt to Drop the item corresponding with the character input
    // void DropItem(char c) 
    // {

    
    // }

    // // attempt to use item corresponding with the char input
    // void UseItem(char c) 
    // {
    // }


}
