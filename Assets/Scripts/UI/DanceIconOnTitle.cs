using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DanceIconOnTitle : MonoBehaviour {
    private float _time;
    private RectTransform _transform;
    private Vector3 _startPos;
    private Quaternion _dir;

    protected void Update () {
	    _time += Time.deltaTime;
        _transform.localPosition = _startPos + _dir * new Vector3(Mathf.Sin(_time) * 20, 0, 0);
    }

    public void Init(DanceMove move, Sprite danceIcon, Quaternion dir, float phase) {
        _transform = GetComponent<RectTransform>();
        _startPos = _transform.localPosition;
        _dir = dir;
        _time = phase / 20.0f;
        var render = GetComponent<Image>();
        render.sprite = danceIcon;
        render.color = new Color(1, 1, 1, DancesExecuted.Instance.HasExecutedDance(move.Id) ? 1 : 0.2f);
    }
}
