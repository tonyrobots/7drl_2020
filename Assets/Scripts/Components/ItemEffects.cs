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
            item.Consume();
        } else {
            target.Map.Game.Log($"You don't need that right now.");
        }
    }

    public static void Gold(Actor target, Item item, int value)
    {
        target.gold += value;
        target.Map.Game.Log($"You pick up {value} gold.");
        item.Consume();
    }
}
