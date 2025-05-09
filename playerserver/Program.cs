﻿using System;
using System.ComponentModel;
using ClassCommand;
using Newtonsoft.Json;
using playerserver;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Example
{

    public class Program : PlayerConnection
    {
        public static void Main(string[] args)
        {
            var wssv = new WebSocketServer("ws://192.168.20.37");
            Message m = new Message() { Action = ActionType.Question, Data = "test test" };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(m);

            wssv.AddWebSocketService<PlayerConnection>("/PlayerConnection");
            wssv.Start();
            Console.WriteLine("Wystartowal");

            //Message msg = new() { Action = ActionType.Login, message = "khfkshfkfsdh" };

            
            //string json =  Newtonsoft.Json.JsonConvert.SerializeObject(msg);
            Console.ReadKey(true);
            wssv.Stop();



        }
    }
}
