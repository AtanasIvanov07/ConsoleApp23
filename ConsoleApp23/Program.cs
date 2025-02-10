using ConsoleApp23;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace TeamPlayerManager
{
    class Program
    {
        static Dictionary<string, Team> teams = new Dictionary<string, Team>(StringComparer.OrdinalIgnoreCase);
        static Dictionary<string, Player> freePlayers = new Dictionary<string, Player>(StringComparer.OrdinalIgnoreCase);

        static void Main(string[] args)
        {
            Console.WriteLine("Enter commands. To exit, type 'exit'.");
            string input;
            while ((input = Console.ReadLine()) != null)
            {
                if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                var tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0)
                    continue;

                string command = tokens[0].ToLower();
                Console.WriteLine($"Received command: {command}");

                switch (command)
                {
                    case "create_team":
                        CreateTeam(tokens);
                        break;
                    case "create_player":
                        CreatePlayer(tokens);
                        break;
                    case "add_player":
                        AddPlayer(tokens);
                        break;
                    case "remove_player":
                        RemovePlayer(tokens);
                        break;
                    case "print_team":
                        PrintTeam(tokens);
                        break;
                    case "print_log_txt":
                        PrintLog(tokens, logType: "txt");
                        break;
                    case "print_log_excel":
                        PrintLog(tokens, logType: "excel");
                        break;
                    default:
                        Console.WriteLine("Unrecognized command.");
                        break;
                }
            }
        }

        private static void CreateTeam(string[] tokens)
        {
            if (tokens.Length < 2)
            {
                Console.WriteLine("Error: Not enough arguments for create_team.");
                return;
            }

            string teamName = tokens[1];
            if (teams.ContainsKey(teamName))
            {
                Console.WriteLine($"A team with the name {teamName} already exists.");
            }
            else
            {
                var team = new Team(teamName);
                teams.Add(teamName, team);
                Console.WriteLine($"Team {teamName} has been created.");
            }
        }

        private static void CreatePlayer(string[] tokens)
        {
            if (tokens.Length < 3)
            {
                Console.WriteLine("Error: Not enough arguments for create_player.");
                return;
            }

            string playerName = tokens[1];
            string position = tokens[2];

            if (freePlayers.ContainsKey(playerName))
            {
                Console.WriteLine($"A player with the name {playerName} already exists.");
            }
            else
            {
                var player = new Player(playerName, position);
                freePlayers.Add(playerName, player);
                Console.WriteLine($"Player {player} has been created (without a team).");
            }
        }

        private static void AddPlayer(string[] tokens)
        {
            if (tokens.Length < 4)
            {
                Console.WriteLine("Error: Not enough arguments for add_player.");
                return;
            }

            string teamName = tokens[1];
            string playerName = tokens[2];
            string position = tokens[3];

            if (!teams.ContainsKey(teamName))
            {
                Console.WriteLine($"Team {teamName} does not exist.");
                return;
            }

            Player player;
            if (freePlayers.ContainsKey(playerName))
            {
                player = freePlayers[playerName];
                if (!player.Position.Equals(position, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Warning: Existing player {playerName} has position {player.Position}, but the given position is {position}. Using {player.Position}.");
                }
            }
            else
            {
                player = new Player(playerName, position);
                freePlayers.Add(playerName, player);
                Console.WriteLine($"Player {player} was created and added as a free player.");
            }

            teams[teamName].AddPlayer(player);
        }

        private static void RemovePlayer(string[] tokens)
        {
            if (tokens.Length < 3)
            {
                Console.WriteLine("Error: Not enough arguments for remove_player.");
                return;
            }

            string teamName = tokens[1];
            string playerName = tokens[2];

            if (!teams.ContainsKey(teamName))
            {
                Console.WriteLine($"Team {teamName} does not exist.");
                return;
            }

            teams[teamName].RemovePlayer(playerName);
        }

        private static void PrintTeam(string[] tokens)
        {
            if (tokens.Length < 4)
            {
                Console.WriteLine("Error: Not enough arguments for print_team.");
                return;
            }

            string teamName = tokens[1];
            string filePath = tokens[2];
            string logType = tokens[3].ToLower();

            if (!teams.ContainsKey(teamName))
            {
                Console.WriteLine($"Team {teamName} does not exist.");
                return;
            }

            ILog? logger = GetLogger(logType);
            if (logger == null)
            {
                Console.WriteLine($"Unsupported log type: {logType}. Use 'txt' or 'excel'.");
                return;
            }

            logger.WriteTeam(teams[teamName], filePath);
        }

        private static void PrintLog(string[] tokens, string logType)
        {
            if (tokens.Length < 3)
            {
                Console.WriteLine("Error: Not enough arguments for print_log.");
                return;
            }

            string teamName = tokens[1];
            string filePath = tokens[2];

            if (!teams.ContainsKey(teamName))
            {
                Console.WriteLine($"Team {teamName} does not exist.");
                return;
            }

            ILog? logger = GetLogger(logType);
            if (logger == null)
            {
                Console.WriteLine($"Unsupported log type: {logType}.");
                return;
            }

            logger.WriteHistory(teams[teamName], filePath);
        }

        private static ILog? GetLogger(string logType)
        {
            switch (logType)
            {
                case "txt":
                    return new TxtLog();
                case "excel":
                    return new ExcelLog();
                default:
                    return null;
            }
        }
    }
}
public class ExcelLog : ILog
{
    public void WriteTeam(Team team, string filePath)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Team");
            worksheet.Cells[1, 1].Value = "Player Name";
            worksheet.Cells[1, 2].Value = "Position";

            int row = 2;
            foreach (var player in team.Players)
            {
                worksheet.Cells[row, 1].Value = player.Name;
                worksheet.Cells[row, 2].Value = player.Position;
                row++;
            }

            SaveExcelFile(package, filePath);
        }
    }

    public void WriteHistory(Team team, string filePath)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("History");
            worksheet.Cells[1, 1].Value = "Timestamp";
            worksheet.Cells[1, 2].Value = "Message";

            int row = 2;
            foreach (var record in team.LogHistory)
            {
                worksheet.Cells[row, 1].Value = record.Timestamp;
                worksheet.Cells[row, 2].Value = record.Message;
                row++;
            }

            SaveExcelFile(package, filePath);
        }
    }

    private void SaveExcelFile(ExcelPackage package, string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        package.SaveAs(fileInfo);
    }
}