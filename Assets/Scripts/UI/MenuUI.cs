using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {

    public GameObject TitleScreen;
    public GameObject HelpScreen;
    public GameObject GameScreen;

    public static MenuUI Instance { get; private set; }

    protected void Awake() {
        Instance = this;
    }

    protected void Start() {
        SetScreen(HelpScreen, false);
    }

    public void SetScreen(GameObject screen, bool fade = true) {
        if (fade) {
            FadeUI.Instance.CrossFade(1.0f, () => DoSetScreen(screen));
        } else {
            DoSetScreen(screen);
        }
    }

    private void DoSetScreen(GameObject screen) {
        var allScreens = new[] {
            TitleScreen,
            HelpScreen,
            GameScreen
        };

        foreach (var s in allScreens) {
            if (s != null) {
                s.SetActive(s == screen);
            }
        }
    }
}
