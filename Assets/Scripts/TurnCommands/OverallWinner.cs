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
                // Someone has won, show the panel and don't flag completed
                gameWinnerPanel.RpcShow(winner.Name);
            }
            else
            {
                // No winner, complete this command
                Completed = true;
            }
        }
    }
}