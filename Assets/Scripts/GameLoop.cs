using System.Collections.Generic;
using Mirror;
using FDaaGF.UI;
using FDaaGF.StartCommands;
using FDaaGF.TurnCommands;
using UnityEngine;

namespace FDaaGF
{
    public class GameLoop : NetworkBehaviour
    {
        // Commands executed once at the start of the game
        private List<IGameCommand> StartGameCommands = new List<IGameCommand>();
        // Commands executed in order during player turns - once the last one has executed, a turn ends
        private List<IGameCommand> TurnCommands = new List<IGameCommand>();

        private GameLoopStage currentStage = GameLoopStage.Start;
        private int currentStartCommand = -1;
        private int currentTurnCommand = -1;


        // Editor fields
        [SerializeField]
        private IntroPanel introPanel;
        [SerializeField]
        private OfferingPanel offeringPanel;
        [SerializeField]
        private RoundWinnerPanel roundWinnerPanel;
        [SerializeField]
        private WorkerPlacementPanel workerPlacementPanel;
        [SerializeField]
        private GameWinnerPanel gameWinnerPanel;
        [SerializeField]
        private GameState gameState;
        [SerializeField]
        private GameUI gameUI;


        public override void OnStartServer()
        {
            base.OnStartServer();

            gameState.Turn = 1;

            // Add resource requirements at random
            var maxRounds = gameState.Players.Count * gameState.WinPosition;
            int lastNumber = -1;
            for (int i = 0; i < maxRounds; i++)
            {
                // Pick a random number, remove duplicates
                var newNumber = Random.Range(0, 4);
                if (newNumber == lastNumber) newNumber++;
                if (newNumber > 3) newNumber = 0;

                gameState.ResourceRequirements.Add((ResourceType)newNumber);
                lastNumber = newNumber;
            }


            // Set up the game start commands
            StartGameCommands.Add(new Intro(introPanel));

            // Set up game loop commands
            TurnCommands.Add(new WorkerPlacement(workerPlacementPanel));
            TurnCommands.Add(new EnterOffering(offeringPanel));
            TurnCommands.Add(new DecideWinner(roundWinnerPanel));
            TurnCommands.Add(new OverallWinner(gameWinnerPanel));
            TurnCommands.Add(new GetResources());
            TurnCommands.Add(new IncrementTurn());
        }

        void Update()
        {
            // Control the commands only on the server
            if (!isServer) return;

            if (currentStage == GameLoopStage.Start)
            {
                if (currentStartCommand == -1)
                {
                    // On first update of game, update UI as you can see the default values behind the intro etc.
                    gameUI.UpdateUI(gameState);
                }

                if (CommandCompleted(StartGameCommands, currentStartCommand))
                {
                    // Move to next command
                    currentStartCommand++;
                    if (currentStartCommand < StartGameCommands.Count)
                    {
                        // Still have start commands to go, execute next one
                        StartGameCommands[currentStartCommand].Execute(gameState);
                    }
                    else
                    {
                        // Move to turn commands
                        currentStage = GameLoopStage.Turn;
                    }
                }
            }
            else if (currentStage == GameLoopStage.Turn)
            {
                if (CommandCompleted(TurnCommands, currentTurnCommand))
                {
                    // If current command has completed, move to next command
                    currentTurnCommand++;
                    if (currentTurnCommand >= TurnCommands.Count)
                    {
                        // If at end of list, return to start
                        currentTurnCommand = 0;
                    }

                    // Tell the UI to update
                    gameUI.UpdateUI(gameState);

                    // Run next command
                    TurnCommands[currentTurnCommand].Execute(gameState);
                }
            }
        }

        private bool CommandCompleted(List<IGameCommand> commandList, int index)
        {
            return index == -1 || commandList[index].Completed;
        }
    }

    public enum GameLoopStage
    {
        Start,
        Turn,
        End
    }
}