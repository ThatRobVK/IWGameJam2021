using System;
using Mirror;
using UnityEngine;

namespace FDaaGF.UI
{
    public class IntroPanel : NetworkBehaviour
    {

        public event Action OnIntroClosed;


        [ClientRpc]
        public void RpcShow()
        {
            GetComponent<CanvasGroup>().alpha = 1;
            GetComponent<CanvasGroup>().interactable = true;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void HandleCloseButtonClicked()
        {
            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().interactable = false;
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            // Tell server that the panel has been closed
            CmdClosePanel();
        }

        [Command(requiresAuthority = false)]
        private void CmdClosePanel()
        {
            OnIntroClosed?.Invoke();
        }
    }
}