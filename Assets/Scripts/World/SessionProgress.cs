using UnityEngine;
using System.Collections;

public class SessionProgress : MonoBehaviour {

    [SerializeField] private World _world;
    [SerializeField] private WorldCameraProgress _camera;
    [SerializeField] private Rhythm _rhythm;
    [SerializeField] private float _sessionLength;
    private float _time;
    private bool _running;

    protected void Start() {
        StartSession();
    }

    private void StartSession() {
        FadeUI.Instance.FadeIn(3);
        _world.Reset();
        _camera.Reset(_sessionLength);
        _rhythm.StartRunning();
        _time = 0;
        _running = true;
    }

    private void EndSession() {
        _rhythm.StopRunning();
        FadeUI.Instance.FadeOut(3);
    }

    protected void Update() {
        _time += Time.deltaTime;
        if (_time > _sessionLength && _running) {
            _running = false;
            EndSession();
        }
    }

}
