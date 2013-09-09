using Chavp.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ChavpCommandMaster
{
    public class CommandViewModel
        : INotifyPropertyChanged
    {
        public SystemCommand SystemCommand { get; set; }
        public BuildCommand BuildCommand { get; set; }

        public IList<string> BuildSlaveList { get; set; }

        public string SelectedSlaveName { get; set; }

        public string RepositoryProviderName
        {
            get { return BuildCommand.RepositoryProviderName; }
            set
            {
                BuildCommand.RepositoryProviderName = value;
                RaisePropertyChanged("RepositoryProviderName");
            }
        }

        public string RepositoryUri
        {
            get { return BuildCommand.RepositoryUri; }
            set 
            { 
                BuildCommand.RepositoryUri = value;
                RaisePropertyChanged("RepositoryUri");
            }
        }

        private string _AlertMessage;
        public string AlertMessage 
        {
            get { return _AlertMessage; }
            set
            {
                _AlertMessage = value;
                RaisePropertyChanged("AlertMessage");
            }
        }

        public string AnnounceMessage
        {
            get { return SystemCommand.Message; }
            set 
            { 
                SystemCommand.Message = value;
                RaisePropertyChanged("AnnounceMessage");
            }
        }

        public CommandViewModel()
        {
            SystemCommand = new SystemCommand();
            BuildCommand = new BuildCommand {  };
            BuildSlaveList = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
