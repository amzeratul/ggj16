using UnityEngine;

public class CreditsUI : MonoBehaviour {
    private bool _active;

    protected void OnEnable() {
        _active = true;
    }

    public void OnOptionChosen(int option) {
        if (_active) {
            if (option == 0) {
                _active = false;
                MenuUI.Instance.SetScreen(MenuUI.Instance.TitleScreen);
            } else {
                string[] handles = new[] { "infinite_ammo", "bitmOO", "viiolaceus", "amzeratul" };
                Application.OpenURL("http://twitter.com/" + handles[option -1]);
            }
        }
    }
}
