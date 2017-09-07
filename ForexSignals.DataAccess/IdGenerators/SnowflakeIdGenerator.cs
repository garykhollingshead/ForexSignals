using System;
using System.Collections.Generic;
using System.Text;
using Flakey;

namespace ForexSignals.DataAccess.IdGenerators
{
    public static class SnowflakeIdGenerator
    {
        private static readonly IdGenerator IdGen = new IdGenerator(0, new DateTime(2013, 1, 1));

        public static string NewIdAsString => IdGen.CreateId().ToString();
    }
}
