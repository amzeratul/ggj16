using UnityEngine;
using System.Collections;

public class BeatVisualizer : MonoBehaviour, Rhythm.Listener {

    [SerializeField] private Rhythm _rhythm;
    [SerializeField] private GameObject _beatOriginal;
    [SerializeField] private BeatBlast _blast;
    
	protected void Start () {
	    _beatOriginal.SetActive(false);
        _rhythm.Register(this, 1);
	}

    protected void OnDestroy() {
        _rhythm.Remove(this);
    }

    public void OnTick() {
        var go = (GameObject) Instantiate(_beatOriginal, _beatOriginal.transform.position, Quaternion.identity);
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(4, 0, 0);
        go.SetActive(true);
        go.GetComponent<Beat>().Init(this, -4, 2);
    }

    public void OnExactTick() {
        _blast.Activate();
    }
}
