using UnityEngine;
using System.Collections;

public class InGameUI : MonoBehaviour {

    [SerializeField] private GameObject _beatArea;
    [SerializeField] private SessionProgress _progress;
    [SerializeField] private GameObject _restartPad;
    [SerializeField] private GameObject _tweetPad;
    [SerializeField] private GameObject _restartIcon;
    [SerializeField] private GameObject _restartKey;
    [SerializeField] private GameObject _tweetKey;
    [SerializeField] private GameObject _restartIconKey;

    public bool CanRestart { get; set; }
    public bool CanTweet { get; set; }

    protected void OnEnable() {
        _progress.StartSession();
    }

    public void Reset() {
        _beatArea.SetActive(true);
    }

    public void EndOfMatch() {
        _beatArea.SetActive(false);
    }

    protected void Update() {
        _restartPad.SetActive(CanRestart && !CanTweet);
        _tweetPad.SetActive(CanTweet && CanRestart);
        _restartIcon.SetActive(CanRestart);
        _restartKey.SetActive(CanRestart && !CanTweet);
        _tweetKey.SetActive(CanTweet && CanRestart);
        _restartIconKey.SetActive(CanRestart);
    }

    public void ShowDanceMove(DanceMove dance) {
        // TODO
    }
}
