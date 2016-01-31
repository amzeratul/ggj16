using UnityEngine;
using System.Collections;


//See UniWebServers/Node/WebsocketServerExample.js for the equivalent server.

public class WebsocketClientExample : MonoBehaviour {

    public string url = "http://localhost:8080";
	
	IEnumerator Start () {
        var ws = new HTTP.WebSocket ();

        //This lets the websocket connection work asynchronously.
        StartCoroutine (ws.Dispatcher ());

        //Connect the websocket to the server.
        ws.Connect (url);

        //Wait for the connection to complete.
        yield return ws.Wait();

        //Always check for an exception here!
        if(ws.exception != null) {
            Debug.Log("An exception occured when connecting: " + ws.exception);
        }

        //Display connection status.
        Debug.Log("Connected? : " + ws.connected);

        //If websocket connected succesfully, we can send and receive messages.
        if(ws.connected) {
            //This specified that our OnStringReceived method will be called when a message is received.
            ws.OnTextMessageRecv += OnStringReceived;
            //This sends a message to the server.
            ws.Send ("Hello!");

            ws.Send ("Goodbye");
        }
	}

    void OnStringReceived(string msg) {
        Debug.Log ("From server -> " + msg);
    }
}
