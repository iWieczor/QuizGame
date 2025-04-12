using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassCommand;
using Example;
using Newtonsoft.Json;
using playerserver.Models;

namespace playerserver
{
    public class ConnectionManager
    {
        private static ConnectionManager _instance;

        private List<PlayerConnection> _connections = new List<PlayerConnection>();
        private Dictionary<string, Player> _players = new();
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
        public void AddPlayer(PlayerConnection conn, string nickname)
        {
            _players[conn.ID] = new Player
            {
                Id = conn.ID,
                Nickname = nickname,
                Score = 0
            };
        }
        private List<Question> _questions = new List<Question>();
        private int _currentQuestionIndex = -1;

        public void LoadQuestions(List<Question> questions)
        {
            _questions = questions;
        }

        public void StartGame()
        {
            _currentQuestionIndex = 0;
            SendNextQuestion();
        }
        public Question GetCurrentQuestion()
        {
            if (_currentQuestionIndex < 0 || _currentQuestionIndex >= _questions.Count)
            {
                throw new InvalidOperationException("Brak dostępnych pytań");
            }
            return _questions[_currentQuestionIndex];
        }

        public void SendNextQuestion()
        {
            _currentQuestionIndex++;
            if (_currentQuestionIndex >= _questions.Count)
            {
                EndGame();
                return;
            }

            var question = _questions[_currentQuestionIndex];
            var msg = new Message
            {
                Action = ActionType.Question,
                Data = JsonConvert.SerializeObject(question)
            };
            SendToAll(JsonConvert.SerializeObject(msg));
        }

        public void EndGame()
        {
            var msg = new Message { Action = ActionType.EndGame };
            SendToAll(JsonConvert.SerializeObject(msg));
        }
        public void CheckAnswer(string playerId, string answer)
        {
            try
            {
                var currentQuestion = GetCurrentQuestion();
                if (answer == currentQuestion.CorrectAnswer && _players.TryGetValue(playerId, out var player))
                {
                    player.Score += 10;
                    var scoreMsg = new Message
                    {
                        Action = ActionType.UpdateScore,
                        Data = JsonConvert.SerializeObject(_players.Values)
                    };
                    SendToAll(JsonConvert.SerializeObject(scoreMsg));
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
