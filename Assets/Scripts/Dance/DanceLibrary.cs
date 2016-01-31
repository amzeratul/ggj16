using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class DanceLibrary : MonoBehaviour {
    [HideInInspector] public DanceMove[] Moves;
    [SerializeField] public TextAsset _data;

    private enum CSVColumns {
        Id,
        EffectDescription,
        Effect,
        Require,
        P1Sequence,
        P2Sequence
    }

    protected void Awake() {
        Moves = new DanceMove[0];
        var lines = _data.text.Split('\n');
        Moves = lines.Skip(1).Select<string, DanceMove>(ParseLine).ToArray();
    }

    private DanceMove ParseLine(string line) {
        // TODO: will fail if any entry has a comma in it
        var columns = line.Split(',');
        var p0 = ParseStepList(columns[(int) CSVColumns.P1Sequence]);
        var p1 = ParseStepList(columns[(int) CSVColumns.P2Sequence]);
        int n = p0.Length;
        var steps = new DanceStepPair[n];
        for (int i = 0; i < n; i++) {
            steps[i] = new DanceStepPair {
                p0 = ParseStep(p0[i], false),
                p1 = ParseStep(p1[i], true)
            };
        }
        return new DanceMove {
            Id = int.Parse(columns[(int) CSVColumns.Id]),
            Description = columns[(int) CSVColumns.EffectDescription],
            Steps = steps,
            Effect = ParseEffect(columns[(int) CSVColumns.Effect]),
            IsAvailable = ParseRequirement(columns[(int) CSVColumns.Require])
        };
    }

    private DanceMove.EffectType ParseEffect(string val) {
        if (string.IsNullOrEmpty(val)) {
            return world => { };
        }

        var words = val.Split(' ');

        if (words[0] == "inc") {
            string var = words[1];
            return world => world.IncrementVariable(var);
        }

        if (words[0] == "add") {
            string obj = words[1];
            string[] vars = words[2].Split('-');
            int variationMin = int.Parse(vars[0]);
            int variationMax = vars.Length > 1 ? int.Parse(vars[1]) : variationMin;
            return world => world.AddObject(obj, UnityEngine.Random.Range(variationMin, variationMax));
        }

        throw new Exception("Unknown effect: " + val);
    }

    private DanceMove.IsAvailableType ParseRequirement(string val) {
        if (string.IsNullOrEmpty(val)) {
            return (world) => true;
        }
        var words = val.Split(' ');
        string variable = words[0];
        int value = int.Parse(words[1]);
        return (world) => world.GetVariable(variable) >= value;
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
        case 'l':
            return DanceStep.TandemLeft;
        case 'r':
            return DanceStep.TandemRight;
        }
        throw new Exception("Failed to parse move: \"" + s + "\"");
    }
}
