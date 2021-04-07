using System;
using System.Linq;
using FDaaGF.UI;
using UnityEngine;

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
        }

        // Hides the panel and flags the command as completed
        private void CompleteCommand()
        {
            winnerPanel.Hide();
            Completed = true;
        }

        // Runs the command
        public void Execute(GameState currentGameState)
        {
            Completed = false;

            // Sort the players so the first one is the winner
            var playersOrdered = currentGameState.Players.OrderByDescending(x => x.CurrentOffer).ToList();

            // Increase the first player's position
            playersOrdered[0].Position++;

            // Show the winners
            // Congratulate the winner
            winnerPanel.RpcShowWinnerPanel(playersOrdered[0].Connection, playersOrdered[0].Name);

            // Poke fun at the losers
            for (int i = 1; i < playersOrdered.Count; i++)
            {
                Debug.LogFormat("Telling {0} to display the loser panel", playersOrdered[i].Name);
                winnerPanel.RpcShowLoserPanel(playersOrdered[i].Connection, playersOrdered[0].Name);
            }
        }
    }
}