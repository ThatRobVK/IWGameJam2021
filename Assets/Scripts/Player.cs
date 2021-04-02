using System.Collections.Generic;

namespace FDaaGF
{
    // Data class representing a player of the game
    public class Player
    {
        // Player name as entered on connection
        public string Name { get; private set; }

        // Resource quantities owned by this player
        public Dictionary<ResourceType, int> Resources { get; private set; }

        // Workers employed by the player
        public List<Worker> Workers { get; private set; }

        public Player(string name)
        {
            Name = name;

            Resources = new Dictionary<ResourceType, int>();
            Resources.Add(ResourceType.Gold, 0);
            Resources.Add(ResourceType.Wheat, 0);
            Resources.Add(ResourceType.Fish, 0);
            Resources.Add(ResourceType.Meat, 0);

            Workers = new List<Worker>();
        }
    }
}