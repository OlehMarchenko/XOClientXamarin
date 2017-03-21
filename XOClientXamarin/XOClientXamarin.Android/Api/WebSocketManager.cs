using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace XOClientXamarin.Droid.Api
{
    public class WebSocketManager
    {
        private ClientWebSocket webSocket;
        public event EventHandler OnMessage;

        public WebSocketManager()
        {
            webSocket = new ClientWebSocket();
            webSocket.ConnectAsync(new Uri("ws://10.0.3.2:8080/"), System.Threading.CancellationToken.None);
        }

        public async void Send(string message)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }

        public async Task<string> Read()
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(new Byte[8192]);
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(segment, System.Threading.CancellationToken.None);
            string message = Encoding.UTF8.GetString(segment.Array);
            return message;
        }

        private async void ListenLoop()
        {
            while (true)
            {
                OnMessage(await Read(), null);
            }
        }

        public void StartListen()
        {
            new Task(() => ListenLoop()).Start();
        }
    }
}