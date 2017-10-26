using System;
using System.Collections.Generic;
using System.Text;

namespace SandTigerShark.GameServer.Services.Models
{
    public class Instruction
    {
        public string UserToken { get; set; }
        public string GameToken { get; set; }

        //Specific to TicTacToe game
        public int Play { get; set; }

    }
}
