using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwitterUI : MonoBehaviour {

    [SerializeField] private GameObject PinUI;
    [SerializeField] private GameObject PreviewWindow;
    [SerializeField] private InputField _pinInput;

    private string _apiKey = "nx1Thg7kyNo1OlZGnFiMwzscc";
    private string _zomgSekratThing = "";

    public void Start() {
        var s = Resources.Load<TextAsset>("tKey.k");
        if (s != null) {
            _zomgSekratThing = s.text.Trim();
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            //SendScreenshot();
        }
    }

    public void SendScreenshot() {
        RequestPin();
    }

    public void OnPinEntered() {
        Debug.Log("Entered PIN: " + _pinInput.text);
        PinUI.SetActive(false);
    }

    private void RequestPin() {
        PinUI.SetActive(true);
    }
}
