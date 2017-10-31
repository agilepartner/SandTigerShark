using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Exceptions;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Games
{
    public abstract class Game
    {
        public Guid Id { get; }
        public abstract GameType Type { get; }
        public Status State { get; protected set; }
        public object LastGameState { get; protected set; }
        public Guid? Player1 { get; private set; }
        public Guid? Player2 { get; private set; }

        protected Game(Guid userToken)
        {
            State = Status.Created;
            Id = Guid.NewGuid();
            AddPlayer(userToken);
        }

        public bool IsAvailable()
        {
            return Player1 == null || Player2 == null;
        }

        protected int GetPlayerNumber(Guid player)
        {
            if(Player1 == player)
            {
                return 1;
            }
            return 2;
        }

        private bool IsPlaying(Guid player)
        {
            return Player1 == player || Player2 == player;
        }

        public void AddPlayer(Guid player)
        {
            if(!IsAvailable())
            {
                throw new GameNotAvailableException(Id);
            }

            if (!IsPlaying(player))
            {
                if (Player1.HasValue)
                {
                    Player2 = player;
                }
                else
                {
                    Player1 = player;
                }
            }
        }

        public async Task Play(Play command, Guid player)
        {
            if(IsAvailable() && !IsPlaying(player))
            {
                AddPlayer(player);
            }

            if(!IsAvailable() && !IsPlaying(player))
            {
                throw new NotAuthorizedException($"Player {player} is not authorized to play in the game {Id}");
            }
            await PlayInternally(command, player);
        }

        protected abstract Task PlayInternally(Play command, Guid player);
    }
}