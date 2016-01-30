using System.IO;
using UnityEngine;
using UnityEditor;

public class UpdateDanceMoves : MonoBehaviour {

    [MenuItem("AnuKi/UpdateDanceList")]
    public static void UpdateDanceList() {
        string url = "https://docs.google.com/spreadsheets/d/195n7fw7LZBRjNGkWzVwSCJh7J369JjHrEUmEp6hmR94/pub?gid=0&single=true&output=csv";
        var request = new WWW(url);
        while (!request.isDone) {}
        File.WriteAllText(Application.dataPath + "/Data/danceMoves.csv", request.text);
        AssetDatabase.Refresh();
    }

    private static void DoUpdateDanceList() {
        throw new System.NotImplementedException();
    }
}
