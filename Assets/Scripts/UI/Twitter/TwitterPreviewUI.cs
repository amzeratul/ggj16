using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwitterPreviewUI : MonoBehaviour {

    [SerializeField] private RawImage _preview;
    
    private Action<bool> _callback;

    public void Show(Texture2D screenshot, Action<bool> callback) {
        _preview.texture = screenshot;
        _callback = callback;
        gameObject.SetActive(true);
    }

    protected void Update() {
        if (Input.GetButtonDown("Submit")) {
            OnResult(true);
        } else if (Input.GetButtonDown("Cancel")) {
            OnResult(false);
        }
    }

    public void OnOK() {
        OnResult(true);
    }

    public void OnCancel() {
        OnResult(false);
    }

    private void OnResult(bool confirm) {
        _callback(confirm);
        gameObject.SetActive(false);
    }
}
