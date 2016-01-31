using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwitterPinUI : MonoBehaviour {

    [SerializeField] private TwitterUI _main;
    [SerializeField] private InputField _pinInput;
    private Action<string> _callback;

    public void GetPin(Action<string> onPinEntered) {
        _pinInput.text = "";
        gameObject.SetActive(true);
        _callback = onPinEntered;
    }

    public void OnPin() {
        _callback(_pinInput.text);
        gameObject.SetActive(false);
    }
}
