using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class World : MonoBehaviour {

    [Serializable]
    public class VariableTarget {
        public string Name;
        public GameObject Object;
    }

    [SerializeField] private VariableTarget[] _variableTargets;
    private readonly Dictionary<string, int> _variables = new Dictionary<string, int>();
    private readonly Dictionary<string, string> _objTypes = new Dictionary<string, string>();
    private readonly List<GameObject> _objectsAdded = new List<GameObject>();

    protected void Awake() {
        _objTypes.Add("tree", "");
        _objTypes.Add("mountain", "");
        _objTypes.Add("bird", "");
        _objTypes.Add("flower", "");
        _objTypes.Add("bush", "");
        _objTypes.Add("totem", "");
        _objTypes.Add("floatingTotem", "");
        _objTypes.Add("fireflies", "");
        _objTypes.Add("cow", "");
        _objTypes.Add("dog", "");
    }

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
        var template = Resources.Load<GameObject>("World/" + obj + "_" + variation);
        if (template) {
            var go = (GameObject) Instantiate(template, Vector3.zero, Quaternion.identity);
            PositionObject(go, obj);
            _objectsAdded.Add(go);
        } else {
            Debug.LogWarning("Object not found: " + obj + "_" + variation);
        }
    }

    private void PositionObject(GameObject go, string objName) {
        // TODO
    }

    private void OnVariableSet(string variable, int value) {
        _variableTargets.First(o => o.Name == variable).Object.SendMessage("OnVariableSet", value, SendMessageOptions.DontRequireReceiver);
    }

    public void Fumble() {
        // TODO: do we care?
    }

    public void Reset() {
        foreach (var go in _objectsAdded) {
            DestroyObject(go);
        }
        _objectsAdded.Clear();
        foreach (var v in _variables) {
            OnVariableSet(v.Key, 0);
        }
        _variables.Clear();
    }
}
