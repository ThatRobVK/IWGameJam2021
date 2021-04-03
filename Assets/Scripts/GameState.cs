using System.Collections.Generic;

namespace FDaaGF
{
    // Data class representing the overall state of the current game
    public class GameState
    {
        // The current game turn
        public int Turn { get; set; }

        // The resources required for each turn        
        public List<ResourceType> ResourceRequirements { get; private set; }

        // All players in the current game
        public List<Player> Players { get; private set; }

        public GameState(string[] playerNames)
        {
            ResourceRequirements = new List<ResourceType>();

            // Add all players to the list
            Players = new List<Player>();
            foreach (string playerName in playerNames)
            {
                Players.Add(new Player(playerName));
            }
        }
    }
}