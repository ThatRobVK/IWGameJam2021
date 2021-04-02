namespace FDaaGF
{
    // Represents an executable game step
    public interface IGameCommand
    {
        // Execute the game step based on the passed in game state
        void Execute(GameState currentGameState);

        // True once this has completed, false if still executing
        bool Completed { get; }
    }
}