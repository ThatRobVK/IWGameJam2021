using System.Linq;
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

            // Sort the players so the first one is the winner
            var playersOrdered = currentGameState.Players.OrderByDescending(x => x.TotalOffer).ToList();

            // Increase the first player's position
            playersOrdered[0].Position++;

            winnerPanel.ShowPanels(playersOrdered);
        }
    }
}