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


        void Start()
        {
            // Hide on load
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            RpcHide();
        }

        [TargetRpc]
        public void RpcShowWinnerPanel(NetworkConnection target, string winnerName)
        {
            Debug.Log("Winner panel");
            gameObject.SetActive(true);

            winnerPanel.SetActive(true);
            loserPanel.SetActive(false);
            winnerNameText.text = winnerName;

            // Wait for a few seconds and then tell the game to move on
            StartCoroutine(CompleteOnTimer());
        }

        [TargetRpc]
        public void RpcShowLoserPanel(NetworkConnection target, string winnerName)
        {
            Debug.Log("Loser panel");
            gameObject.SetActive(true);

            winnerPanel.SetActive(false);
            loserPanel.SetActive(true);
            loserNameText.text = winnerName;
        }

        [ClientRpc]
        private void RpcHide()
        {
            gameObject.SetActive(false);
        }

        IEnumerator CompleteOnTimer()
        {
            yield return new WaitForSeconds(5);

            OnTimerCompleted?.Invoke();
        }
    }
}