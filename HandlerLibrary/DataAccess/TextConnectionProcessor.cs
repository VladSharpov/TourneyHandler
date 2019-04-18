using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectionProcessor
    {
        public static string GenerateFullPath(this string fileName)
        {
            return Path.Combine(ConfigurationManager.AppSettings["directoryPath"], fileName);
        }

        public static List<string> GetListFromFile(this string path)
        {
            if (!File.Exists(path))
            {
                return new List<string>();
            }

            return File.ReadAllLines(path).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                var prizeModel = new PrizeModel
                {
                    Id = int.Parse(cols[0]), // not TryParse, because we don't want to continue with half-good data
                    PlaceNumber = int.Parse(cols[1]),
                    PlaceName = cols[2],
                    PrizeAmount = decimal.Parse(cols[3]),
                    PrizePercentage = double.Parse(cols[4])
                };

                output.Add(prizeModel);
            }

            return output;
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            // TODO - library AutoMapper to implement DRY

            var output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                var personModel = new PersonModel();
                personModel.Id = int.Parse(cols[0]); // not TryParse, because we don't want to continue with half-good data
                personModel.FirstName = cols[1];
                personModel.LastName = cols[2];
                personModel.EmailAddress = cols[3];
                personModel.CellphoneNumber = cols[4];

                output.Add(personModel);
            }

            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> allPeople = peopleFileName.GenerateFullPath().GetListFromFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                var team = new TeamModel
                {
                    Id = int.Parse(cols[0]),
                    TeamName = cols[1]
                };

                HashSet<int> teamMemberIds = cols[2].Split('|').Select(x => int.Parse(x)).ToHashSet();
                team.TeamMembers = allPeople.Where(x => teamMemberIds.Contains(x.Id)).ToList();

                output.Add(team);
            }

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(
            this List<string> lines, 
            string teamFileName, 
            string peopleFileName, 
            string prizeFileName)
        {
            //id,TournamentName,EntryFee,(id|id|id - EnteredTeams),(id|id|id - Prizes),(Rounds - id^id^id|id^id^id|id^id^id)

            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> allTeams = teamFileName.GenerateFullPath().GetListFromFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> allPrizes = prizeFileName.GenerateFullPath().GetListFromFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                var tournament = new TournamentModel
                {
                    Id = int.Parse(cols[0]),
                    TournamentName = cols[1],
                    EntryFee = decimal.Parse(cols[2])
                };

                HashSet<int> teamIds = cols[3].Split('|').Select(x => int.Parse(x)).ToHashSet();
                tournament.EnteredTeams = allTeams.Where(x => teamIds.Contains(x.Id)).ToList();

                HashSet<int> prizeIds = cols[4].Split('|').Select(x => int.Parse(x)).ToHashSet();
                tournament.Prizes = allPrizes.Where(x => prizeIds.Contains(x.Id)).ToList();

                // TODO - Capture rounds information

                output.Add(tournament);
            }

            return output;
        }

        public static void SavePrizesToFile(this List<PrizeModel> prizes, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(var model in prizes)
            {
                lines.Add($"{ model.Id },{ model.PlaceNumber },{ model.PlaceName },{ model.PrizeAmount },{ model.PrizePercentage }");
            }

            File.WriteAllLines(fileName.GenerateFullPath(), lines);
        }

        public static void SavePeopleToFile(this List<PersonModel> people, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (var person in people)
            {
                lines.Add($"{ person.Id },{ person.FirstName },{ person.LastName },{ person.EmailAddress },{ person.CellphoneNumber }");
            }

            File.WriteAllLines(fileName.GenerateFullPath(), lines);
        }

        public static void SaveTeamsToFile(this List<TeamModel> teams, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (var team in teams)
            {
                StringBuilder teamMembers = new StringBuilder();

                if (team.TeamMembers.Count > 0)
                {
                    team.TeamMembers.ForEach(x => teamMembers.Append($"{ x.Id }|"));
                    teamMembers.Remove(teamMembers.Length - 1, 1); // Removing last redunant '|'
                }

                lines.Add($"{ team.Id },{ team.TeamName },{ teamMembers }");
            }

            File.WriteAllLines(fileName.GenerateFullPath(), lines);
        }

        public static void SaveTournamentsToFile(this List<TournamentModel> tournaments, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (var tournament in tournaments)
            {
                StringBuilder teams = new StringBuilder();
                StringBuilder prizes = new StringBuilder();
                StringBuilder rounds = new StringBuilder();

                if (tournament.EnteredTeams.Count > 0)
                {
                    tournament.EnteredTeams.ForEach(x => teams.Append($"{ x.Id }|"));
                    teams.Remove(teams.Length - 1, 1); // Removing last redunant '|'
                }

                if (tournament.Prizes.Count > 0)
                {
                    tournament.Prizes.ForEach(x => prizes.Append($"{ x.Id }|"));
                    prizes.Remove(prizes.Length - 1, 1);
                }

                foreach(List<MatchupModel> matchupList in tournament.Rounds)
                {
                    StringBuilder matchups = new StringBuilder();

                    if (matchupList.Count > 0)
                    {
                        matchupList.ForEach(x => matchups.Append($"{ x.Id }^"));
                        matchups.Remove(matchups.Length - 1, 1);
                    }

                    if (tournament.Rounds.Count > 0)
                    {
                        tournament.Rounds.ForEach(x => rounds.Append($"{ matchups }|"));
                        rounds.Remove(rounds.Length - 1, 1);
                    }
                }

                lines.Add($"{ tournament.Id },{ tournament.TournamentName },{ tournament.EntryFee },({ teams }),({ prizes }),({ rounds })");
            }

            File.WriteAllLines(fileName.GenerateFullPath(), lines);
        }
    }
}
