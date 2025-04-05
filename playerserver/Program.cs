using System;
using System.ComponentModel;
using System.Text.Json;
using ClassCommand;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Example
{

    public class PlayerConnection : WebSocketBehavior
    {

        public PlayerConnection():base()
        {
            ConnectionManager.Instance.AddConnection(this);
           
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data;
            Message message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(msg);
            //Send(msg);
            ConnectionManager.Instance.SendToAll(msg);
            Console.WriteLine($"{ID}, {Thread.CurrentThread.ManagedThreadId}");
        }
        public void SendMsg(string msg)
        {
            Send(msg);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var wssv = new WebSocketServer("ws://192.168.21.226");

            wssv.AddWebSocketService<PlayerConnection>("/PlayerConnection");
            wssv.Start();
            Console.WriteLine("Wystartowal");

            Message msg = new() { action = ActionType.Login, message = "khfkshfkfsdh" };

            
            string json =  Newtonsoft.Json.JsonConvert.SerializeObject(msg);
            Console.ReadKey(true);
            wssv.Stop();



        }
    }
    public class ConnectionManager
    {
        private static ConnectionManager _instance;

        private List<PlayerConnection> _connections = new List<PlayerConnection>();
        private ConnectionManager() { }

        public static ConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConnectionManager();
                return _instance;

            }

        }

        public void AddConnection(PlayerConnection conn)
        {
            _connections.Add(conn);
        }
        public void SendToAll(string msg)
        {
            foreach (PlayerConnection conn in _connections)
            {
                conn.SendMsg(msg);
            }
        }
    }
}