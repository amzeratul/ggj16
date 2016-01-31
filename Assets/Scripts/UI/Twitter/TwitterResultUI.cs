using UnityEngine;
using System.Collections;

public class TwitterResultUI : MonoBehaviour {

    [SerializeField] private GameObject _good;
    [SerializeField] private GameObject _bad;

    public void ShowResult(bool success) {
        _good.SetActive(success);
        _bad.SetActive(!success);
        gameObject.SetActive(true);
        StartCoroutine(DoShowResult());
    }

    private IEnumerator DoShowResult() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
