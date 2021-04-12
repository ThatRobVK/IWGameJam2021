using System.Collections.Generic;
using Mirror;

namespace FDaaGF
{
    // Data class representing a player of the game
    public class Player
    {
        // Player name as entered on connection
        public string Name = string.Empty;
        public int ConnectionId = -1;
        public int CurrentOffer = -1;
        public int Position = 0;
        public NetworkConnectionToClient Connection;

        // Resource quantities owned by this player
        public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

        // Workers employed by the player
        public List<Worker> Workers = new List<Worker>();

        public Player(NetworkConnectionToClient connection, string name)
        {
            Connection = connection;
            ConnectionId = connection.connectionId;
            Name = name;

            Resources.Add(ResourceType.Gold, 5);
            Resources.Add(ResourceType.Wheat, 10);
            Resources.Add(ResourceType.Fish, 15);
            Resources.Add(ResourceType.Meat, 20);
        }
    }
}