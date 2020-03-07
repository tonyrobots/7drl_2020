using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Helpers
{
    public static class TextAssetHelper {

        public static string GetRandomLinefromTextAsset(string taName, bool skipFirstLine=true)
        {
            TextAsset ta = (TextAsset)Resources.Load("TextAssets/" + taName, typeof(TextAsset));
            return GetRandomLineFromTextAsset(ta, skipFirstLine);
        }

        static string GetRandomLineFromTextAsset(TextAsset ta, bool skipFirstLine=true) {
            if (ta == null) {
                Debug.LogError($"Tried to read from textasset {ta} but failed");
                return "";
            }
            string[] lines = ta.text.Split('\n');
            int startLine = skipFirstLine?  1 : 0;
            string line = lines[UnityEngine.Random.Range(startLine,lines.Length)];
            return line.Trim();
        
        }
    
    }

    public static class Extensions {
        public static void Shuffle<T>(this IList<T> list)
        {
            for (var i = list.Count; i > 0; i--)
                list.Swap(0, UnityEngine.Random.Range(0, i));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

    }


}
