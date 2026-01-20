using CITEK_test_app;
using CITEK_test_app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CITEK_test_app
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableDictionary<int, AddressObjectTable>? AddressObjectTables { get; set; }

        string _header;
        public string Header
        {
            get
            {
                return _header;
            }
            private set
            {
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        double _progressPercent;
        public double ProgressPercent
        {
            get
            {
                return _progressPercent;
            }
            private set
            {
                _progressPercent = value;
                OnPropertyChanged("ProgressPercent");
            }
        }

        bool _updatingDataAllowed;
        public bool UpdatingDataAllowed
        {
            get
            {
                return _updatingDataAllowed;
            }
            private set
            {
                _updatingDataAllowed = value;
                OnPropertyChanged("UpdatingDataAllowed");
            }
        }

        bool _creatingReportAllowed;
        public bool CreatingReportAllowed
        {
            get
            {
                return _creatingReportAllowed;
            }
            private set
            {
                _creatingReportAllowed = value;
                OnPropertyChanged("CreatingReportAllowed");
            }
        }

        string _log;
        public string Log
        {
            get
            {
                return _log;
            }
            private set
            {
                _log = value;
                OnPropertyChanged("Log");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public MainWindowViewModel()
        {
            UpdatingDataAllowed = true;
            CreatingReportAllowed = false;

            ProgressPercent = 0.0;
            Log = "Нажмите \"Загрузить данные\" чтобы начать работу\n";

            DataLoader.SetProgressPercentEvent += SetProgress;
            Logger.UpdateLogEvent += AddLogLine;
        }

        private async void UpdateDataCommandFunc()
        {
            UpdatingDataAllowed = false;
            CreatingReportAllowed = false;

            await DataLoader.UpdateDataAsync();

            AddressObjectTables = DataAnalizer.AnalizeData();

            if (AddressObjectTables == null)
            {
                UpdatingDataAllowed = true;
                CreatingReportAllowed = false;
                return;
            }

            var date = DataAnalizer.GetDate();
            Header = "Отчёт по добавленным адресным объектам за " + date.ToShortDateString();

            UpdatingDataAllowed = true;
            CreatingReportAllowed = true;
        }

        private void SetProgress(double progress)
        {
            ProgressPercent = progress;
        }
        private void AddLogLine(string line)
        {
            Log += line+"\n";
        }

        public void UpdateDataCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateDataCommandFunc();
        }

        public void SaveDataInReportCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            UpdatingDataAllowed = false;
            CreatingReportAllowed = false;

            ReportCreator.CreateReportAsync(AddressObjectTables, Header);

            UpdatingDataAllowed = true;
            CreatingReportAllowed = true;
        }
    }
}
