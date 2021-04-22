using System;
using System.Linq;
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
        private Text winnerUnnecessarySacrificeText;
        [SerializeField]
        private Text winnerNoSacrificeText;
        [SerializeField]
        private Text winnerNecessarySacrificeText;
        [SerializeField]
        private GameObject loserPanel;
        [SerializeField]
        private Image loserImage;
        [SerializeField]
        private Text loserNameText;
        [SerializeField]
        private Text loserUnnecessarySacrificeText;
        [SerializeField]
        private Text loserNoSacriciceText;
        [SerializeField]
        private GameState gameState;


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
        public void ShowPanels(List<Player> winners, List<Player> losers)
        {
            var highestLosingOffering = 0;
            var winnerNames = string.Join(" and ", winners.Select(x => x.Name).ToArray());
            var winnerImage = winners[0].Image;

            // Poke fun at the losers
            foreach (var loser in losers)
            {
                RpcShowLoserPanel(loser.Connection, winnerNames, winnerImage, loser.CurrentSacrifice);
                highestLosingOffering = Math.Max(loser.CurrentOffer, highestLosingOffering);
            }

            // Show the winners
            // Congratulate the winner
            foreach (var winner in winners)
            {
                RpcShowWinnerPanel(winner.Connection, winner.CurrentOffer - highestLosingOffering, winner.CurrentSacrifice);
            }
        }

        [TargetRpc]
        public void RpcShowWinnerPanel(NetworkConnection target, int offeringDifference, bool madeSacrifice)
        {
            ShowHideCanvasGroup(true);

            winnerPanel.SetActive(true);
            loserPanel.SetActive(false);

            if (offeringDifference > 5 && madeSacrifice)
            {
                // Unnecessary sacrifice
                winnerUnnecessarySacrificeText.gameObject.SetActive(true);
                winnerNecessarySacrificeText.gameObject.SetActive(false);
                winnerNoSacrificeText.gameObject.SetActive(false);
            }
            else if (offeringDifference <= 5 && madeSacrifice)
            {
                // Sacrifice but necessary
                winnerNecessarySacrificeText.gameObject.SetActive(true);
                winnerUnnecessarySacrificeText.gameObject.SetActive(false);
                winnerNoSacrificeText.gameObject.SetActive(false);
            }
            else
            {
                // No sacrifice
                winnerNoSacrificeText.gameObject.SetActive(true);
                winnerUnnecessarySacrificeText.gameObject.SetActive(false);
                winnerNecessarySacrificeText.gameObject.SetActive(false);
            }

            // Wait for a few seconds and then tell the game to move on
            if (isServer) StartCoroutine(CompleteOnTimer());
        }

        [TargetRpc]
        public void RpcShowLoserPanel(NetworkConnection target, string winnerName, int winnerImage, bool madeSacrifice)
        {
            ShowHideCanvasGroup(true);

            winnerPanel.SetActive(false);
            loserPanel.SetActive(true);
            loserNameText.text = winnerName;
            loserImage.sprite = gameState.PlayerImages[winnerImage];

            // Show flavour text about sacrifice
            loserUnnecessarySacrificeText.gameObject.SetActive(madeSacrifice);
            loserNoSacriciceText.gameObject.SetActive(!madeSacrifice);

            // Wait for a few seconds and then tell the game to move on
            if (isServer) StartCoroutine(CompleteOnTimer());
        }

        [ClientRpc]
        public void RpcHide()
        {
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