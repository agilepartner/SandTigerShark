namespace SandTigerShark.GameServer.Services.TicTacToes
{
    class PlayResult
    {
        public int[] Board { get; set; }
        public bool GameOver { get; set; }
        public int? Winner { get; set; }
    }
}