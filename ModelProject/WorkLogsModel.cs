using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class WorkLogsModel
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string MSG { get; set; }
        public int? MSGStatus { get; set; }
    }
}
