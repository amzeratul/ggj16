using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class DanceLibrary : MonoBehaviour {
    public DanceMove[] Moves;
    [SerializeField] public TextAsset _data;

    protected void Awake() {
        Moves = new DanceMove[0];
        var lines = _data.text.Split('\n');
        Moves = lines.Skip(1).Select<string, DanceMove>(ParseLine).ToArray();
    }

    private DanceMove ParseLine(string line) {
        // TODO: will fail if any entry has a comma in it
        var columns = line.Split(',');
        var p0 = ParseStepList(columns[3]);
        var p1 = ParseStepList(columns[4]);
        int n = p0.Length;
        var steps = new DanceStepPair[n];
        for (int i = 0; i < n; i++) {
            steps[i] = new DanceStepPair {
                p0 = ParseStep(p0[i], false),
                p1 = ParseStep(p1[i], true)
            };
        }
        return new DanceMove {
            Description = columns[1],
            Steps = steps
        };
    }

    private static string[] ParseStepList(string val) {
        var raw = val.Trim().ToLower();
        if (string.IsNullOrEmpty(raw)) {
            return new string[0];
        }
        return raw.Split(' ');
    }

    private DanceStep ParseStep(string s, bool flip) {
        var c = s[0];
        switch (c) {
        case '<':
            return flip ? DanceStep.Close : DanceStep.Away;
        case '>':
            return flip ? DanceStep.Away : DanceStep.Close;
        case '^':
            return DanceStep.Up;
        case 'v':
            return DanceStep.Down;
        case 'x':
            return DanceStep.Swap;
        case '.':
            return DanceStep.Idle;
        }
        throw new Exception("Failed to parse move: \"" + s + "\"");
    }
}
