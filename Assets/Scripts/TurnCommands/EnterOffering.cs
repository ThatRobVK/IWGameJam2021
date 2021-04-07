using System.Linq;
using UnityEngine;
using FDaaGF.UI;
using Mirror;

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
            offeringPanel.RpcHideAll();
        }


        // Starts the command
        public void Execute(GameState currentGameState)
        {
            // Not completed until user gives input
            Completed = false;

            gameState = currentGameState;

            var currentResource = currentGameState.ResourceRequirements[currentGameState.Turn - 1];

            // Reset all players and tell them how much of the current resource they have
            foreach (var player in gameState.Players)
            {
                player.CurrentOffer = -1;

                var resourceAmount = player.Resources[currentResource];
                offeringPanel.RpcShow(player.Connection, currentResource, resourceAmount);
            }
        }

        // Called on offeringPanel.OnOfferingConfirmed
        private void HandleOfferingConfirmed(NetworkConnectionToClient client, int value)
        {
            gameState.Players.Where(x => x.ConnectionId == client.connectionId).First().CurrentOffer = value;
            offeringPanel.RpcHide(client);


            // If no players left with no offer made, process everything
            if (gameState.Players.Where(x => x.CurrentOffer == -1).Count() == 0)
            {
                // Decrease everyone's resource amount
                foreach (var player in gameState.Players)
                {
                    player.Resources[gameState.ResourceRequirements[gameState.Turn - 1]] -= player.CurrentOffer;
                }

                // Tell all clients to hide the offer panel
                offeringPanel.RpcHideAll();

                // Flag we're completed for the game loop
                Completed = true;
            }
        }
    }
}