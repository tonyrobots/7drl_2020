using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameField = null;
    [SerializeField] private TextMeshProUGUI healthField = null;
    [SerializeField] private TextMeshProUGUI messagesField = null;

    // [SerializeField] private Text goldField;
    // [SerializeField] private Text mapLevelField;


    public void UpdatePlayerStats(Game game)
    {

        nameField.text = game.Player.Name;
        healthField.text = $"Health:  {game.Player.health.Hitpoints}/{game.Player.health.MaxHitpoints}";
        // goldField.text = "Gold:      " + game.Player.Gold;
        // mapLevelField.text = "Map Level: " + game.mapLevel;
    }

    public void UpdateMessageLog(Game game) {
        string messageToShow = "";
        while (game.messageLog.Count > 0)
        {
            messageToShow += game.messageLog.Dequeue().messageText + " ... ";
        }


        messagesField.text = messageToShow;
    }

}
