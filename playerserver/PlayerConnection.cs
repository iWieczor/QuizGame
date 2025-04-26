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
                    var nickname = JsonConvert.DeserializeObject<string>(msg.Data);
                    ConnectionManager.Instance.AddPlayer(this, nickname);
                    break;
                case ActionType.Answer:
                    var answer = JsonConvert.DeserializeObject<string>(msg.Data);
                    ConnectionManager.Instance.CheckAnswer(this.ID, answer);
                    break;
            }


            
        }

        public void SendMsg(string msg)
        {
            Send(msg);
        }
    }
}