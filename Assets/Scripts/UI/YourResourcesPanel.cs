using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI.Room
{
    public class YourResourcesPanel : NetworkBehaviour
    {
        [SerializeField]
        private GameState gameState;
        [SerializeField]
        private Text[] ResourceValueText;
        [SerializeField]
        private Text[] ResourceIncreaseText;

        private int currentTurn = -1;


        void Start()
        {
            // Working around a weird bug where on different size windows the panels disappear
            GetComponent<RectTransform>().anchoredPosition = new Vector2(70, 0);
        }

        void Update()
        {
            if (isServer && gameState.Turn != currentTurn)
            {
                // On a new turn, on the server loop through all players
                foreach (var player in gameState.Players)
                {
                    // Call the client RPC with the player's resource values
                    var resourceValues = new int[player.Resources.Count];
                    var resourceIncreases = new int[player.Resources.Count];

                    for (int i = 0; i < player.Resources.Count; i++)
                    {
                        resourceValues[i] = player.Resources[(ResourceType)i];
                        resourceIncreases[i] = player.Workers.Where(x => x.WorkPlacement == (ResourceType)i).Count();
                    }

                    RpcUpdateUI(player.Connection, resourceValues, resourceIncreases);
                }

                currentTurn = gameState.Turn;
            }
        }

        [TargetRpc]
        private void RpcUpdateUI(NetworkConnection target, int[] resourceValues, int[] resourceIncreases)
        {
            // Update the UI for all resources passed in
            for (int i = 0; i < resourceValues.Length; i++)
            {
                ResourceValueText[i].text = resourceValues[i].ToString();
                ResourceIncreaseText[i].text = resourceIncreases[i].ToString();
            }
        }
    }
}