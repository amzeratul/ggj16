using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    private readonly Dictionary<string, int> _variables = new Dictionary<string, int>();

    public void IncrementVariable(string variable) {
        int value;
        if (_variables.TryGetValue(variable, out value)) {
            _variables.Remove(variable);
            value++;
        } else {
            value = 1;
        }
        _variables.Add(variable, value);
        OnVariableSet(variable, value);
    }

    public int GetVariable(string variable) {
        int value;
        if (_variables.TryGetValue(variable, out value)) {
            return value;
        } else {
            return 0;
        }
    }
    
    public void AddObject(string obj, int variation) {
        // TODO
    }

    private void OnVariableSet(string variable, int value) {
        // TODO
    }

    public void Fumble() {
        // TODO?
    }
}
