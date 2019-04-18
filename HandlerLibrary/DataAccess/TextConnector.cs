using HandlerLibrary.DataAccess.TextHelpers;
using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizeFileName = "PrizeModels.csv";
        private const string PeopleFileName = "PersonModels.csv";
        private const string TeamFileName = "TeamModels.csv";
        private const string TournamentFileName = "TournamentModels.csv";

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output = PeopleFileName.GenerateFullPath().GetListFromFile().ConvertToPersonModels();

            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output = TeamFileName.GenerateFullPath().GetListFromFile().ConvertToTeamModels(PeopleFileName);

            return output;
        }

        public PrizeModel CreatePrize(PrizeModel prize)
        {
            // Load the text file and convert to List<PrizeModel>
            List<PrizeModel> prizes = PrizeFileName.GenerateFullPath().GetListFromFile().ConvertToPrizeModels();
            // TODO check if Directory exists, add if not

            // Find max id and assign proper id to the new model
            if (prizes.Count > 0)
            {
                int maxId = prizes.Max(x => x.Id);
                prize.Id = maxId + 1;
            }
            else
            {
                prize.Id = 1;
            }

            // Add model to the List<PrizeModel>
            prizes.Add(prize);

            // Convert and save List<PrizeModel> to the text file
            prizes.SavePrizesToFile(PrizeFileName);

            return prize;
        }

        public PersonModel CreatePerson(PersonModel person)
        {
            List<PersonModel> people = GetPerson_All();

            if (people.Count > 0)
            {
                int maxId = people.Max(x => x.Id);
                person.Id = maxId + 1;
            }
            else
            {
                person.Id = 1;
            }

            people.Add(person);

            people.SavePeopleToFile(PeopleFileName);

            return person;
        }

        public TeamModel CreateTeam(TeamModel team)
        {
            List<TeamModel> teams = GetTeam_All();

            if (teams.Count > 0)
            {
                int maxId = teams.Max(x => x.Id);
                team.Id = maxId + 1;
            }
            else
            {
                team.Id = 1;
            }

            teams.Add(team);

            teams.SaveTeamsToFile(TeamFileName);

            return team;
        }

        public void CreateTournament(TournamentModel tournament)
        {
            List<TournamentModel> tournaments = TournamentFileName
                .GenerateFullPath()
                .GetListFromFile()
                .ConvertToTournamentModels(TeamFileName, PeopleFileName, PrizeFileName);

            if (tournaments.Count > 0)
            {
                int maxId = tournaments.Max(x => x.Id);
                tournament.Id = maxId + 1;
            }
            else
            {
                tournament.Id = 1;
            }

            tournaments.Add(tournament);

            tournaments.SaveTournamentsToFile(TournamentFileName);
        }
    }
}
