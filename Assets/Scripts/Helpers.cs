using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Helpers
{
    public static class TextAssetHelper {

        public static string GetRandomLinefromTextAsset(string taName)
        {
            TextAsset ta = (TextAsset)Resources.Load("TextAssets/" + taName, typeof(TextAsset));
            return GetRandomLineFromTextAsset(ta);
        }

        static string GetRandomLineFromTextAsset(TextAsset ta) {
            if (ta == null) return "";

            string[] lines = ta.text.Split('\n');
            string line = lines[UnityEngine.Random.Range(0,lines.Length)];
            return line.Trim();
        
        }
    
    }

    public static class ItemEffects {
    
        public static void HealingPotion(Actor target, Item item, int value) {
            if (target.health.Hitpoints < target.health.MaxHitpoints) {
                target.health.Heal(UnityEngine.Random.Range(4,value));
                item.Consume();
            }
        }

        public static void Gold(Actor target, Item item, int value) {
            target.gold += value;
            item.Consume();
        }
    }

}
