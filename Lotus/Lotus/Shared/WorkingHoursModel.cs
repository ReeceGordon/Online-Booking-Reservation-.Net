using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    public class WorkingHoursModel
    {
        public int Id { get; set; }
        public string Staff_Id { get; set; }
        public string MondayHours { get; set; }
        public bool MondayClosed { get; set; }
        public string TuesdayHours { get; set; }
        public bool TuesdayClosed { get; set; }
        public string WednesdayHours { get; set; }
        public bool WednesdayClosed { get; set; }
        public string ThursdayHours { get; set; }
        public bool ThursdayClosed { get; set; }
        public string FridayHours { get; set; }
        public bool FridayClosed { get; set; }
        public string SaturdayHours { get; set; }
        public bool SaturdayClosed { get; set; }
        public string SundayHours { get; set; }
        public bool SundayClosed { get; set; }
    }
}
