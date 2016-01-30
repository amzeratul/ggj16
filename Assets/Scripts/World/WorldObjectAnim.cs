using UnityEngine;
using System.Collections;

public class WorldObjectAnim : MonoBehaviour, Rhythm.Listener {
    [SerializeField] private int _beats = 4;
    [SerializeField] private AnimType _animType;
    [SerializeField] private float _intensity = 1;

    private float _bpm;
    private bool _animating;
    private float _time;
    private int _numTicks;

    public enum AnimType {
        Swing,
        Squish
    }

    public Quaternion BaseRotation = Quaternion.identity;

    protected void Start () {
        Rhythm.Instance.Register(this);
	}

    protected void OnDestroy() {
        Rhythm.Instance.Remove(this);
    }
	
	protected void Update () {
	    float t = 0.5f;
	    if (_animating) {
	        _time += _bpm / (60.0f * _beats) * Time.deltaTime;
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

    private void UpdateSwing(float t) {
        float range = 10 * _intensity;
        transform.rotation = BaseRotation * Quaternion.AngleAxis(Mathf.Lerp(-range, range, t), Vector3.forward);
    }

    private void UpdateSquish(float t) {
        float min = 1.0f - 0.1f * _intensity;
        float max = 1.0f + 0.1f * _intensity;
        transform.localScale = new Vector3(Mathf.Lerp(min, max, t), Mathf.Lerp(max, min, t), 1);
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
