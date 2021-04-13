using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FDaaGF.UI;
using Mirror;

namespace FDaaGF
{
    public class WorkerPlacement : IGameCommand
    {
        // True when this command has completed and the game loop can continue to the next command
        public bool Completed { get; private set; }


        private GameState gameState;
        private WorkerPlacementPanel workerPlacementPanel;

        // Constructor
        public WorkerPlacement(WorkerPlacementPanel panel)
        {
            // Hook up event and hide UI panel
            this.workerPlacementPanel = panel;
            workerPlacementPanel.OnPlacementsConfirmed += HandlePlacementsConfirmed;
            workerPlacementPanel.RpcHideAll();
        }


        // Starts the command
        public void Execute(GameState currentGameState)
        {
            // Not completed until user gives input
            Completed = false;

            gameState = currentGameState;

            // Reset all players and tell them how much of the current resource they have
            foreach (var player in gameState.Players)
            {
                player.WorkerPlacementUpdated = false;
                workerPlacementPanel.RpcShow(player.Connection, player.Workers);
            }
        }

        // Called on offeringPanel.OnOfferingConfirmed
        private void HandlePlacementsConfirmed(NetworkConnectionToClient client, List<Worker> workers)
        {
            gameState.Players.Where(x => x.ConnectionId == client.connectionId).First().Workers = workers;
            gameState.Players.Where(x => x.ConnectionId == client.connectionId).First().WorkerPlacementUpdated = true;
            workerPlacementPanel.RpcHide(client);

            // If no players left with no offer made, process everything
            if (gameState.Players.Where(x => !x.WorkerPlacementUpdated).Count() == 0)
            {
                // Tell all clients to hide the offer panel
                workerPlacementPanel.RpcHideAll();

                // Flag we're completed for the game loop
                Completed = true;
            }
        }
    }
}