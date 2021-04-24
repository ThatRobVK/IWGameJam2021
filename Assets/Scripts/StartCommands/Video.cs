using FDaaGF.UI;

namespace FDaaGF.StartCommands
{
    public class Video : IGameCommand
    {
        public bool Completed { get; private set; }

        private VideoPanel videoPanel;

        public Video(VideoPanel panel)
        {
            videoPanel = panel;
            videoPanel.OnVideoCompleted += CompleteCommand;
        }

        public void Execute(GameState currentGameState)
        {
            Completed = false;

            videoPanel.RpcShow();
        }

        // Hides the panel and flags the command as completed
        private void CompleteCommand()
        {
            videoPanel.RpcHide();
            Completed = true;
        }
    }
}