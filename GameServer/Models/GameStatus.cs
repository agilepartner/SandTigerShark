namespace SandTigerShark.Models
{
    
    public class GameStatus
    {

        public enum Status { IN_PROGRESS, LOST, WON, DRAW };

        public string id;
        public bool canIPlay;
        public Status status;
        public object lastGameState;

        public GameStatus(string id, bool canIPlay, Status status, object lastGameState)
        {
            this.id = id;
            this.canIPlay = canIPlay;
            this.status = status;
            this.lastGameState = lastGameState;
        }

        public string GetId()
        {
            return id;
        }

        public bool CanIPlay()
        {
            return this.canIPlay;
        }

        public Status GetStatus()
        {
            return this.status;
        }

    }

}