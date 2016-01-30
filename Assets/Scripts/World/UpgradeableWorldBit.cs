using UnityEngine;
using System.Collections;

public class UpgradeableWorldBit : MonoBehaviour {
    [SerializeField] private GameObject[] _objects;
    private int _prevLevel = -1;

    public void Start() {
        foreach (var o in _objects) {
            if (o != null) {
                o.SetActive(false);
            }
        }
    }

    public void OnVariableSet(int level) {
        int actualLevel = Mathf.Clamp(level, 0, _objects.Length - 1);
        if (actualLevel != _prevLevel) {
            if (_prevLevel >= 0 && _objects[_prevLevel] != null) {
                SetActive(_objects[_prevLevel], false);
            }
            if (_objects[actualLevel] != null) {
                SetActive(_objects[actualLevel], true);
            }
            _prevLevel = actualLevel;
        }
    }

    private void SetActive(GameObject o, bool b) {
        o.SetActive(b);
    }
}
