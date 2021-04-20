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
        [SerializeField]
        private Image playerImage;
        [SerializeField]
        private Image[] opponentImages;


        private Vector2[] playerPositionVectors = new Vector2[] { new Vector2(0, -120), new Vector2(0, -100), new Vector2(0, -80), new Vector2(0, -60), new Vector2(0, -40), new Vector2(0, -20), new Vector2(0, 0) };
        private Vector2[][] opponentPositionVectors = new Vector2[][] {
             new Vector2[] { new Vector2(0, 120), new Vector2(0, 100), new Vector2(0, 80), new Vector2(0, 60), new Vector2(0, 40), new Vector2(0, 20), new Vector2(0,0) },
             new Vector2[] { new Vector2(-120, 0), new Vector2(-100, 0), new Vector2(-80, 0), new Vector2(-60, 0), new Vector2(-40, 0), new Vector2(-20, 0), new Vector2(0,0) },
             new Vector2[] { new Vector2(120, 0), new Vector2(100, 0), new Vector2(80, 0), new Vector2(60, 0), new Vector2(40, 0), new Vector2(20, 0), new Vector2(0,0) }
        };



        public void UpdateUI(GameState gameState)
        {
            foreach (var player in gameState.Players)
            {
                // Set player and opponent names in UI
                var opponentNames = gameState.Players.Where(x => x.ConnectionId != player.ConnectionId).Select(x => x.Name).ToArray();
                RpcSetNames(player.Connection, player.Name, opponentNames);

                // Set resource counts
                var resources = player.Resources.Select(x => x.Value).ToList();
                var opponents = gameState.Players.Where(x => x.ConnectionId != player.ConnectionId).ToList();
                foreach (var opponent in opponents)
                {
                    resources.AddRange(opponent.Resources.Select(x => x.Value).ToArray());
                }
                RpcSetResources(player.Connection, resources.ToArray());

                // Set player positions
                var opponentPositions = gameState.Players.Where(x => x.ConnectionId != player.ConnectionId).Select(x => x.Position).ToArray();
                RpcSetPositions(player.Connection, player.Position, opponentPositions);
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

        [TargetRpc]
        private void RpcSetPositions(NetworkConnection target, int playerPosition, int[] opponentPositions)
        {
            playerImage.GetComponent<RectTransform>().anchoredPosition = playerPositionVectors[playerPosition];

            for (int i = 0; i < 3; i++)
            {
                if (i < opponentPositions.Length)
                {
                    opponentImages[i].GetComponent<RectTransform>().anchoredPosition = opponentPositionVectors[i][opponentPositions[i]];
                }
                else
                {
                    opponentImages[i].gameObject.SetActive(false);
                }
            }
        }
    }
}