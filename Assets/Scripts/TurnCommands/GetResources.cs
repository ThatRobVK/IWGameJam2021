using System.Linq;

namespace FDaaGF.TurnCommands
{
    public class GetResources : IGameCommand
    {

        public bool Completed { get; private set; }


        // Runs the command
        public void Execute(GameState currentGameState)
        {
            Completed = false;

            foreach (var player in currentGameState.Players)
            {
                int recruiting = 0;

                foreach (var worker in player.Workers)
                {
                    if (!worker.Recruiting)
                    {
                        // Increment resource for the worker
                        player.Resources[worker.WorkPlacement]++;
                    }
                    else
                    {
                        // Count recruiters
                        recruiting++;
                    }
                }

                for (int i = 0; i < recruiting; i++)
                {
                    // Increment workers if recruiting
                    player.Workers.Add(new Worker());
                }
            }

            Completed = true;
        }
    }
}