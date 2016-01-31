using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class RequestBenchmark : MonoBehaviour
{

    
    IEnumerator Start ()
    {
        var uniwebSeconds = 0.0;
        var wwwSeconds = 0.0;

        //Warm up engines.
        yield return new HTTP.Request ("GET", "http://www.differentmethods.com/").Send();
        yield return new WWW ("http://www.differentmethods.com/");

        for (var i=0; i<10; i++) {
            var clock = new Stopwatch ();
            var req = new HTTP.Request ("GET", "http://www.differentmethods.com/");
            clock.Start ();
            yield return req.Send ();
            clock.Stop ();
            uniwebSeconds += clock.Elapsed.TotalSeconds;
        }
        for (var i=0; i<10; i++) {
            var clock = new Stopwatch ();
            var req = new WWW ("http://www.differentmethods.com/");
            clock.Start ();
            yield return req;
            clock.Stop ();
            wwwSeconds += clock.Elapsed.TotalSeconds;
        }

        UnityEngine.Debug.Log("UniWeb: " + (uniwebSeconds/10));
        UnityEngine.Debug.Log("WWW: " + (wwwSeconds/10));
    }
    
}
