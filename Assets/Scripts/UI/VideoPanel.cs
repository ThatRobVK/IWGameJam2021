using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Video;

namespace FDaaGF.UI
{
    public class VideoPanel : NetworkBehaviour
    {

        public event Action OnVideoCompleted;

        [SerializeField]
        private AudioSource gameMusic;


        [ClientRpc]
        public void RpcShow()
        {
            GetComponent<CanvasGroup>().alpha = 1;
            GetComponent<CanvasGroup>().interactable = true;
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            GetComponentInChildren<VideoPlayer>().Play();
            // Wait for a few seconds and then tell the game to move on
            StartCoroutine(StartMusicOnTimer());
            if (isServer) StartCoroutine(CompleteOnTimer());
        }


        IEnumerator StartMusicOnTimer()
        {
            yield return new WaitForSeconds(57);

            gameMusic.Play();
        }

        IEnumerator CompleteOnTimer()
        {
            yield return new WaitForSeconds(58.5f);

            OnVideoCompleted?.Invoke();
        }

        [ClientRpc]
        public void RpcHide()
        {
            GetComponentInChildren<VideoPlayer>().Stop();

            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().interactable = false;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }
}