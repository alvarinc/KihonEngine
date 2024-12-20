using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngine.Core.Server.Commands
{
    internal class GameCommandResult
    {
        public bool StateUpdated { get; set; }
        public bool PeerInitializated { get; set; }
    }
}
