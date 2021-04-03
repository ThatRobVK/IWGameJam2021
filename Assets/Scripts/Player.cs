using System.Collections.Generic;
using UnityEngine;

namespace FDaaGF
{
    // Data class representing a player of the game
    public class Player
    {
        // Player name as entered on connection
        public string Name = string.Empty;
        public int ConnectionId = -1;
        public int CurrentOffer = -1;

        // Resource quantities owned by this player
        public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

        // Workers employed by the player
        public List<Worker> Workers = new List<Worker>();

        public Player(int connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;

            Resources.Add(ResourceType.Gold, 0);
            Resources.Add(ResourceType.Wheat, 0);
            Resources.Add(ResourceType.Fish, 0);
            Resources.Add(ResourceType.Meat, 0);
        }
    }
}