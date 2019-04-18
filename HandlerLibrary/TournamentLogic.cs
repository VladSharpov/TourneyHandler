using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel tournament)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(tournament.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int emptySlots = FindNumberOfEmptySlots(rounds, randomizedTeams.Count);

            tournament.Rounds.Add(CreateFirstRound(emptySlots, randomizedTeams));

            CreateOtherRounds(tournament, rounds);
        }

        // TODO - Refactor logic

        private static void CreateOtherRounds(TournamentModel tournament, int rounds)
        {
            int currentRoundNumber = 2; // First already created
            List<MatchupModel> previousRoundMatchups = tournament.Rounds.First();
            List<MatchupModel> currentRoundMatchups = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();

            while (currentRoundNumber <= rounds)
            {
                foreach (MatchupModel matchup in previousRoundMatchups)
                {
                    currentMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = matchup });

                    if (currentMatchup.Entries.Count > 1)
                    {
                        currentMatchup.MatchupRound = currentRoundNumber;
                        currentRoundMatchups.Add(currentMatchup);
                        currentMatchup = new MatchupModel();
                    }
                }

                tournament.Rounds.Add(currentRoundMatchups);
                previousRoundMatchups = currentRoundMatchups;

                currentRoundMatchups = new List<MatchupModel>();
                currentRoundNumber++;
            }
        }

        private static List<MatchupModel> CreateFirstRound(int emptySlots, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                currentMatchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                if (emptySlots > 0 || currentMatchup.Entries.Count > 1)
                {
                    currentMatchup.MatchupRound = 1;
                    output.Add(currentMatchup);
                    currentMatchup = new MatchupModel();

                    if (emptySlots > 0)
                    {
                        emptySlots--;
                    }
                }
            }

            return output;
        }

        private static int FindNumberOfEmptySlots(int rounds, int teams)
        {
            int output = 0;
            int totalTeams = 0;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - teams;

            return output;
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output++;
                val *= 2;
            }

            return output;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return new List<TeamModel>(teams.OrderBy(x => Guid.NewGuid()));
        }
    }
}
