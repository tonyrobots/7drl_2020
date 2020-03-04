using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Helpers
{
    public static class TextAssetHelper {

        public static string GetRandomLinefromTextAsset(string taName, bool skipFirstLine=false)
        {
            TextAsset ta = (TextAsset)Resources.Load("TextAssets/" + taName, typeof(TextAsset));
            return GetRandomLineFromTextAsset(ta, skipFirstLine);
        }

        static string GetRandomLineFromTextAsset(TextAsset ta, bool skipFirstLine=false) {
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


}
