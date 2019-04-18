using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary.Models
{
    /// <summary>
    /// Represents one tournament, with all of the rounds, matchups, prizes and outcomes.
    /// </summary>
    public class TournamentModel
    {
        /// <summary>
        /// The unique identifier for the tournament.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the tournament.
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// The amount of money each team needs to put up to enter.
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// The list of entered teams.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// The list of prizes for various places.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// The list of rounds, each consists of list of matchups.
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
    }
}
