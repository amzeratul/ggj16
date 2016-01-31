using UnityEngine;
using System.Collections;

public class KeepAliveExample : MonoBehaviour
{

    void Start ()
    {
        StartCoroutine (TestKeepAlive ());

    }

    void OnGUI ()
    {
        if (GUILayout.Button ("Restart")) {
            Start ();
        }

    }

    IEnumerator TestKeepAlive ()
    {
        for (var i = 0; i < 3; i++) {

            var r = new HTTP.Request ("GET", "http://google.com/");
            yield return r.Send ();
            if (r.exception == null) {
                Debug.Log (r.response.status);
            } else {
                Debug.LogError (r.exception);
            }
            yield return new WaitForSeconds(1);

        }

    }

}
