using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class GameWinnerPanel : NetworkBehaviour
    {
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Text winnerNameText;


        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            ShowHideCanvasGroup(false);
        }

        // Shows the panel - called on all clients
        [ClientRpc]
        public void RpcShow(string winnerName)
        {
            ShowHideCanvasGroup(true);

            winnerNameText.text = winnerName;
        }


        private void ShowHideCanvasGroup(bool show)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (show) ? 1 : 0;
                canvasGroup.blocksRaycasts = show;
                canvasGroup.interactable = show;
            }
        }
    }
}