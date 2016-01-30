using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour {

    private float _curFade = 1;
    private float _fadeTarget;
    private float _fadeSpeed;
    private Image _img;

    protected void Awake() {
        Instance = this;
        _img = GetComponent<Image>();
    }

    public void FadeIn(float time) {
        DoFade(0, time);
    }

    public void FadeOut(float time) {
        DoFade(1, time);
    }

    private void DoFade(float target, float time) {
        _fadeTarget = target;
        _fadeSpeed = 1.0f / Mathf.Max(0.01f, time);
    }

    protected void Update() {
        _curFade = Mathf.MoveTowards(_curFade, _fadeTarget, _fadeSpeed * Time.deltaTime);
        _img.color = new Color(0, 0, 0, _curFade);
        _img.enabled = _curFade > 0.0001f;
    }

    public static FadeUI Instance { get; private set; }
}
