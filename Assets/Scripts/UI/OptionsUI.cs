using UnityEngine;
using System.Collections;

public class OptionsUI : MonoBehaviour {

    [SerializeField] private Component _target;
    [SerializeField] private GameObject[] _options;
    private int _option;
    private float _cooldown;

    protected void OnEnable() {
        _option = 0;
        _cooldown = 0.2f;
    }

    protected void Update() {
        if (_cooldown >= 0) {
            _cooldown -= Time.deltaTime;
        }

        for (int i = 0; i < _options.Length; i++) {
            _options[i].SetActive(i == _option);
        }

        float input = Input.GetAxis("Joy0 LeftX") + Input.GetAxis("Joy0 RightX");
        if (input < -0.7f && _cooldown <= 0) {
            Move(-1);
        }
        if (input > 0.7f && _cooldown <= 0) {
            Move(1);
        }
        if (Mathf.Abs(input) < 0.3f) {
            _cooldown = 0;
        }

        if (Input.GetButtonDown("Submit")) {
            //_chose = true;
            _target.SendMessage("OnOptionChosen", _option);
        }
    }

    private void Move(int i) {
        _option = (_option + _options.Length + i) % _options.Length;
        _cooldown = 0.2f;
    }
}
