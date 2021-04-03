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

            // Reset all players' offers
            gameState.Players.ForEach(x => x.CurrentOffer = -1);

            // Show the panel and wait for user input
            offeringPanel.RpcShow(currentGameState.ResourceRequirements[currentGameState.Turn - 1]);
        }

        // Called on offeringPanel.OnOfferingConfirmed
        private void HandleOfferingConfirmed(NetworkConnectionToClient client, int value)
        {
            gameState.Players.Where(x => x.ConnectionId == client.connectionId).First().CurrentOffer = value;
            offeringPanel.RpcHide(client);


            if (gameState.Players.Where(x => x.CurrentOffer == -1).Count() == 0)
            {
                // If no players left with no offer made, hide panel and set completed.
                Completed = true;

                var offersText = "Offers made:\n";
                gameState.Players.ForEach(x => offersText = string.Format("{0}{1} offered {2} {3}\n", offersText, x.Name, x.CurrentOffer, gameState.ResourceRequirements[gameState.Turn - 1]));
                Debug.Log(offersText);

                offeringPanel.RpcHideAll();
            }
        }
    }
}