using System.Collections.Generic;
using Mirror;
using FDaaGF.UI;
using FDaaGF.TurnCommands;
using UnityEngine;

namespace FDaaGF
{
    public class GameLoop : NetworkBehaviour
    {
        // Commands executed in order during player turns - once the last one has executed, a turn ends
        private List<IGameCommand> TurnCommands = new List<IGameCommand>();

        private int currentTurnCommand = -1;


        // Editor fields
        [SerializeField]
        private OfferingPanel offeringPanel;
        [SerializeField]
        private RoundWinnerPanel roundWinnerPanel;
        [SerializeField]
        private WorkerPlacementPanel workerPlacementPanel;
        [SerializeField]
        private GameState gameState;


        public override void OnStartServer()
        {
            base.OnStartServer();

            gameState.Turn = 1;

            // Add resource requirements at random
            for (int i = 0; i < 20; i++)
            {
                gameState.ResourceRequirements.Add((ResourceType)Random.Range(0, 3));
            }

            // Set up game loop commands
            TurnCommands.Add(new EnterOffering(offeringPanel));
            TurnCommands.Add(new DecideWinner(roundWinnerPanel));
            TurnCommands.Add(new GetResources());
            TurnCommands.Add(new WorkerPlacement(workerPlacementPanel));
            TurnCommands.Add(new IncrementTurn());
        }

        void Update()
        {
            // Control the commands only on the server
            if (!isServer) return;

            if (currentTurnCommand == -1 || TurnCommands[currentTurnCommand].Completed)
            {
                // If current command has completed, move to next command
                currentTurnCommand++;
                if (currentTurnCommand >= TurnCommands.Count)
                {
                    // If at end of list, return to start
                    currentTurnCommand = 0;
                }

                // Run next command
                ExecuteTurnCommand(currentTurnCommand);
            }
        }

        // Runs a command at the given index
        private void ExecuteTurnCommand(int index)
        {
            TurnCommands[index].Execute(gameState);
        }
    }
}