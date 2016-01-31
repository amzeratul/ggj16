using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DanceIcon : MonoBehaviour {
    private Image _image;

    protected void Awake() {
        _image = GetComponent<Image>();
        _image.color = new Color(1, 1, 1, 0);
    }

    public void OnEnable() {
        _image.color = new Color(1, 1, 1, 0);
    }

    public void ShowDanceIcon(int id) {
        _image.sprite = MenuUI.Instance.DanceIcons[id];
        _image.SetNativeSize();
        StartCoroutine(DoShowDanceIcon());
    }

    private IEnumerator DoShowDanceIcon() {
        var opaque = new Color(1, 1, 1, 1);
        var transparent = new Color(1, 1, 1, 0);
        _image.color = opaque;
        for (int i = 0; i < 2; i++) {
            yield return new WaitForSeconds(0.05f);
            _image.color = transparent;
            yield return new WaitForSeconds(0.05f);
            _image.color = opaque;
        }
        yield return new WaitForSeconds(0.5f);
        for (float t = 0; t < 1; t += Time.deltaTime * 2) {
            _image.color = new Color(1, 1, 1, 1 - t);
            yield return null;
        }
        _image.color = transparent;
    }
}
