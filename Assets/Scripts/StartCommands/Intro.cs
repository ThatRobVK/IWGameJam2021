using FDaaGF.UI;

namespace FDaaGF.StartCommands
{
    public class Intro : IGameCommand
    {
        public bool Completed { get; private set; }

        private GameState gameState;
        private IntroPanel introPanel;
        private int introPanelsClosed;

        public Intro(IntroPanel panel)
        {
            introPanel = panel;
            introPanel.OnIntroClosed += HandleIntroClosed;
        }

        private void HandleIntroClosed()
        {
            introPanelsClosed++;
            if (introPanelsClosed == gameState.Players.Count)
            {
                // If all panels have been closed, set complete
                Completed = true;
            }
        }

        public void Execute(GameState currentGameState)
        {
            Completed = false;
            introPanelsClosed = 0;
            gameState = currentGameState;

            introPanel.RpcShow();
        }
    }
}