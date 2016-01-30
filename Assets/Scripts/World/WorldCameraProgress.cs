using UnityEngine;
using System.Collections;

public class WorldCameraProgress : MonoBehaviour {
    private float _time;
    private float _length = 1;
    private WorldCamera _cam;

    protected void Awake() {
        _cam = GetComponent<WorldCamera>();
    }

    protected void Update () {
	    _time += Time.deltaTime;
        float t = Mathf.Clamp01(_time / _length);
        _cam.Progress = t;
    }

    public void Reset(float sessionLength) {
        _length = sessionLength;
    }
}
