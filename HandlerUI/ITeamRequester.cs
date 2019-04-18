using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerUI
{
    public interface ITeamRequester
    {
        void TeamComplete(TeamModel team);
    }
}
