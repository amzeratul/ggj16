using UnityEngine;
using System.Collections;

public class WorldObjectAnim : MonoBehaviour, Rhythm.Listener {
    [SerializeField] private int _beats = 4;
    [SerializeField] private AnimType _animType;

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
        transform.rotation = BaseRotation * Quaternion.AngleAxis(Mathf.Lerp(-10, 10, t), Vector3.forward);
    }

    private void UpdateSquish(float t) {
        transform.localScale = new Vector3(Mathf.Lerp(0.9f, 1.1f, t), Mathf.Lerp(1.1f, 0.9f, t), 1);
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
