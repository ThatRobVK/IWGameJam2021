using System.Collections.Generic;
using UnityEngine;

namespace FDaaGF
{
    // Data class representing the overall state of the current game
    [CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/GameState", order = 1)]
    public class GameState : ScriptableObject
    {
        // The current game turn
        public int Turn = 0;

        // The resources required for each turn        
        public List<ResourceType> ResourceRequirements = new List<ResourceType>();

        // All players in the current game
        public List<Player> Players = new List<Player>();
    }
}