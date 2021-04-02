using System.Collections.Generic;
using UnityEngine;
using FDaaGF.TurnCommands;

namespace FDaaGF
{
    public class GameLoop : MonoBehaviour
    {
        // Commands executed in order during player turns - once the last one has executed, a turn ends
        private List<IGameCommand> TurnCommands = new List<IGameCommand>();

        private int currentTurnCommand = -1;
        private GameState gameState;

        void Start()
        {
            // Init state
            gameState = new GameState(new string[] { "Rob", "Mat", "BenC", "BenL" });

            TurnCommands.Add(new IncrementTurn());

            // Run first command
            currentTurnCommand = 0;
            ExecuteTurnCommand(currentTurnCommand);
        }

        void Update()
        {
            if (TurnCommands[currentTurnCommand].Completed)
            {
                // If completed, move to next command
                currentTurnCommand++;
                if (currentTurnCommand >= TurnCommands.Count)
                {
                    Debug.Log(gameState.Turn);
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