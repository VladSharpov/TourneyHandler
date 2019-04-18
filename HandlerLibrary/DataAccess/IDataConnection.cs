using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary.DataAccess
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel prize);

        PersonModel CreatePerson(PersonModel person);

        TeamModel CreateTeam(TeamModel team);

        void CreateTournament(TournamentModel tournament);

        List<PersonModel> GetPerson_All();

        List<TeamModel> GetTeam_All();
    }
}
