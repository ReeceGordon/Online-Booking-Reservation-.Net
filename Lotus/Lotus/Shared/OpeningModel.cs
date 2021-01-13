using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    public class OpeningModel
    {
        public int Id { get; set; }

        public string Day { get; set; }

        public int Opening { get; set; }

        public int Closing { get; set; }

        public bool Closed { get; set; }
    }
}
