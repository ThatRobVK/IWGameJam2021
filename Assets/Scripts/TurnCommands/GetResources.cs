using UnityEngine;

namespace FDaaGF.TurnCommands
{
    public class GetResources : IGameCommand
    {

        public bool Completed { get; private set; }


        // Runs the command
        public void Execute(GameState currentGameState)
        {
            Completed = false;

            // Increment resources for each worker
            foreach (var player in currentGameState.Players)
            {
                Debug.Log("Incrementing for player " + player.Name);
                foreach (var worker in player.Workers)
                {
                    Debug.LogFormat("Incrementing {0} - current value {1}", worker.WorkPlacement.ToString(), player.Resources[worker.WorkPlacement]);
                    player.Resources[worker.WorkPlacement]++;
                    Debug.LogFormat("New value: {0}", player.Resources[worker.WorkPlacement]);

                }
            }

            Completed = true;
        }
    }
}