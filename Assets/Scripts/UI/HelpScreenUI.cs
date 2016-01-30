using UnityEngine;
using System.Collections;

public class HelpScreenUI : MonoBehaviour {
    private bool _active;

    protected void OnEnable() {
        _active = true;
    }

	protected void Update () {
        if (_active) {
            if (Input.GetButton("Submit")) {
                _active = false;
                MenuUI.Instance.SetScreen(MenuUI.Instance.GameScreen);
            }
        }
	}
}
