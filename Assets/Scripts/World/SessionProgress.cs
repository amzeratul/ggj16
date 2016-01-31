using UnityEngine;
using System.Collections;

public class SessionProgress : MonoBehaviour {

    [SerializeField] private World _world;
    [SerializeField] private WorldCameraProgress _camera;
    [SerializeField] private Rhythm _rhythm;
    [SerializeField] private float _sessionLength;
    [SerializeField] private InGameUI _gameUi;
    public Texture2D Screenshot;
    private float _time;
    private bool _running;
    private bool _waitingRestart;
    private bool _waitingScreenshot;

    public void StartSession() {
        _world.Reset();
        _camera.Reset(_sessionLength);
        _rhythm.StartRunning();
        _time = 0;
        _running = true;
        _gameUi.Reset();
    }

    private void EndSession() {
        StartCoroutine(DoEndSession());
    }

    public void Restart() {
        StartCoroutine(DoRestart());
    }

    private IEnumerator DoRestart() {
        FadeUI.Instance.FadeOut(3);
        yield return new WaitForSeconds(3.5f);
        StartSession();
        FadeUI.Instance.FadeIn(2);
    }

    private IEnumerator DoEndSession() {
        _rhythm.StopRunning();
        _gameUi.EndOfMatch();
        yield return null;
        _waitingScreenshot = true;
        while (_waitingScreenshot) {
            yield return null;
        }
        _waitingRestart = true;
        _gameUi.ShowDanceMove(null);
        _gameUi.WaitingRestart();
    }

    private void TakeScreenshot() {
        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();
        Screenshot = tex;
    }

    protected void Update() {
        _time += Time.deltaTime;
        if (_time > _sessionLength && _running) {
            _running = false;
            EndSession();
        }

        if (_waitingRestart) {
            if (Input.GetButtonDown("Submit")) {
                _waitingRestart = false;
                Restart();
            }
        }
    }

    protected void LateUpdate() {
        if (_waitingScreenshot) {
            TakeScreenshot();
            _waitingScreenshot = false;
        }
    }

}
