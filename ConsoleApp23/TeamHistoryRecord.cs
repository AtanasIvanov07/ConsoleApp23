using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    public class TeamHistoryRecord
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public TeamHistoryRecord(DateTime timestamp, string message)
        {
            Timestamp = timestamp;
            Message = message;
        }
    }
}
