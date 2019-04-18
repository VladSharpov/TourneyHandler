using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerUI
{
    public interface IPrizeRequester
    {
        void PrizeComplete(PrizeModel prize);
    }
}
