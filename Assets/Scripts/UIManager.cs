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
    [SerializeField] private TextMeshProUGUI DLevelField = null;
    [SerializeField] private TextMeshProUGUI turnsField = null;
    [SerializeField] private TextMeshProUGUI goldField = null;

    [SerializeField] private TextMeshProUGUI strField = null;
    [SerializeField] private TextMeshProUGUI agiField = null;
    [SerializeField] private TextMeshProUGUI armorField = null;
    [SerializeField] private TextMeshProUGUI dodgeField = null;
    [SerializeField] private TextMeshProUGUI blockField = null;

    [SerializeField] private TextMeshProUGUI weaponField = null;


    [SerializeField] private TextMeshProUGUI messagesField = null;

    // [SerializeField] private Text goldField;
    // [SerializeField] private Text mapLevelField;

    public void Start() {

    }


    public void UpdatePlayerStats(Game game)
    {

        nameField.text = game.Player.Name;

        if (game.Player.health.Hitpoints > 0) {
            healthField.text = $"{game.Player.health.Hitpoints}/{game.Player.health.MaxHitpoints}";
        } else {
            healthField.text = "<#ff0000>Dead.</color>";
        }

        string statusesList = "";
        foreach (string status in game.Player.statuses) {
            statusesList += $"{status}     ";
        }
        statusField.text = statusesList;
        goldField.text = $"{game.Player.gold}";
        DLevelField.text = $"Dungeon Level: {game.DungeonLevel}";
        turnsField.text = $"Turns: {game.TurnCount}";
        strField.text = $"{game.Player.strength}";
        agiField.text = $"{game.Player.agility}";
        armorField.text = $"{game.Player.armor}";
        dodgeField.text = $"{game.Player.EvadeChance()*100}%"; 
        
        if (game.Player.myWeapon != null) weaponField.text = $"{game.Player.myWeapon.Name} ({game.Player.myWeapon.DamageDice})";



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
