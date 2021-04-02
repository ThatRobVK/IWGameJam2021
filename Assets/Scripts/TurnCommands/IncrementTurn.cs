namespace FDaaGF.TurnCommands
{
    public class IncrementTurn : IGameCommand
    {
        public bool Completed { get; private set; }

        public void Execute(GameState currentGameState)
        {
            currentGameState.Turn++;
            Completed = true;
        }
    }
}