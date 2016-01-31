using UnityEngine;
using System.Collections;

public class UpgradeableWorldBit : MonoBehaviour {
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private bool _incremental;

    private int _prevLevel = -1;

    public void Start() {
        Reset();
    }

    private void Reset() {
        foreach (var o in _objects) {
            if (o != null) {
                o.SetActive(false);
            }
        }
        if (_objects[0] != null) {
            _objects[0].SetActive(true);
        }
    }

    public void OnVariableSet(int level) {
        if (level == 0) {
            Reset();
            return;
        }

        int actualLevel = Mathf.Clamp(level, 0, _objects.Length - 1);
        if (actualLevel != _prevLevel) {
            if (_prevLevel >= 0 && _objects[_prevLevel] != null && !_incremental) {
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
