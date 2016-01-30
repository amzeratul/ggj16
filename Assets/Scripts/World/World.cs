using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Random = UnityEngine.Random;

public class World : MonoBehaviour {

    [Serializable]
    public class VariableTarget {
        public string Name;
        public GameObject Object;
    }

    [Serializable]
    public class SpawnSettings {
        public string Name;
        public float MinHeight;
        public float MaxHeight;
        public float MaxDistance;
        public bool AlignWithCore;

        public SpawnSettings(string name, float minHeight, float maxHeight, float maxDistance = 12, bool align = true) {
            Name = name;
            MinHeight = minHeight;
            MaxHeight = maxHeight;
            MaxDistance = maxDistance;
            AlignWithCore = align;
        }
    }

    [SerializeField] private VariableTarget[] _variableTargets;
    private readonly Dictionary<string, int> _variables = new Dictionary<string, int>();
    private readonly List<SpawnSettings> _objTypes = new List<SpawnSettings>();
    private readonly List<GameObject> _objectsAdded = new List<GameObject>();

    protected void Awake() {
        // Heights:
        // Centre of world: -40
        // Top of screen: 8.5
        // Bottom of screen: -6
        // Midground: -4
        // Horizon: -2.5
        // Ocean horizon: -1.2

        // Screen width:
        // Initial: 6.5
        // Final: 12

        _objTypes.Add(new SpawnSettings("tree", -4.5f, -3f));
        _objTypes.Add(new SpawnSettings("mountain", -2.5f, -2f));
        _objTypes.Add(new SpawnSettings("bird", -0.5f, 5f));
        _objTypes.Add(new SpawnSettings("cloud", -0.5f, 5f));
        _objTypes.Add(new SpawnSettings("flower", -5.5f, -4.5f));
        _objTypes.Add(new SpawnSettings("bush", -5.5f, -4.5f));
        _objTypes.Add(new SpawnSettings("totem", -4.5f, -3f));
        _objTypes.Add(new SpawnSettings("floatingTotem", 3, 5));
        _objTypes.Add(new SpawnSettings("fireflies", -5.5f, -1f));
        _objTypes.Add(new SpawnSettings("cow", -4.5f, -3f));
        _objTypes.Add(new SpawnSettings("dog", -4.5f, -3f));
    }

    public void Start() {
    }

    protected void Update() {
        //if (Input.GetKeyDown(KeyCode.Space)) AddObject("cloud", 0);
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
            go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
            _objectsAdded.Add(go);
        } else {
            Debug.LogWarning("Object not found: " + obj + "_" + variation);
        }
    }

    private void PositionObject(GameObject go, string objName) {
        var par = _objTypes.First(o => o.Name == objName);
        float coreOffset = -40;
        float height = UnityEngine.Random.Range(par.MinHeight, par.MaxHeight) - coreOffset;
        float x = UnityEngine.Random.Range(-par.MaxDistance, par.MaxDistance);
        // height² = x² + y²
        // y = sqrt(height² - x²)
        float y = Mathf.Sqrt(height * height - x * x);
        go.transform.position = new Vector3(x, y + coreOffset, 0);
        if (par.AlignWithCore) {
            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(x, y, 0).normalized);
        }
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
