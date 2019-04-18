using Dapper;
using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//@PlaceNumber int,
//@PlaceName nvarchar(50),
//@PrizeAmount money,
//@PrizePercentage float,
//@id int = 0 output

namespace HandlerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private const string dbName = "Tournaments";

        /// <summary>
        /// Saves a new prize to the database.
        /// </summary>
        /// <param name="prize">The prize information</param>
        /// <returns>The prize information, including its unique identifier in the database.</returns>
        public PrizeModel CreatePrize(PrizeModel prize)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(dbName)))
            {
                var @params = new DynamicParameters();
                @params.Add("@PlaceNumber", prize.PlaceNumber);
                @params.Add("@PlaceName", prize.PlaceName);
                @params.Add("@PrizeAmount", prize.PrizeAmount);
                @params.Add("@PrizePercentage", prize.PrizePercentage);
                @params.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", @params, commandType: CommandType.StoredProcedure);

                prize.Id = @params.Get<int>("@id");

                return prize;
            }
        }

        public PersonModel CreatePerson(PersonModel person)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(dbName)))
            {
                var @params = new DynamicParameters();
                @params.Add("@FirstName", person.FirstName);
                @params.Add("@LastName", person.LastName);
                @params.Add("@EmailAddress", person.EmailAddress);
                @params.Add("@CellphoneNumber", person.CellphoneNumber);
                @params.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", @params, commandType: CommandType.StoredProcedure);

                person.Id = @params.Get<int>("@id");

                return person;
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(dbName)))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }

            return output;
        }

        public TeamModel CreateTeam(TeamModel team)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(dbName)))
            {
                var @params = new DynamicParameters();
                @params.Add("@TeamName", team.TeamName);
                @params.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("spTeams_Insert", @params, commandType: CommandType.StoredProcedure);

                team.Id = @params.Get<int>("@id");

                foreach (PersonModel teamMember in team.TeamMembers)
                {
                    @params = new DynamicParameters();
                    @params.Add("@TeamId", team.Id);
                    @params.Add("@PersonId", teamMember.Id);

                    connection.Execute("spTeamMembers_Insert", @params, commandType: CommandType.StoredProcedure);
                }

                return team;
            }
        }


        public void CreateTournament(TournamentModel tournament)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(dbName)))
            {
                SaveTournament(tournament, connection);

                SaveTournamentPrizes(tournament, connection);

                SaveTournamentEntries(tournament, connection);

                SaveTournamentRounds(tournament, connection);
            }
        }

        private void SaveTournamentRounds(TournamentModel tournament, IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        private static void SaveTournamentEntries(TournamentModel tournament, IDbConnection connection)
        {
            foreach (TeamModel team in tournament.EnteredTeams)
            {
                var @params = new DynamicParameters();
                @params.Add("@TournamentId", tournament.Id);
                @params.Add("@TeamId", team.Id);

                connection.Execute("spTournamentEntries_Insert", @params, commandType: CommandType.StoredProcedure);
            }
        }

        private static void SaveTournamentPrizes(TournamentModel tournament, IDbConnection connection)
        {
            foreach (PrizeModel prize in tournament.Prizes)
            {
                var @params = new DynamicParameters();
                @params.Add("@TournamentId", tournament.Id);
                @params.Add("@PrizeId", prize.Id);

                connection.Execute("spTournamentPrizes_Insert", @params, commandType: CommandType.StoredProcedure);
            }
        }

        private static void SaveTournament(TournamentModel tournament, IDbConnection connection)
        {
            var @params = new DynamicParameters();
            @params.Add("@TournamentName", tournament.TournamentName);
            @params.Add("@EntryFee", tournament.EntryFee);
            @params.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("spTournaments_Insert", @params, commandType: CommandType.StoredProcedure);

            tournament.Id = @params.Get<int>("@id");
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(dbName)))
            {
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();

                foreach (TeamModel team in output)
                {
                    var @params = new DynamicParameters();
                    @params.Add("@TeamId", team.Id);

                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", @params, 
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }

            return output;
        }
    }
}
