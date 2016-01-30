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
        MakeBeat(1, 1);
        MakeBeat(-1, 1);
    }

    private void MakeBeat(int side, float time) {
        var go = (GameObject) Instantiate(_beatOriginal, _beatOriginal.transform.position, Quaternion.identity);
        go.SetActive(true);
        go.transform.SetParent(transform, true);
        go.transform.localPosition = new Vector3(470 * side, -18, 0);
        go.transform.localScale = Vector3.one;
        go.GetComponent<Beat>().Init(this, -260 * side, 1);
    }

    public void OnExactTick() {
        _blast.Activate();
    }
}
