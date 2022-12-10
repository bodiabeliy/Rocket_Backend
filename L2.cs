namespace RetrievePlugin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class L2: Level
    {
        public L2()
        {
            L3Items = new List<L3>();
        }

        public List<L3> L3Items { get; set; }
    }
}
