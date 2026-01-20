using System.Reflection.PortableExecutable;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CITEK_test_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            CommandBinding updateDataCommandBinding = new CommandBinding();
            updateDataCommandBinding.Command = MainWindowCommands.UpdateDataCommand;
            updateDataCommandBinding.Executed += (DataContext as MainWindowViewModel).UpdateDataCommand_Execute;
            UpdateDataButton.CommandBindings.Add(updateDataCommandBinding);
            UpdateDataButton.Command = MainWindowCommands.UpdateDataCommand;

            CommandBinding saveDataInReportCommandBinding = new CommandBinding();
            saveDataInReportCommandBinding.Command = MainWindowCommands.SaveDataInReportCommand;
            saveDataInReportCommandBinding.Executed += (DataContext as MainWindowViewModel).SaveDataInReportCommand_Execute;
            SaveDataInReportButton.CommandBindings.Add(saveDataInReportCommandBinding);
            SaveDataInReportButton.Command = MainWindowCommands.SaveDataInReportCommand;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}