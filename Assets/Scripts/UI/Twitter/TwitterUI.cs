using System;
using UnityEngine;
using UnityEngine.UI;

public class TwitterUI : MonoBehaviour {

    [SerializeField] private TwitterPinUI _pinUI;
    [SerializeField] private TwitterPreviewUI _previewWindow;
    [SerializeField] private TwitterLoadingUI _loadingWindow;
    [SerializeField] private TwitterResultUI _resultWindow;

    private string _apiKey = "nx1Thg7kyNo1OlZGnFiMwzscc";
    private string _zomgSekratThing = "";
    private Action<bool> _callback;
    private Texture2D _screenshot;

    Twitter.RequestTokenResponse _requestTokenResponse;
    Twitter.AccessTokenResponse _accessTokenResponse;
    private bool _gotToken;

    const string PLAYER_PREFS_TWITTER_USER_ID = "TwitterUserID";
    const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME = "TwitterUserScreenName";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN = "TwitterUserToken";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

    public void Start() {
        var s = Resources.Load<TextAsset>("tKey.k");
        if (s != null) {
            _zomgSekratThing = s.text.Trim();
        }
        LoadTwitterUserInfo();
    }

    public void SendScreenshot(Texture2D screenshot, Action<bool> callback) {
        _callback = callback;
        //_screenshotData = screenshot.EncodeToJPG(75);
        _screenshot = screenshot;
        if (_gotToken) {
            ShowConfirmTweet();
        } else {
            RequestToken();
        }
    }

    private void ShowConfirmTweet() {
        _previewWindow.Show(_screenshot, ok => {
            if (ok) {
                TryToSendScreenshot();
            } else {
                OnResult(false);
            }
        });
    }

    private void ShowLoading() {
        _loadingWindow.gameObject.SetActive(true);
    }

    private void HideLoading() {
        _loadingWindow.gameObject.SetActive(false);
    }

    private void ShowRequestPIN() {
        HideLoading();
        _pinUI.GetPin(OnPinEntered);
    }

    private void OnPinEntered(string pin) {
        ShowLoading();
        StartCoroutine(Twitter.API.GetAccessToken(_apiKey, _zomgSekratThing, _requestTokenResponse.Token, pin.Trim(), OnAccessTokenCallback));
    }

    private void RequestToken() {
        ShowLoading();
        StartCoroutine(Twitter.API.GetRequestToken(_apiKey, _zomgSekratThing, OnRequestTokenCallback));
    }
    
    private void Print(string str) {
        print(str);
    }

    void LoadTwitterUserInfo() {
        _accessTokenResponse = new Twitter.AccessTokenResponse();

        _accessTokenResponse.UserId = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_ID);
        _accessTokenResponse.ScreenName = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
        _accessTokenResponse.Token = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN);
        _accessTokenResponse.TokenSecret = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET);

        if (!string.IsNullOrEmpty(_accessTokenResponse.Token) &&
            !string.IsNullOrEmpty(_accessTokenResponse.ScreenName) &&
            !string.IsNullOrEmpty(_accessTokenResponse.Token) &&
            !string.IsNullOrEmpty(_accessTokenResponse.TokenSecret)) {
            string log = "LoadTwitterUserInfo - succeeded";
            log += "\n    UserId : " + _accessTokenResponse.UserId;
            log += "\n    ScreenName : " + _accessTokenResponse.ScreenName;
            log += "\n    Token : " + _accessTokenResponse.Token;
            log += "\n    TokenSecret : " + _accessTokenResponse.TokenSecret;
            Print(log);
            _gotToken = true;
        }
    }

    void OnRequestTokenCallback(bool success, Twitter.RequestTokenResponse response) {
        if (success) {
            string log = "OnRequestTokenCallback - succeeded";
            log += "\n    Token : " + response.Token;
            log += "\n    TokenSecret : " + response.TokenSecret;
            Print(log);

            _requestTokenResponse = response;

            Twitter.API.OpenAuthorizationPage(response.Token);
            ShowRequestPIN();
        } else {
            Print("OnRequestTokenCallback - failed.");
            OnFail();
        }
    }

    void OnAccessTokenCallback(bool success, Twitter.AccessTokenResponse response) {
        if (success) {
            string log = "OnAccessTokenCallback - succeeded";
            log += "\n    UserId : " + response.UserId;
            log += "\n    ScreenName : " + response.ScreenName;
            log += "\n    Token : " + response.Token;
            log += "\n    TokenSecret : " + response.TokenSecret;
            Print(log);

            _accessTokenResponse = response;

            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_ID, response.UserId);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME, response.ScreenName);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN, response.Token);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET, response.TokenSecret);

            ShowConfirmTweet();
        } else {
            Print("OnAccessTokenCallback - failed.");
            OnFail();
        }
    }

    private void TryToSendScreenshot() {
        ShowLoading();
        var data = _screenshot.EncodeToJPG(80);
        StartCoroutine(Twitter.API.PostImageTweet("My world at Anu & Ki #ggj16 #anuki", data, _apiKey, _zomgSekratThing, _accessTokenResponse, OnPostTweet));
    }

    private void OnPostTweet(bool success) {
        OnResult(success);
    }

    private void OnFail() {
        OnResult(false);
    }

    private void OnResult(bool success) {
        HideLoading();

        if (_callback != null) {
            var c = _callback;
            _callback = null;
            c(success);
        }

        _screenshot = null;
    }

}
