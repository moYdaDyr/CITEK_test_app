using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITEK_test_app.Models
{
    public static class Logger
    {
        public delegate void UpdateLogDelegate(string line);

        public static event UpdateLogDelegate UpdateLogEvent;

        public static void UpdateLog(string line)
        {
            UpdateLogEvent(line);
        }
    }
}
