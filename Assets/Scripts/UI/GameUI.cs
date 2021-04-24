using System;
using System.Collections.Generic;
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
        private Image[] playerImage;
        [SerializeField]
        private Image[] opponent0Images;
        [SerializeField]
        private Image[] opponent1Images;
        [SerializeField]
        private Image[] opponent2Images;
        [SerializeField]
        private GameState gameState;




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
                List<int> images = new List<int>();
                images.Add(gameState.Players.Where(x => x.ConnectionId == player.ConnectionId).First().Image);
                images.AddRange(gameState.Players.Where(x => x.ConnectionId != player.ConnectionId).Select(x => x.Image).ToArray());
                RpcSetPositions(player.Connection, player.Position, opponentPositions, images.ToArray());
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
        private void RpcSetPositions(NetworkConnection target, int playerPosition, int[] opponentPositions, int[] images)
        {
            ShowPosition(playerPosition, playerImage, images[0]);
            ShowPosition((opponentPositions.Length >= 1) ? opponentPositions[0] : -1, opponent0Images, (images.Length >= 2) ? images[1] : -1);
            ShowPosition((opponentPositions.Length >= 2) ? opponentPositions[1] : -1, opponent1Images, (images.Length >= 3) ? images[2] : -1);
            ShowPosition((opponentPositions.Length >= 3) ? opponentPositions[2] : -1, opponent2Images, (images.Length >= 4) ? images[3] : -1);
        }

        private void ShowPosition(int position, Image[] images, int imageIndex)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(i == position);
                if (imageIndex > -1 && images[i].gameObject.activeInHierarchy)
                {
                    images[i].sprite = gameState.PlayerImages[imageIndex];
                }
            }
        }
    }
}