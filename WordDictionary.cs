using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Loads a wordlist from Resources/wordlist.txt if present, else uses a tiny fallback.
public static class WordDictionary
{
    private static HashSet<string> words;
    private static bool loaded = false;

    public static void EnsureLoaded()
    {
        if (loaded) return;
        words = new HashSet<string>();

        // Try resources
        TextAsset asset = Resources.Load<TextAsset>("wordlist");
        if (asset != null)
        {
            using (StringReader sr = new StringReader(asset.text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var w = line.Trim().ToUpperInvariant();
                    if (w.Length > 0) words.Add(w);
                }
            }
            Debug.Log($"Loaded wordlist from Resources with {words.Count} words.");
        }
        else
        {
            // tiny fallback set
            string[] fallback = new[] {"CAT","DOG","CATS","DOGS","RUN","RUNS","PLAY","PLAYER","APPLE","ORANGE","GAME","WORD","WORDS","TEST","TESTS","EAT","EATS","ATE"};
            foreach (var s in fallback) words.Add(s);
            Debug.LogWarning("No Resources/wordlist.txt found — using small fallback dictionary. Add a full wordlist at Assets/Resources/wordlist.txt for proper validation.");
        }
        loaded = true;
    }

    public static bool IsValid(string word)
    {
        EnsureLoaded();
        if (string.IsNullOrWhiteSpace(word)) return false;
        return words.Contains(word.ToUpperInvariant());
    }
}
