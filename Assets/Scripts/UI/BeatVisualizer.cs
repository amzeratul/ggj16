using UnityEngine;
using System.Collections;

public class BeatVisualizer : MonoBehaviour {

    [SerializeField] private GameObject _beatOriginal;
    [SerializeField] private BeatBlast _blast;
    
	protected void Start () {
	    _beatOriginal.SetActive(false);
	}
	
	protected void Update () {
	    
	}

    public void OnDanceTick(bool warmUp) {
        if (!warmUp) {
            _blast.Activate();
        }

        // Scrolling beat
        var go = (GameObject) Instantiate(_beatOriginal, _beatOriginal.transform.position, Quaternion.identity);
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(4, 0, 0);
        go.SetActive(true);
        go.GetComponent<Beat>().Init(-4, 2);
    }
}
