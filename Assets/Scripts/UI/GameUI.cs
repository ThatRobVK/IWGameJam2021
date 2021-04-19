using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class GameUI : NetworkBehaviour
    {
        [SerializeField]
        private Text playerName;
        [SerializeField]
        private Text[] opponentNames;
        [SerializeField]
        private ResourcesPanel[] resourcesPanels;


        public void UpdateUI(GameState gameState)
        {
            foreach (var player in gameState.Players)
            {
                var opponentNames = gameState.Players.Where(x => x.ConnectionId != player.ConnectionId).Select(x => x.Name).ToArray();
                RpcSetNames(player.Connection, player.Name, opponentNames);

                var resources = player.Resources.Select(x => x.Value).ToList();
                var opponents = gameState.Players.Where(x => x.ConnectionId != player.ConnectionId).ToList();
                foreach (var opponent in opponents)
                {
                    resources.AddRange(opponent.Resources.Select(x => x.Value).ToArray());
                }
                RpcSetResources(player.Connection, resources.ToArray());
            }
        }

        [TargetRpc]
        private void RpcSetNames(NetworkConnection target, string playerName, string[] opponentNames)
        {
            this.playerName.text = playerName;

            for (int i = 0; i < this.opponentNames.Length; i++)
            {
                if (i < opponentNames.Length)
                {
                    this.opponentNames[i].text = opponentNames[i];
                }
                else
                {
                    this.opponentNames[i].transform.parent.gameObject.SetActive(false);
                }
            }
        }

        [TargetRpc]
        private void RpcSetResources(NetworkConnection target, int[] resources)
        {
            for (int i = 0; i < resources.Length; i += 4)
            {
                resourcesPanels[i / 4].UpdateUI(new int[] { resources[i], resources[i + 1], resources[i + 2], resources[i + 3] });
            }

            if (resources.Length < 16)
            {
                for (int i = resources.Length + 3; i < 16; i += 4)
                {
                    resourcesPanels[i / 4].gameObject.SetActive(false);
                }
            }
        }
    }
}