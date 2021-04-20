using System.Linq;
using System.Collections.Generic;
using FDaaGF.UI;

namespace FDaaGF.TurnCommands
{
    public class DecideWinner : IGameCommand
    {

        public bool Completed { get; private set; }

        private RoundWinnerPanel winnerPanel;

        public DecideWinner(RoundWinnerPanel winnerPanel)
        {
            this.winnerPanel = winnerPanel;
            winnerPanel.OnTimerCompleted += CompleteCommand;
            winnerPanel.Hide();
        }

        // Hides the panel and flags the command as completed
        private void CompleteCommand()
        {
            winnerPanel.RpcHide();
            Completed = true;
        }

        // Runs the command
        public void Execute(GameState currentGameState)
        {
            Completed = false;

            var highestBid = currentGameState.Players.Max(x => x.TotalOffer);
            // Highest bidders with sacrifice win
            var winners = currentGameState.Players.Where(x => x.TotalOffer == highestBid && x.CurrentSacrifice).ToList();
            // If no winners, then all highest bidders
            if (winners.Count == 0) winners = currentGameState.Players.Where(x => x.TotalOffer == highestBid).ToList();
            // Losers are not winners
            var losers = currentGameState.Players.Where(x => !winners.Contains(x)).ToList();

            // Increase the winners' position
            winners.ForEach(x => x.Position++);

            winnerPanel.ShowPanels(winners, losers);
        }
    }
}