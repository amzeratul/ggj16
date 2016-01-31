using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DanceIconOnTitle : MonoBehaviour {
    public Color ActiveCol = new Color(1, 1, 1, 1);
    public Color InactiveCol = new Color(1, 1, 1, 0.2f);

    private float _time;
    private RectTransform _transform;
    private Vector3 _startPos;
    private Quaternion _dir;
    private bool _setup;
    private int _danceId;

    protected void Update () {
	    _time += Time.deltaTime;
        _transform.localPosition = _startPos + _dir * new Vector3(Mathf.Sin(_time) * 20, 0, 0);
    }
    
    public void Init(DanceMove move, Sprite danceIcon, Quaternion dir, float phase) {
        _setup = true;
        _transform = GetComponent<RectTransform>();
        _startPos = _transform.localPosition;
        _dir = dir;
        _time = phase / 20.0f;
        var render = GetComponent<Image>();
        render.sprite = danceIcon;
        _danceId = move.Id;
        render.color = DancesExecuted.Instance.HasExecutedDance(_danceId) ? ActiveCol : InactiveCol;
    }

    protected void OnEnable() {
        if (_setup) {
            var render = GetComponent<Image>();
            render.color = DancesExecuted.Instance.HasExecutedDance(_danceId) ? ActiveCol : InactiveCol;
        }
    }
}
