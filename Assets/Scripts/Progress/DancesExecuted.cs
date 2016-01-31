using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DancesExecuted : MonoBehaviour {

    public static DancesExecuted Instance { get; private set; }
    private HashSet<int> _executed = new HashSet<int>();

    protected void Awake() {
        Instance = this;
        var exes = PlayerPrefs.GetString("executedDances");
        if (!string.IsNullOrEmpty(exes)) {
            foreach (int d in exes.Split(' ').Select<string, int>(int.Parse)) {
                _executed.Add(d);
            }
        }
    }

    public void ExecuteDance(DanceMove move) {
        if (!_executed.Contains(move.Id)) {
            _executed.Add(move.Id);
            Save();
        }
    }

    private void Save() {
        var strs = string.Join(" ", _executed.Select(i => i.ToString()).ToArray());
        PlayerPrefs.SetString("executedDances", strs);
        PlayerPrefs.Save();
    }

    public bool HasExecutedDance(int id) {
        return _executed.Contains(id);
    }

    public int[] GetExecutedDances() {
        return _executed.ToArray();
    }
}
