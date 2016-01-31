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
    [SerializeField] private Rhythm _rhythm;
    private readonly Dictionary<string, int> _variables = new Dictionary<string, int>();
    private readonly List<SpawnSettings> _objTypes = new List<SpawnSettings>();
    private readonly List<GameObject> _objectsAdded = new List<GameObject>();
    private readonly Dictionary<string, int> _countInTheWorld = new Dictionary<string, int>();

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

        _objTypes.Add(new SpawnSettings("tree", -4.0f, -2.5f));
        _objTypes.Add(new SpawnSettings("mountain", -2.5f, -2f));
        _objTypes.Add(new SpawnSettings("bird", -0.5f, 5f));
        _objTypes.Add(new SpawnSettings("cloud", -0.5f, 5f));
        _objTypes.Add(new SpawnSettings("flower", -6f, -5f));
        _objTypes.Add(new SpawnSettings("bush", -6f, -5f));
        _objTypes.Add(new SpawnSettings("grass", -6f, -5f));
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

        int curCount = 0;
        if (_countInTheWorld.TryGetValue(obj, out curCount)) {
            _countInTheWorld.Remove(obj);
        }
        ++curCount;
        _countInTheWorld.Add(obj, curCount);
        OnNewObjectAdded(obj, curCount);
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
        go.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.FloorToInt((height + coreOffset) * 10);
    }

    private void OnVariableSet(string variable, int value) {
        var target = _variableTargets.FirstOrDefault(o => o.Name == variable);
        if (target != null) {
            target.Object.SendMessage("OnVariableSet", value, SendMessageOptions.DontRequireReceiver);
        }

        if (variable == "ground" || variable == "mountain") {
            float volume = Mathf.Clamp(GetVariable("ground"), 0, 3) * 0.2f + Mathf.Clamp(GetVariable("mountain"), 0, 4) * 0.1f;
            _rhythm.FadeLayerTowards(1, Mathf.Clamp01(volume));
        }
        if (variable == "moon") {
            _rhythm.FadeLayerTowards(3, Mathf.Clamp01(value / 3.0f));
        }
        if (variable == "sun") {
            _rhythm.FadeLayerTowards(4, Mathf.Clamp01(value / 3.0f));
        }
    }

    private void OnNewObjectAdded(string objType, int curCount) {
        if (objType == "tree") {
            _rhythm.FadeLayerTowards(2, Mathf.Clamp01(curCount * 0.2f));
        }
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
        _countInTheWorld.Clear();
    }
}
