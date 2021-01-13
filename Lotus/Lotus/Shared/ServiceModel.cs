using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    public class ServiceModel
    {
        public int Id { get; set; }

        public string Category_Id { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }

        public double Price { get; set; }
    }
}
