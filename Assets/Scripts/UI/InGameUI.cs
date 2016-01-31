using UnityEngine;
using System.Collections;

public class InGameUI : MonoBehaviour {

    [SerializeField] private GameObject _beatArea;
    [SerializeField] private SessionProgress _progress;

    protected void OnEnable() {
        _progress.StartSession();
    }

    public void Reset() {
        _beatArea.SetActive(true);
    }

    public void EndOfMatch() {
        _beatArea.SetActive(false);
    }

    public void WaitingRestart() {
        // TODO
    }

    public void ShowDanceMove(DanceMove dance) {
        // TODO
    }
}
