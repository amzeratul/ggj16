using UnityEngine;
using System.Collections;

public class Beat : MonoBehaviour {
    private float _speed;
    private float _ttl;
    private float _time;
    private bool _activated;
    private SpriteRenderer _renderer;
    private BeatVisualizer _visualizer;

    protected void Start () {
	    _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = new Color(1, 1, 1, 0);
	}
	
	protected void Update () {
	    _time += Time.deltaTime;

	    float a = Mathf.Clamp01(Mathf.Min(_time, _ttl - _time) / 0.5f);
	    _renderer.color = new Color(1, 1, 1, a);
	    transform.localPosition += new Vector3(_speed * Time.deltaTime, 0, 0);
	    if (!_activated && transform.localPosition.x < 0) {
	        _activated = true;
            _visualizer.OnExactTick();
	    }

	    if (_time > _ttl) {
	        DestroyObject(gameObject);
	    }
	}

    public void Init(BeatVisualizer beatVisualizer, float speed, float ttl) {
        _visualizer = beatVisualizer;
        _speed = speed;
        _ttl = ttl;
    }
}
