using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.WebSockets
{
    public class WebSocketGameServer
    {
        private readonly HttpListener _listener = new();
        private readonly WebSocketConnectionHandler _connectionHandler;

        public WebSocketGameServer(WebSocketConnectionHandler connectionHandler)
        {
            _connectionHandler = connectionHandler;
        }

        public async Task StartAsync(string url, CancellationToken token)
        {
            _listener.Prefixes.Add(url);
            _listener.Start();
            Console.WriteLine($"🌐 WebSocket server listening at {url}");

            while (!token.IsCancellationRequested)
            {
                var context = await _listener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    _ = Task.Run(() => _connectionHandler.HandleConnectionAsync(wsContext.WebSocket));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }

            _listener.Stop();
            Console.WriteLine("🧵 WebSocket server has shut down.");
        }

    }
}
