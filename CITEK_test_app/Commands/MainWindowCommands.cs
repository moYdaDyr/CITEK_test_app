using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CITEK_test_app
{
    public static class MainWindowCommands
    {

        static MainWindowCommands()
        {
            UpdateDataCommand = new RoutedUICommand("Update data", "UpdateDataCommand", typeof(MainWindowCommands));
            SaveDataInReportCommand = new RoutedUICommand("Save sata", "SaveDataInReportCommand", typeof(MainWindowCommands));
        }

        public static RoutedUICommand UpdateDataCommand { get; private set; }

        public static RoutedUICommand SaveDataInReportCommand { get; private set; }

    }
}
