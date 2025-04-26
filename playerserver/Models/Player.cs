using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playerserver.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public int Score { get; set; }
    }
}
