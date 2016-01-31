UniWeb
------

UniWeb allows you to use a common HTTP api across Unity Web players, iOS
and desktop builds.

See the UniWebServers zip file for simple socket.io and websocket servers 
which you can run using node.js. Note, you will need to move this zip file 
before uncompressing, as the .js files it contains will not compile in Unity.


How to do a HTTP GET request.
-----------------------------

var request = new HTTP.Request("GET", url);
//set headers
request.headers.Set("Hello", "World");
yield return request.Send();
if(request.exception != null) 
    Debug.LogError(request.exception);
else {
    var response = request.response;
    //inspect response code
    Debug.Log(response.status);
    //inspect headers
    Debug.Log(response.headers.Get("Content-Type"));
    //Get the body as a byte array
    Debug.Log(response.bytes);
    //Or as a string
    Debug.Log(response.Text);
}


How to do a HTTP POST request.
------------------------------

A post request is much the same as the GET request, however you assign
a value to the request.bytes field, or the request.Text property.

var request = new HTTP.Request("POST", url);
request.Text = "Hello from UniWeb!";
request.Send();


How to post forms.
------------------

var w = new WWWForm();
w.AddField("hello", "world");
w.AddBinaryData("file", new byte[] { 65,65,65,65 });
var r = new HTTP.Request (url, w);
yield return r.Send();


How to setup the embedded Web Server.
-------------------------------------

Add a HttpServer component to a game object. Configure it with the port you wish to use.

Press play, then use a browser to visit http://localhost:<your port>/. You should get a 
404 Not found message.

To add a URL into the browser, attach a component derived from HttpRequestHandler (see 
HelloWorldHandler.cs for example) which at least overrides the GET method. Set the path
variable on this component to "/" (the root path) or whatever path you prefer, then click 
Play. This time when you visit the url (with the right path) in your browser, you should
get the results of your GET method as text. You can also override PUT, POST and DELETE
methods. Your Unity game is now serving HTTP!


Support
-------

Support is available from support@differentmethods.com.

