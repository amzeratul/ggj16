using UnityEngine;
using System.Collections;

public class TwitterLoadingUI : MonoBehaviour {

    [SerializeField] private RectTransform _spinny;
    private float _time;

    protected void OnEnable() {
        _time = 0;
    }
    
	protected void Update () {
	    _time += Time.deltaTime;
	    _spinny.localRotation = Quaternion.AngleAxis((_time * 720) % 360, Vector3.forward);
	}
}
