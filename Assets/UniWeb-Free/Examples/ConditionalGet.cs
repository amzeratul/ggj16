using UnityEngine;
using System.Collections;

public class ConditionalGet : MonoBehaviour {

	// Use this for initialization
    IEnumerator Start () {
        var uri = "http://www.differentmethods.com/packages/SpatialAudioSourceDemo.html";

        var req = new HTTP.Request("GET", uri);
        req.useCache = true;
        req.headers.Set("If-None-Match", "xyzzy");
        yield return req.Send();
        Debug.Log(req.response.headers);

        Debug.Log ("Using Etag: " + req.response.headers.Get("ETag"));
        var newReq = new HTTP.Request("GET", uri);
        newReq.useCache = true;
        newReq.headers.Set("If-None-Match", req.response.headers.Get("ETag"));
        yield return newReq.Send();
        Debug.Log(newReq.response.headers);

	
	}
	
	
}
