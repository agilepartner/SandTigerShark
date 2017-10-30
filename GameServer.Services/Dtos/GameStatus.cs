using SandTigerShark.GameServer.Services.Games;

namespace SandTigerShark.GameServer.Services.Dtos
{
    public class GameStatus
    {
        public object LastState { get; set; }
        public bool CanIPlay { get; set; }
        public Status Status { get; set; }
    }
}