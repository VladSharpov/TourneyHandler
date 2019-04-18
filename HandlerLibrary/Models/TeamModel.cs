using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary.Models
{
    public class TeamModel
    {
        /// <summary>
        /// The unique identifier for the team.
        /// </summary>
        public int Id { get; set; }

        public string TeamName { get; set; }

        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
    }
}
