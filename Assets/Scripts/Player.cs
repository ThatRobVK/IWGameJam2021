using System.Collections.Generic;
using Mirror;

namespace FDaaGF
{
    // Data class representing a player of the game
    public class Player
    {
        // Player name as entered on connection
        public string Name = string.Empty;
        public bool WorkerPlacementUpdated = true;
        public int ConnectionId = -1;
        public int CurrentOffer = -1;
        public int Position = 0;
        public bool CurrentSacrifice = false;
        public NetworkConnectionToClient Connection;

        // Resource quantities owned by this player
        public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

        // Workers employed by the player
        public List<Worker> Workers = new List<Worker>();

        public int TotalOffer => CurrentOffer + ((CurrentSacrifice) ? 5 : 0);

        public Player(NetworkConnectionToClient connection, string name)
        {
            Connection = connection;
            ConnectionId = connection.connectionId;
            Name = name;

            // Starting resources
            Resources.Add(ResourceType.Gold, 10);
            Resources.Add(ResourceType.Wheat, 10);
            Resources.Add(ResourceType.Fish, 10);
            Resources.Add(ResourceType.Meat, 10);

            // Starting workers
            Workers.Add(new Worker { WorkPlacement = ResourceType.Gold });
            Workers.Add(new Worker { WorkPlacement = ResourceType.Wheat });
            Workers.Add(new Worker { WorkPlacement = ResourceType.Fish });
            Workers.Add(new Worker { WorkPlacement = ResourceType.Meat });
        }
    }
}