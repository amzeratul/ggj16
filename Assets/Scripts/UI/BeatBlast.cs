using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class BeatBlast : MonoBehaviour {
    private Image _renderer;

    protected void Start () {
	    _renderer = GetComponent<Image>();
        _renderer.color = new Color(1, 1, 1, 1);
	}
	
    public void Activate() {
        StartCoroutine(DoActivate());
    }

    private IEnumerator DoActivate() {
        for (float t = 0; t < 1; t += Time.deltaTime / 0.2f) {
            //_renderer.color = new Color(1, 1, 1, 1 - t);
            float s = 1.0f + 0.3f * Mathf.Sin(t * Mathf.PI);
            transform.localScale = new Vector3(s, s, 1);
            yield return null;
        }
        transform.localScale = Vector3.one;
        //_renderer.color = new Color(1, 1, 1, 1);
    }
}
