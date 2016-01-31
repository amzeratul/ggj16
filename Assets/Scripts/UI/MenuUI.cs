using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {

    public GameObject TitleScreen;
    public GameObject HelpScreen;
    public GameObject CreditsScreen;
    public GameObject GameScreen;
    public Sprite[] DanceIcons;

    public static MenuUI Instance { get; private set; }

    protected void Awake() {
        Instance = this;
    }

    protected void Start() {
        SetScreen(TitleScreen, false);
        FadeUI.Instance.FadeIn(0.5f);
    }

    public void SetScreen(GameObject screen, bool fade = true) {
        if (fade) {
            FadeUI.Instance.CrossFade(2.0f, () => DoSetScreen(screen));
        } else {
            DoSetScreen(screen);
        }
    }

    private void DoSetScreen(GameObject screen) {
        var allScreens = new[] {
            TitleScreen,
            CreditsScreen,
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
