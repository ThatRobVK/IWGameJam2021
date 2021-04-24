using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace FDaaGF.UI.Room
{
    public class PlayersPanel : NetworkBehaviour
    {


        [SerializeField]
        private GameState gameState;
        [SerializeField]
        private PlayerPanel playerPanelPrefab;

        private List<PlayerPanel> playerPanels = new List<PlayerPanel>();

        void Update()
        {
            if (!isServer) return;

            var isDirty = false;

            if (playerPanels.Count != gameState.Players.Count)
            {
                // Players added or removed
                isDirty = true;
            }
            else
            {
                for (int i = 0; i < gameState.Players.Count; i++)
                {
                    if (playerPanels[i].Name != gameState.Players[i].Name ||
                        playerPanels[i].Image != gameState.PlayerImages[gameState.Players[i].Image])
                    {
                        // Details have changed
                        isDirty = true;
                        break;
                    }
                }
            }

            if (isDirty)
            {
                RpcUpdatePlayers(gameState.Players.Select(x => x.Name).ToArray(), gameState.Players.Select(x => x.Image).ToArray(), gameState.Players.Select(x => x.IsReady).ToArray());
            }
        }

        [ClientRpc]
        private void RpcUpdatePlayers(string[] playerNames, int[] images, bool[] readyStates)
        {
            for (int i = 0; i < playerNames.Length; i++)
            {
                if (i > playerPanels.Count - 1)
                {
                    var newPanel = Instantiate<PlayerPanel>(playerPanelPrefab, transform);
                    playerPanels.Add(newPanel);
                }

                playerPanels[i].SetPlayerDetails(playerNames[i], gameState.PlayerImages[images[i]], readyStates[i]);
            }
        }
    }
}