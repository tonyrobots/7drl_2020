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
    [SerializeField] private TextMeshProUGUI statusField = null;
    [SerializeField] private TextMeshProUGUI messagesField = null;

    // [SerializeField] private Text goldField;
    // [SerializeField] private Text mapLevelField;

    public void Start() {

    }


    public void UpdatePlayerStats(Game game)
    {

        nameField.text = game.Player.Name;

        if (game.Player.health.Hitpoints > 0) {
            healthField.text = $"Health:  {game.Player.health.Hitpoints}/{game.Player.health.MaxHitpoints}";
        } else {
            healthField.text = "Health: <#ff0000>Dead.</color>";
        }

        string statusesList = "";
        foreach (string status in game.Player.statuses) {
            statusesList += $"{status}     ";
        }
        statusField.text = statusesList;
        // goldField.text = "Gold:      " + game.Player.Gold;
        // mapLevelField.text = "Map Level: " + game.mapLevel;
    }

    public void UpdateMessageLog(Game game) {
        string messageToShow = "";
        while (game.messageLog.Count > 0)
        {   
            Message m = game.messageLog.Dequeue();
            messageToShow += $"<{m.hexColor}>{m.messageText}</color>... ";
        }


        messagesField.text = messageToShow;
    }

}
