using System.Linq;
using UnityEngine;
using FDaaGF.UI;

namespace FDaaGF.TurnCommands
{
    public class OverallWinner : IGameCommand
    {
        public bool Completed { get; private set; }

        private GameWinnerPanel gameWinnerPanel;
        private GameState gameState;

        public OverallWinner(GameWinnerPanel winnerPanel)
        {
            gameWinnerPanel = winnerPanel;
        }

        public void Execute(GameState currentGameState)
        {
            Completed = false;

            var winners = currentGameState.Players.Where(x => x.Position == currentGameState.WinPosition).ToList();
            if (winners.Count > 0)
            {
                gameState = currentGameState;
                // Multiple winners, the one with the most sacrifices wins
                var winner = winners.OrderByDescending(x => x.TotalSacrifices).First();
                gameWinnerPanel.RpcShow(winner.Image, WinnerText(winner.Name), SacrificesImage(), SacrificesText(), GoldImage(), GoldText(), GrainImage(), GrainText(), MeatImage(), MeatText(), FishImage(), FishText());
            }
            else
            {
                // No winner, complete this command
                Completed = true;
            }
        }

        private string WinnerText(string winnerName)
        {
            return string.Format("<b>{0} is Victorious!</b> Their village Priest ascended the temple, and was blessed by the Gods. Their harvests were plentiful, and for some weird reason the Europeans never found their village.", winnerName);
        }

        private int SacrificesImage()
        {
            var player = gameState.Players.OrderByDescending(x => x.TotalSacrifices).First();
            return (player.TotalSacrifices > 0) ? player.Image : -1;
        }

        private string SacrificesText()
        {
            var player = gameState.Players.OrderByDescending(x => x.TotalSacrifices).First();
            return string.Format("The ruthless <b>{0}</b> sacrificed <b>{1}</b> people. Soon, their village Priest turned on them and <b>{0}</b> became the next sacrifice to be made.", player.Name, player.TotalSacrifices);
        }

        private int GrainImage()
        {
            return gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Wheat]).First().Image;
        }

        private string GrainText()
        {
            var player = gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Wheat]).First();
            return string.Format("<b>{0}</b> gathered a hoard of grain, feeding their village for years to come.", player.Name);
        }

        private int GoldImage()
        {
            return gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Gold]).First().Image;
        }

        private string GoldText()
        {
            var player = gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Gold]).First();
            return string.Format("<b>{0}</b> had streets paved with gold. Unfortunately, the Spaniards loved gold, and they soon relieved the village of it. And their heads.", player.Name);
        }

        private int MeatImage()
        {
            return gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Meat]).First().Image;
        }

        private string MeatText()
        {
            var player = gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Meat]).First();
            return string.Format("<b>{0}</b> built a meat empire. Their villagers dined like kings. Many cows and pigs regretted coming to their village.", player.Name);
        }

        private int FishImage()
        {
            return gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Fish]).First().Image;
        }

        private string FishText()
        {
            var player = gameState.Players.OrderByDescending(x => x.Resources[ResourceType.Fish]).First();
            return string.Format("<b>{0}</b> fed their villagers mostly fish, which is arguably a healthier diet than meat. The smell however, is a different story.", player.Name);
        }
    }
}