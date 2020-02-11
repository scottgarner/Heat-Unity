using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


class WebSocketBridge : MonoBehaviour
{

    private ClientWebSocket webSocket = null;

    public delegate void ReceiveAction(string message);
    public event ReceiveAction OnReceived;

    void Start()
    {
        Task connect = Connect("wss://heat-ebs.j38.net/");
    }

    void OnDestroy()
    {
        if (webSocket != null)
            webSocket.Dispose();

        Debug.Log("WebSocket closed.");
    }

    //

    public async Task Connect(string uri)
    {

        try
        {
            webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);

            Debug.Log(webSocket.State);

            Send("{\"channel\":97032862}");

            Task receiveTask = Task.WhenAll(Receive());

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

    }

    private async Task Send(string message)
    {
        var encoded = Encoding.UTF8.GetBytes(message);
        var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);

        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task Receive()
    {

        ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[8192]);

        while (webSocket.State == WebSocketState.Open)
        {

            WebSocketReceiveResult result = null;

            using (var ms = new MemoryStream())
            {
                do
                {
                    result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        string message = reader.ReadToEnd();

                        Debug.Log(message);
                        if (OnReceived != null) OnReceived(message);
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
            }
        }

    }
}
