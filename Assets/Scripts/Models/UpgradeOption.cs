using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Helpers;

public class UpgradeOption
{
    string name;
    string description;
    // public delegate void OptionEffect(Player player, int value);
    public delegate void OptionEffect();

    OptionEffect myOptionEffect;



    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public OptionEffect MyOptionEffect { get => myOptionEffect; set => myOptionEffect = value; }

    public UpgradeOption(string n, string d, OptionEffect action) {
        Name = n;
        Description = d;
        MyOptionEffect = action;
    }

    public static List<UpgradeOption> GenerateUpgradeOptions(Player player, int level=1)  // this is UGLY! refactor!
    {
        int value = UnityEngine.Random.Range(4, 11);

        List<UpgradeOption> upgradesMasterList = new List<UpgradeOption>();

        upgradesMasterList.Add(new UpgradeOption($"Strength +{value}", $"You add {value} strength, which will help you do more damage.", () => { UpgradeOption.increaseStrength(player, value); }));
        upgradesMasterList.Add(new UpgradeOption($"Max Health +{2 * value}", $"You get {2*value} more health, so you are strong like ox.", () => { UpgradeOption.increaseMaxHP(player, value * 2); }));
        upgradesMasterList.Add(new UpgradeOption($"Agility +{value}", $"You gain {value} agility, which will help you critically hit and evade attacks.", () => { UpgradeOption.increaseAgility(player, value); }));
        upgradesMasterList.Add(new UpgradeOption($"Natural Armor +1", $"Your skin thickens to provide extra armor, reducing incoming damage.", () => { UpgradeOption.addArmor(player, 1); }));
        upgradesMasterList.Add(new UpgradeOption($"{value*100} Gold", $"It's useless, but it's shiny!", () => { UpgradeOption.addGold(player, value*100); }));
        upgradesMasterList.Add(new UpgradeOption($"Quick Learner", $"You're a clever one! Gain Experience Points {value}% faster.", () => { UpgradeOption.increaseXPmodifier(player, value); }));

        upgradesMasterList.Shuffle();

        List<UpgradeOption> upgradeOptions = new List<UpgradeOption>();
        for (int i = 0; i < 3; i++)
        {
            upgradeOptions.Add(upgradesMasterList[i]);
        }
        return upgradeOptions; 
    }

    public static void increaseStrength(Player player, int n){
        player.strength += n;
    }

    public static void increaseAgility(Player player, int n)
    {
        player.agility += n;
    }
    public static void increaseMaxHP(Player player, int n)
    {
        player.health.MaxHitpoints += n;
        player.health.Hitpoints += n;
    }
    public static void addArmor(Player player, int n)
    {
        player.armor += n;
    }

    public static void addGold(Player player, int n)
    {
        player.gold += n;
    }

    public static void increaseXPmodifier(Player player, int n)
    {
        player.experienceModifier += (float)(n/100f);
        Debug.Log("XP modifier is now " + player.experienceModifier);
    }


}
