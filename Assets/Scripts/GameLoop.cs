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
        private GameWinnerPanel gameWinnerPanel;
        [SerializeField]
        private GameState gameState;
        [SerializeField]
        private GameUI gameUI;


        public override void OnStartServer()
        {
            base.OnStartServer();

            gameState.Turn = 1;

            var maxRounds = gameState.Players.Count * gameState.WinPosition;
            Debug.Log(maxRounds);

            // Add resource requirements at random
            int lastNumber = -1;
            for (int i = 0; i < maxRounds; i++)
            {
                // Pick a random number, remove duplicates
                var newNumber = Random.Range(0, 4);
                if (newNumber == lastNumber) newNumber++;
                if (newNumber > 3) newNumber = 0;

                gameState.ResourceRequirements.Add((ResourceType)newNumber);
                lastNumber = newNumber;
            }

            var requirements = "Resource Requirements:\n";
            foreach (var requirement in gameState.ResourceRequirements)
            {
                requirements = string.Format("{0}{1}\n", requirements, requirement.ToString());
            }
            Debug.Log(requirements);

            // Set up game loop commands
            TurnCommands.Add(new WorkerPlacement(workerPlacementPanel));
            TurnCommands.Add(new EnterOffering(offeringPanel));
            TurnCommands.Add(new DecideWinner(roundWinnerPanel));
            TurnCommands.Add(new OverallWinner(gameWinnerPanel));
            TurnCommands.Add(new GetResources());
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

                // Tell the UI to update
                gameUI.UpdateUI(gameState);

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