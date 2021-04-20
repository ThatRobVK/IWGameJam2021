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

            var winners = currentGameState.Players.Where(x => x.Position == currentGameState.WinPosition).ToList();
            if (winners.Count > 0)
            {
                // Multiple winners, the one with the most sacrifices wins
                var winner = winners.OrderByDescending(x => x.TotalSacrifices).First();
                gameWinnerPanel.RpcShow(winner.Name);
            }
            else if (winners.Count == 1)
            {
                // Single winner
                gameWinnerPanel.RpcShow(winners[0].Name);
            }
            else
            {
                // No winner, complete this command
                Completed = true;
            }
        }
    }
}