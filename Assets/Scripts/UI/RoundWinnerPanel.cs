using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class RoundWinnerPanel : NetworkBehaviour
    {
        public event Action OnTimerCompleted;

        [SerializeField]
        private GameObject winnerPanel;
        [SerializeField]
        private Text winnerNameText;
        [SerializeField]
        private GameObject loserPanel;
        [SerializeField]
        private Text loserNameText;


        private CanvasGroup canvasGroup;


        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            ShowHideCanvasGroup(false);
        }


        public void Hide()
        {
            RpcHide();
        }

        [Server]
        public void ShowPanels(List<Player> playersOrdered)
        {
            // Poke fun at the losers
            for (int i = 1; i < playersOrdered.Count; i++)
            {
                Debug.LogFormat("Telling {0} to display the loser panel", playersOrdered[i].Name);
                RpcShowLoserPanel(playersOrdered[i].Connection, playersOrdered[0].Name);
            }

            // Show the winners
            // Congratulate the winner
            RpcShowWinnerPanel(playersOrdered[0].Connection, playersOrdered[0].Name);
        }

        [TargetRpc]
        public void RpcShowWinnerPanel(NetworkConnection target, string winnerName)
        {
            Debug.Log("Winner panel");
            ShowHideCanvasGroup(true);

            winnerPanel.SetActive(true);
            loserPanel.SetActive(false);
            winnerNameText.text = winnerName;

            // Wait for a few seconds and then tell the game to move on
            if (isServer) StartCoroutine(CompleteOnTimer());
        }

        [TargetRpc]
        public void RpcShowLoserPanel(NetworkConnection target, string winnerName)
        {
            Debug.Log("Loser panel");
            ShowHideCanvasGroup(true);

            winnerPanel.SetActive(false);
            loserPanel.SetActive(true);
            loserNameText.text = winnerName;

            // Wait for a few seconds and then tell the game to move on
            if (isServer) StartCoroutine(CompleteOnTimer());
        }

        [ClientRpc]
        public void RpcHide()
        {
            Debug.Log("Hiding");
            ShowHideCanvasGroup(false);
        }

        IEnumerator CompleteOnTimer()
        {
            yield return new WaitForSeconds(5);

            OnTimerCompleted?.Invoke();
        }

        private void ShowHideCanvasGroup(bool show)
        {
            canvasGroup.alpha = (show) ? 1 : 0;
            canvasGroup.blocksRaycasts = show;
            canvasGroup.interactable = show;
        }

    }
}