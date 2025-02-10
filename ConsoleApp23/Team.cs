using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    public class Team
    {
        public string Name { get; set; }
        public IList<Player> Players { get; set; }
        public List<TeamHistoryRecord> LogHistory { get; set; }

        public Team(string name)
        {
            Name = name;
            Players = new List<Player>();
            LogHistory = new List<TeamHistoryRecord>();
            LogHistory.Add(new TeamHistoryRecord(DateTime.Now, $"Отборът {name} е създаден."));
        }

        public void AddPlayer(Player player)
        {
            if (Players.Any(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Играч {player.Name} вече е добавен в отбора {Name}.");
            }
            else
            {
                Players.Add(player);
                Console.WriteLine($"Играч {player} беше добавен в отбора {Name}.");
                LogHistory.Add(new TeamHistoryRecord(DateTime.Now, $"Играчът {player.Name} с позиция {player.Position} се присъедини към отбора."));
            }
        }

        public void RemovePlayer(string playerName)
        {
            var player = Players.FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));
            if (player != null)
            {
                Players.Remove(player);
                Console.WriteLine($"Играч {playerName} беше премахнат от отбора {Name}.");
                LogHistory.Add(new TeamHistoryRecord(DateTime.Now, $"Играчът {playerName} е премахнат от отбора."));
            }
            else
            {
                Console.WriteLine($"Играч {playerName} не беше намерен в отбора {Name}.");
            }
        }

        public override string ToString()
        {
            return $"{Name} (Брой играчи: {Players.Count})";
        }
    }
}
