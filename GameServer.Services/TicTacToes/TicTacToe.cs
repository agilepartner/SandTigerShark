﻿using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Http;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.TicTacToes
{
    internal class TicTacToe : Game
    {
        private readonly IRestProxy restProxy;
        private readonly string url;
        public override GameType Type => GameType.TicTacToe;

        public TicTacToe(
            IRestProxy restProxy,
            AzureConfig configuration,
            Guid userToken)
            : base(userToken)
        {
            this.restProxy = restProxy;
            url = configuration.TicTacToe;
            LastGameState = CreateBoard();
        }

        private static int[] CreateBoard()
        {
            var board = new int[9];

            for (var i = 0; i < board.Length; i++)
            {
                board[i] = 0;
            }
            return board;
        }
        
        protected override async Task PlayInternally(Commands.Play command, Guid player)
        {
            int position = 0;

            if(command.Command == null ||
                !int.TryParse(command.Command.ToString(), out position))
            {
                throw new InvalidCommandException("Position is required in the command.");
            }

            var play = new Play
            {
                Board = (int[])LastGameState,
                Player = GetPlayerNumber(player),
                Position = position
            };
            //TODO handle exception properly here
            var boardState = await restProxy.PostAsync<Play, int[]>(url, play);
            LastGameState = boardState;
        }
    }
}