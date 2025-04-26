using System;
using WebSocketSharp.Server;
using WebSocketSharp;
using Newtonsoft.Json;
using playerserver.Models;
using ClassCommand;

namespace playerserver
{
    public class PlayerConnection : WebSocketBehavior
    {
        public PlayerConnection() : base()
        {
            ConnectionManager.Instance.AddConnection(this);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast(e.Data);

            
            
            var msg = JsonConvert.DeserializeObject<Message>(e.Data);
            switch (msg.Action)
            {
                case ActionType.Login:
                    var nickname = msg.Data;
                    ConnectionManager.Instance.AddPlayer(this, nickname);
                    DisplayLogins();
                    break;
                case ActionType.Answer:
                    var answer =msg.Data;
                    ConnectionManager.Instance.CheckAnswer(this.ID, answer);
                    break;
            }
           // Sessions.Broadcast(msg.Data);




        }

        private void DisplayLogins()
        {
            foreach (var p in ConnectionManager.Instance.Players)
            {
                Sessions.Broadcast(p.Value.Nickname);

            }
        }

        public void SendMsg(string msg)
        {
            Send(msg);
        }
    }
}