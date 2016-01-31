using UnityEngine;
using System.Collections;

public class ForceProxyExample : MonoBehaviour
{

    void Start ()
    {
        //This sets the proxy for ALL http requests. This address is the default address used by Charles.
        HTTP.Request.proxy = new System.Uri("http://127.0.0.1:8888");

        StartCoroutine (FetchAssetBundle());
    }


    void OnGUI ()
    {
        if (GUILayout.Button ("Restart")) {
            Start ();
        }

    }

    IEnumerator FetchAssetBundle() {
        var r = new HTTP.Request("GET", "http://differentmethods.com/~simon/uniwebtest.unity3d?xyzzy");
        yield return r.Send();
        if(r.exception == null) {
            Debug.Log(r.response.status);
            var abcr = r.response.AssetBundleCreateRequest();
            yield return abcr;
            Debug.Log(abcr.assetBundle);
        } else {
            Debug.LogError(r.exception);
        }
    }

}
