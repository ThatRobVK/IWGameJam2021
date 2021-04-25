using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class GameWinnerPanel : NetworkBehaviour
    {
        private CanvasGroup canvasGroup;

        [SerializeField]
        private GameState gameState;
        [SerializeField]
        private Image winnerImage;
        [SerializeField]
        private Text winnerText;
        [SerializeField]
        private Image sacrificeImage;
        [SerializeField]
        private Text sacrificeText;
        [SerializeField]
        private Image goldImage;
        [SerializeField]
        private Text goldText;
        [SerializeField]
        private Image wheatImage;
        [SerializeField]
        private Text wheatText;
        [SerializeField]
        private Image meatImage;
        [SerializeField]
        private Text meatText;
        [SerializeField]
        private Image fishImage;
        [SerializeField]
        private Text fishText;



        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            ShowHideCanvasGroup(false);
        }


        [ClientRpc]
        public void RpcShow(int winnerImage, string winnerText, int sacrificesImage, string sacrificesText, int goldImage, string goldText, int wheatImage, string wheatText, int meatImage, string meatText, int fishImage, string fishText)
        {
            this.winnerImage.sprite = gameState.PlayerImages[winnerImage];
            this.winnerText.text = winnerText;

            if (sacrificesImage > -1)
            {
                this.sacrificeImage.sprite = gameState.PlayerImages[sacrificesImage];
                this.sacrificeText.text = sacrificesText;
            }
            else
            {
                this.sacrificeImage.gameObject.SetActive(false);
                this.sacrificeText.gameObject.SetActive(false);
            }

            this.goldImage.sprite = gameState.PlayerImages[goldImage];
            this.goldText.text = goldText;
            this.wheatImage.sprite = gameState.PlayerImages[wheatImage];
            this.wheatText.text = wheatText;
            this.meatImage.sprite = gameState.PlayerImages[meatImage];
            this.meatText.text = meatText;
            this.fishImage.sprite = gameState.PlayerImages[fishImage];
            this.fishText.text = fishText;

            ShowHideCanvasGroup(true);
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