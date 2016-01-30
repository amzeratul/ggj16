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
        MakeBeat(470, 1);
        MakeBeat(-470, 1);
    }

    private void MakeBeat(float x, float time) {
        var go = (GameObject) Instantiate(_beatOriginal, _beatOriginal.transform.position, Quaternion.identity);
        go.SetActive(true);
        go.transform.SetParent(transform, true);
        go.transform.localPosition = new Vector3(x, -18, 0);
        //go.GetComponent<RectTransform>().localPosition = new Vector3(x, -18, 0);
        go.GetComponent<Beat>().Init(this, -x, 1);
    }

    public void OnExactTick() {
        _blast.Activate();
    }
}
