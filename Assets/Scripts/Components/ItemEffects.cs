using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemEffects
{

    public static void HealingPotion(Actor target, Item item, int value)
    {
        if (target.health.Hitpoints < target.health.MaxHitpoints)
        {
            target.Map.Game.Log($"You drink the potion.");
            target.health.Heal(UnityEngine.Random.Range(4, value));
            //item.Consume();
        } else {
            target.Map.Game.Log($"You don't need that right now.");
        }
    }

    public static void Gold(Actor target, Item item, int value)
    {
        target.gold += value;
        target.Map.Game.Log($"You pick up {value} gold.");
        //item.Consume();
    }

    public static void Bleeder(Actor target, Item item, int value) 
    {
        target.Map.Game.Log("Your weapon inflicts a nasty wound");
        target.health.InflictBleeding(value);
    }

    public static void DescendLevel(Actor target, Item item, int value) {
        target.Map.Game.Log("You descend the stairs");
        target.Map.Game.DescendToNextLevel();
    }

    public static void RevealMap(Actor target, Item item, int value) {
        target.Map.Game.Log("Your surrounding are magically revealed!");
        target.Map.RevealAll();
    }
    public static void Teleport(Actor target, Item item, int value)
    {
        target.Map.Game.Log($"{target.Name} is teleported to another part of the level.");
        target.PlaceAtTile(target.Map.GetRandomEmptyFloorTile());
    }
}
