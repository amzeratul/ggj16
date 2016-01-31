using UnityEngine;
using System.Collections;
using System.Linq;

public class TitleScreenUI : MonoBehaviour {
    [SerializeField] private DanceLibrary _danceLibrary;
    [SerializeField] private MenuUI _ui;
    [SerializeField] private GameObject _danceIcon;

    private bool _active;

    protected void Start() {
        InitIcons();
    }

    private void InitIcons() {
        var validMoves = _danceLibrary.Moves.Where(m => m.Steps.Length != 0).ToArray();
        int i = 0;
        _danceIcon.SetActive(true);
        foreach (var move in validMoves) {
            var go = (GameObject) Instantiate(_danceIcon, _danceIcon.transform.position, _danceIcon.transform.rotation);
            go.transform.SetParent(transform, true);
            var rect = go.GetComponent<RectTransform>();
            float a = 360.0f * i / validMoves.Length;
            var dir = Quaternion.AngleAxis(a, Vector3.forward);
            rect.transform.localPosition = dir * new Vector3(400, 0, 0);
            float s = 0.4f;
            rect.transform.localScale = new Vector3(s, s, s);
            go.GetComponent<DanceIconOnTitle>().Init(move, _ui.DanceIcons[move.Id], dir, a);
            i++;
        }
        _danceIcon.SetActive(false);
    }

    protected void OnEnable() {
        _active = true;
    }

    protected void Update() {
        if (_active) {
            if (Input.GetButton("Submit")) {
                _active = false;
                MenuUI.Instance.SetScreen(MenuUI.Instance.HelpScreen);
            }
        }
    }
}
