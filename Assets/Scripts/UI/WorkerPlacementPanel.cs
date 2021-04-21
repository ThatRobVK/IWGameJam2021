using System;
using System.Linq;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class WorkerPlacementPanel : NetworkBehaviour
    {
        // Event definitions
        public event Action<NetworkConnectionToClient, List<Worker>> OnPlacementsConfirmed;


        // Editor fields
        [SerializeField]
        private GameObject InputPanel;
        [SerializeField]
        private GameObject WaitPanel;
        [SerializeField]
        private Text[] resourceText;
        [SerializeField]
        private Text recruitingText;
        [SerializeField]
        private Text freeWorkersText;
        [SerializeField]
        private Text tooManySacrificesText;


        private int maxRecruiters = 1;
        private int availableWorkerCount;
        private CanvasGroup canvasGroup;
        private List<Worker> workers;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            ShowHideCanvasGroup(false);
        }

        // Called when the Confirm button is clicked - runs on the client the event happens on
        public void HandleConfirmButtonClick()
        {
            if (workers.Count == availableWorkerCount)
            {
                CmdRaisePlacementsConfirmed(workers);
            }
        }

        // Clears and hides the panel - called on all clients
        [TargetRpc]
        public void RpcHide(NetworkConnection target)
        {
            InputPanel.SetActive(false);
            WaitPanel.SetActive(true);
        }

        [ClientRpc]
        public void RpcHideAll()
        {
            InputPanel.SetActive(true);
            WaitPanel.SetActive(false);

            ShowHideCanvasGroup(false);
        }

        // Shows the panel - called on all clients
        [TargetRpc]
        public void RpcShow(NetworkConnection target, List<Worker> playerWorkers, bool canRecruit)
        {
            ShowHideCanvasGroup(true);

            InputPanel.SetActive(true);
            WaitPanel.SetActive(false);

            workers = playerWorkers;
            availableWorkerCount = playerWorkers.Count();

            if (!canRecruit)
            {
                // Player can no longer recruit, so remove all recruiters
                playerWorkers.ForEach(x => x.Recruiting = false);
                maxRecruiters = 0;
                tooManySacrificesText.gameObject.SetActive(true);
            }

            ShowWorkerCounts();
        }

        // Notify listeners that an offering has been sent - called on the server, can be called by any client
        [Command(requiresAuthority = false)]
        private void CmdRaisePlacementsConfirmed(List<Worker> newWorkers, NetworkConnectionToClient sender = null)
        {
            OnPlacementsConfirmed?.Invoke(sender, newWorkers);
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

        public void Increment(int resourceIndex)
        {
            if (workers.Count < availableWorkerCount)
            {
                if (resourceIndex <= 3)
                {
                    // Add a worker of the specified resource
                    workers.Add(new Worker { WorkPlacement = (ResourceType)resourceIndex });
                }
                else if (resourceIndex == 4 && workers.Where(x => x.Recruiting).Count() < maxRecruiters)
                {
                    // If recruitment and not at max recruiters, add a recruiter
                    workers.Add(new Worker { WorkPlacement = ResourceType.Gold, Recruiting = true });
                }
            }

            ShowWorkerCounts();
        }

        public void Decrement(int resourceIndex)
        {
            if (resourceIndex <= 3 && workers.Where(x => x.WorkPlacement == (ResourceType)resourceIndex && !x.Recruiting).Count() > 0)
            {
                // Remove a worker of the given resource type
                var worker = workers.Where(x => x.WorkPlacement == (ResourceType)resourceIndex && !x.Recruiting).First();
                workers.Remove(worker);
            }
            else if (resourceIndex == 4 && workers.Where(x => x.Recruiting).Count() > 0)
            {
                // Remove a recruiting worker
                var worker = workers.Where(x => x.Recruiting).First();
                workers.Remove(worker);
            }

            ShowWorkerCounts();
        }

        private void ShowWorkerCounts()
        {
            resourceText[(int)ResourceType.Gold].text = workers.Where(x => x.WorkPlacement == ResourceType.Gold && !x.Recruiting).Count().ToString();
            resourceText[(int)ResourceType.Wheat].text = workers.Where(x => x.WorkPlacement == ResourceType.Wheat && !x.Recruiting).Count().ToString();
            resourceText[(int)ResourceType.Fish].text = workers.Where(x => x.WorkPlacement == ResourceType.Fish && !x.Recruiting).Count().ToString();
            resourceText[(int)ResourceType.Meat].text = workers.Where(x => x.WorkPlacement == ResourceType.Meat && !x.Recruiting).Count().ToString();
            recruitingText.text = workers.Where(x => x.Recruiting).Count().ToString();
            freeWorkersText.text = (availableWorkerCount - workers.Count).ToString();
        }
    }
}