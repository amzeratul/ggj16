using UnityEngine;
using System.Collections;

public class InGameUI : MonoBehaviour {

    [SerializeField] private SessionProgress _progress;

    protected void OnEnable() {
        _progress.StartSession();
    }
}
