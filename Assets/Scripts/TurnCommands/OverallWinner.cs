using System.Linq;
using UnityEngine;
using FDaaGF.UI;

namespace FDaaGF.TurnCommands
{
    public class OverallWinner : IGameCommand
    {
        public bool Completed { get; private set; }

        private GameWinnerPanel gameWinnerPanel;

        public OverallWinner(GameWinnerPanel winnerPanel)
        {
            gameWinnerPanel = winnerPanel;
        }

        public void Execute(GameState currentGameState)
        {
            Completed = false;

            var winner = currentGameState.Players.Where(x => x.Position == currentGameState.WinPosition).FirstOrDefault();
            if (winner != null)
            {
                gameWinnerPanel.RpcShow(winner.Name);
            }
            else
            {
                Completed = true;
            }
        }
    }
}