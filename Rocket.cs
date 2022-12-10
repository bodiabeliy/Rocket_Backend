namespace RetrievePlugin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Rocket : Level
    {
        public Rocket()
        {
            L2Items = new List<L2>();
        }

        public List<L2> L2Items { get; set; }
    }
}
