using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    public class AppointmentsModel
    {
        public int Id { get; set; }
        public string Member_Id { get; set; }
        public string Staff_Id { get; set; }
        public string Type { get; set; }
        public string Full_Name { get; set; }
        public string Start { get; set; }
        public int Duration { get; set; }
        public string End { get; set; }
        public DateTime App_Date { get; set; }
        public string Status { get; set; }
    }
}
