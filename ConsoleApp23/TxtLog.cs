using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    public class TxtLog : ILog
    {
        public void WriteTeam(Team team, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Отбор: {team.Name}");
                writer.WriteLine("Списък с играчи:");
                if (team.Players.Count > 0)
                {
                    foreach (var player in team.Players)
                    {
                        writer.WriteLine($"- {player.Name} ({player.Position})");
                    }
                }
                else
                {
                    writer.WriteLine("Няма играчи.");
                }
                writer.WriteLine();
                writer.WriteLine("История на отбора:");
                foreach (var a in team.LogHistory)
                {
                    writer.WriteLine($"{a.Timestamp}: {a.Message}");
                }
            }
            Console.WriteLine($"Отборът {team.Name} е записан във файл {filePath} във формат TXT.");
        }

        public void WriteHistory(Team team, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("История на отбора:");
                foreach (var record in team.LogHistory)
                {
                    writer.WriteLine($"{record.Timestamp}: {record.Message}");
                }
            }
            Console.WriteLine($"Историята на отбора {team.Name} е записана във файл {filePath} във формат TXT.");
        }
    }
}
