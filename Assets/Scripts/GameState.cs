using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace FDaaGF
{
    // Data class representing the overall state of the current game
    [CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/GameState", order = 1)]
    public class GameState : ScriptableObject
    {
        public int WinPosition = 6;

        // The current game turn
        [SyncVar]
        public int Turn = 0;

        // The resources required for each turn        
        public SyncList<ResourceType> ResourceRequirements = new SyncList<ResourceType>();

        // All players in the current game
        public SyncList<Player> Players = new SyncList<Player>();
    }
}