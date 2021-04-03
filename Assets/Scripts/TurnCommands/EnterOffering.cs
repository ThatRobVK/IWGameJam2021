using UnityEngine;
using FDaaGF.UI;

namespace FDaaGF
{
    public class EnterOffering : IGameCommand
    {
        public bool Completed { get; private set; }

        private GameState gameState;
        private OfferingPanel offeringPanel;

        public void Execute(GameState currentGameState)
        {
            Completed = false;

            gameState = currentGameState;

            offeringPanel.Show();
        }

        private void HandleOfferingConfirmed(int value)
        {
            Debug.LogFormat("Offering made: {0}", value);
            offeringPanel.Hide();
            Completed = true;
        }

        public EnterOffering(OfferingPanel panel)
        {
            this.offeringPanel = panel;
            offeringPanel.OnOfferingConfirmed += HandleOfferingConfirmed;
            offeringPanel.Hide();
        }
    }
}