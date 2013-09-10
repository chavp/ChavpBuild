using Chavp.Messages;
using EasyNetQ;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace ChavpCommandMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBus _bus;
        IPublishChannel _publishBuildCommandChannel;
        IPublishChannel _publishSystemCommandChannel;

        CommandViewModel _buildCommandVM;
        public MainWindow()
        {
            InitializeComponent();

            string rabbitMQBrokerHost = "localhost";
            string virtualHost = "command-pipeline";
            string username = "guest";
            string password = "guest";

            string connectionString = string.Format(
                "host={0};virtualHost={1};username={2};password={3}",
                rabbitMQBrokerHost, virtualHost, username, password);

            _bus = RabbitHutch.CreateBus(connectionString);

            _publishBuildCommandChannel = _bus.OpenPublishChannel(x => x.WithPublisherConfirms());
            _publishSystemCommandChannel = _bus.OpenPublishChannel();

            _buildCommandVM = new CommandViewModel();
            _buildCommandVM.AlertMessage = "[Alert Message]";
            _buildCommandVM.AnnounceMessage = "Hello Build Slave";

            DataContext = _buildCommandVM;

            _buildCommandVM.BuildSlaveList.Add("cb-slave-1");
            _buildCommandVM.BuildSlaveList.Add("cb-slave-2");
        }

        private void Bulid_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_buildCommandVM.SelectedSlaveName))
            {
                _buildCommandVM.AlertMessage = "Please selectde Chavp Build Slave";

                return;
            }

            var buildCommand = new BuildCommand
            {
                RepositoryProviderName = _buildCommandVM.RepositoryProviderName,
                RepositoryUri = _buildCommandVM.RepositoryUri
            };

            _publishBuildCommandChannel.Publish<BuildCommand>(
                buildCommand,
                x => x.WithTopic(_buildCommandVM.SelectedSlaveName)
                .OnSuccess(() =>
                {
                    _buildCommandVM.AlertMessage = string.Format(
                        "Build {0} Success. {1}", _buildCommandVM.SelectedSlaveName, DateTime.Now.ToString());
                })
                .OnFailure(() =>
                {
                    _buildCommandVM.AlertMessage = string.Format(
                        "Build {0} Fail. {1}", _buildCommandVM.SelectedSlaveName, DateTime.Now.ToString());
                }));
        }

        private void Announce_Click(object sender, RoutedEventArgs e)
        {
            var sysCommand = new SystemCommand
            {
                Message = _buildCommandVM.AnnounceMessage
            };

            _publishSystemCommandChannel.Publish<SystemCommand>(sysCommand);

        }

        protected override void OnClosed(EventArgs e)
        {
            if (_bus != null)
            {
                _bus.Dispose();
            }
            base.OnClosed(e);
        }
  
    }
}
