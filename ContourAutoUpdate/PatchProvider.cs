using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourAutoUpdate
{
    internal class PatchProvider
    {
        public PatchServerInfo Server { get; }
        public PatchProvider(PatchServerInfo server) => Server = server;
    }
}
