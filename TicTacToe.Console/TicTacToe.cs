using System;
using System.Threading.Tasks;

namespace SandTigerShark.TicTacToe.App
{
    internal sealed class TicTacToe
    {
        private readonly TicTacToeRepository repository;
        public Guid Id { get; private set; }
        public bool InProgress { get; private set; }

        public TicTacToe(TicTacToeRepository repository)
        {
            this.repository = repository;
            InProgress = true;
        }

        internal async Task Start()
        {
            Id = await repository.CreateOrGetAvailableGame();
        }

        internal async Task<PlayResult> Play(int position)
        {
            return await repository.Play(Id, position);
        }
    }
}