using System.Collections.Generic;
using Mirror;
using FDaaGF.UI;
using FDaaGF.TurnCommands;

namespace FDaaGF
{
    public class GameLoop : NetworkBehaviour
    {
        // Commands executed in order during player turns - once the last one has executed, a turn ends
        private List<IGameCommand> TurnCommands = new List<IGameCommand>();

        private int currentTurnCommand = -1;
        private GameState gameState;


        // Editor fields
        public OfferingPanel OfferingPanel;


        void Start()
        {
            // Init state
            // TODO: Get player names from networked players
            gameState = new GameState(new string[] { "Rob", "Mat", "BenC", "BenL" });

            // Set up game loop commands
            TurnCommands.Add(new IncrementTurn());
            TurnCommands.Add(new EnterOffering(OfferingPanel));

            // Run first command
            currentTurnCommand = 0;
            ExecuteTurnCommand(currentTurnCommand);
        }

        void Update()
        {
            // Control the commands only on the server
            if (!isServer) return;

            if (TurnCommands[currentTurnCommand].Completed)
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
            if (TurnCommands.Count > index)
            {
                TurnCommands[index].Execute(gameState);
            }
        }
    }
}