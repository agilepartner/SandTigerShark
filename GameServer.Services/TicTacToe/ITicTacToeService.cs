using SandTigerShark.GameServer.Services.Commands;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.TicTacToe
{
    public interface ITicTacToeService
    {
        Task Play(Play command);
    }
}