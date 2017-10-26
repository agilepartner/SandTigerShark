namespace SandTigerShark.Services.Models
{
    public class GameStatus
    {
        public enum Status { IN_PROGRESS, LOST, WON, DRAW };

        public string Id { get; private set; }
        public bool CanIPlay { get; set; }
        public Status State { get; set; }
        public object LastGameState { get; set; }

        public GameStatus(string id, bool canIPlay, Status status, object lastGameState)
        {
            Id = id;
            CanIPlay = canIPlay;
            State = status;
            LastGameState = lastGameState;
        }
    }
}