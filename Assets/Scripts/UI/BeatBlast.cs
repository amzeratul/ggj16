using UnityEngine;
using System.Collections;

public class BeatBlast : MonoBehaviour {
    private SpriteRenderer _renderer;

    protected void Start () {
	    _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = new Color(1, 1, 1, 0);
	}
	
    public void Activate() {
        StartCoroutine(DoActivate());
    }

    private IEnumerator DoActivate() {
        for (float t = 0; t < 1; t += Time.deltaTime / 0.2f) {
            _renderer.color = new Color(1, 1, 1, 1 - t);
            float s = t * 2 + 0.5f;
            transform.localScale = new Vector3(s, s, 1);
            yield return null;
        }
        _renderer.color = new Color(1, 1, 1, 0);
    }
}
