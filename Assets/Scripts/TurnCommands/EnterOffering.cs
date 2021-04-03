using UnityEngine;
using FDaaGF.UI;

namespace FDaaGF
{
    public class EnterOffering : IGameCommand
    {
        // True when this command has completed and the game loop can continue to the next command
        public bool Completed { get; private set; }


        private GameState gameState;
        private OfferingPanel offeringPanel;


        // Constructor
        public EnterOffering(OfferingPanel panel)
        {
            // Hook up event and hide UI panel
            this.offeringPanel = panel;
            offeringPanel.OnOfferingConfirmed += HandleOfferingConfirmed;
            offeringPanel.RpcHide();
        }


        // Starts the command
        public void Execute(GameState currentGameState)
        {
            // Not completed until user gives input
            Completed = false;

            gameState = currentGameState;

            // Show the panel and wait for user input
            offeringPanel.RpcShow(currentGameState.ResourceRequirements[currentGameState.Turn - 1]);
        }

        // Called on offeringPanel.OnOfferingConfirmed
        private void HandleOfferingConfirmed(int value)
        {
            // Hide the panel and flag this command has completed
            offeringPanel.RpcHide();
            Completed = true;
        }
    }
}