using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class GameUI : NetworkBehaviour
    {
        [SerializeField]
        private Text PlayerName;
        [SerializeField]
        private Text[] OpponentNames;


        public void UpdateUI(GameState gameState)
        {
            foreach (var player in gameState.Players)
            {
                var opponentNames = gameState.Players.Where(x => x.ConnectionId != player.ConnectionId).Select(x => x.Name).ToArray();
                RpcSetNames(player.Connection, player.Name, opponentNames);
            }
        }

        [TargetRpc]
        private void RpcSetNames(NetworkConnection target, string playerName, string[] opponentNames)
        {
            PlayerName.text = playerName;

            for (int i = 0; i < OpponentNames.Length; i++)
            {
                if (i < opponentNames.Length)
                {
                    OpponentNames[i].text = opponentNames[i];
                }
                else
                {
                    OpponentNames[i].transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}