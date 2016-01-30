using UnityEngine;
using System.Collections;

public class WorldObjectAnim : MonoBehaviour, Rhythm.Listener {
    [SerializeField] private int _beats = 4;
    [SerializeField] private AnimType _animType;
    [SerializeField] private float _intensity = 1;

    private float _bpm;
    private bool _animating;
    private float _spawnTime;
    private float _time;
    private int _numTicks;
    private bool _spawning;

    public enum AnimType {
        Swing,
        Squish
    }

    public Quaternion BaseRotation = Quaternion.identity;
    private Vector3 _spawnScale = Vector3.one;
    private Vector3 _animScale = Vector3.one;

    protected void Start () {
        BaseRotation = transform.rotation;
        Rhythm.Instance.Register(this);
        transform.localScale = Vector3.zero;
    }

    public void OnSpawn() {
        _spawning = true;
    }

    protected void OnDestroy() {
        Rhythm.Instance.Remove(this);
    }
	
	protected void Update () {
	    if (_spawning) {
	        UpdateSpawning();
	    }

	    UpdateNormal();

	    transform.localScale = new Vector3(_spawnScale.x * _animScale.x, _spawnScale.y * _animScale.y, 1);
	}

    private void UpdateNormal() {
        float t = 0.5f;
        if (_animating) {
            _time = (_time + (_bpm / (60.0f * _beats) * Time.deltaTime)) % 1.0f;
            t = MathUtil.Smooth(Mathf.Abs(1.0f - Mathf.Abs(1 - 2 * (_time + 0.25f))));
        }

        switch (_animType) {
        case AnimType.Squish:
            UpdateSquish(t);
            break;
        case AnimType.Swing:
            UpdateSwing(t);
            break;
        }
    }

    private void UpdateSpawning() {
        _spawnTime += Time.deltaTime;
        float len = 1.0f;
        if (_spawnTime >= len) {
            _spawning = false;
            _spawnTime = 0;
            _spawnScale = new Vector3(1, 1, 1);
        } else {
            float t = MathUtil.Overshoot(_spawnTime / len);
            _spawnScale = new Vector3(t, t, t);
        }
    }

    private void UpdateSwing(float t) {
        float range = 10 * _intensity;
        transform.rotation = BaseRotation * Quaternion.AngleAxis(Mathf.Lerp(-range, range, t), Vector3.forward);
    }

    private void UpdateSquish(float t) {
        float min = 1.0f - 0.1f * _intensity;
        float max = 1.0f + 0.1f * _intensity;
        _animScale = new Vector3(Mathf.Lerp(min, max, t), Mathf.Lerp(max, min, t), 1);
    }

    public void OnTick() {
        _animating = true;
        if (_numTicks % _beats == 0) {
            _bpm = Rhythm.Instance.BPM;
            _time = 0;
        }
        _numTicks++;
    }
}
