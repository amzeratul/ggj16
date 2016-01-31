using UnityEngine;
using System.Collections;

public class SessionProgress : MonoBehaviour {

    [SerializeField] private World _world;
    [SerializeField] private WorldCameraProgress _camera;
    [SerializeField] private Rhythm _rhythm;
    [SerializeField] private DanceExecuter _dance;
    [SerializeField] private float _sessionLength;
    [SerializeField] private InGameUI _gameUi;
    [SerializeField] private TwitterUI _twitterUi;
    [SerializeField] public Texture2D _screenshot;

    private float _time;
    private bool _running;
    private bool _waitingRestart;
    private bool _waitingScreenshot;
    private bool _hasFocus;
    private bool _canTweet;

    public void StartSession() {
        _world.Reset();
        _camera.Reset(_sessionLength);
        _rhythm.StartRunning();
        _dance.Reset();
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
        
        yield return new WaitForEndOfFrame();
        TakeScreenshot();

        _hasFocus = true;
        _waitingRestart = true;
        _gameUi.ShowDanceMove(null);
        _gameUi.CanRestart = true;
    }

    private void TakeScreenshot() {
        int w = Screen.width;
        int h = Screen.height;
        Texture2D tex = new Texture2D(w, h);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        int maxW = 1280;
        if (w > maxW) {
            TextureScale.Bilinear(tex, maxW, h * maxW / w);
        }
        _screenshot = tex;
        _canTweet = true;
        _gameUi.CanTweet = true;
    }

    protected void Update() {
        _time += Time.deltaTime;
        if (_time > _sessionLength && _running) {
            _running = false;
            EndSession();
        }

        if (_waitingRestart) {
            if (Input.GetButtonDown("Tweet") && _canTweet) {
                _hasFocus = false;
                _gameUi.CanRestart = false;
                _gameUi.CanTweet = false;
                _twitterUi.SendScreenshot(_screenshot, success => {
                    _canTweet = !success;
                    _gameUi.CanTweet = _canTweet;
                    _gameUi.CanRestart = true;
                    _hasFocus = true;
                });
            }
            if (Input.GetButtonDown("Submit") && _hasFocus) {
                _waitingRestart = false;
                _gameUi.CanRestart = false;
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
