using UnityEngine;

namespace FDaaGF.TurnCommands
{
    public class IncrementTurn : IGameCommand
    {
        // True when this command has completed and the game loop can continue to the next command
        public bool Completed { get; private set; }


        // Starts the command
        public void Execute(GameState currentGameState)
        {
            // Increment the turn and flag as completed
            currentGameState.Turn++;
            Completed = true;

            Debug.LogFormat("Starting turn {0}", currentGameState.Turn);
        }
    }
}