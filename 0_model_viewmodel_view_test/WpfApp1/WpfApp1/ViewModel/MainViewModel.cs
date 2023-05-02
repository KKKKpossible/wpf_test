using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace WpfApp1.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int baud;
        private string content;

        #region 생성자
        public MainViewModel()
        {
            baud = 115200;
            Content = string.Format("{0} baudrate", baud);
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Property
        public int Baud 
        {
            get
            {
                return baud;
            }
            set
            {
                int baud_buff = baud;

                baud = value;
                OnPropertyChanged(nameof(baud));

                if(baud < 115198)
                {
                    MessageBox.Show("error");
                    baud = baud_buff;
                    OnPropertyChanged(nameof(baud));
                }
                else
                {
                    OnPropertyChanged(nameof(EnablePlus));
                    Content = string.Format("{0} baudrate", baud);
                }
            }
        }
        public string Content 
        { 
            get => content;
            set
            {
                content = value;
                OnPropertyChanged(nameof(content));
            }
        }

        public bool EnablePlus
        {
            get
            {
                if (baud >= 115200)
                {
                    return false;
                }

                return true;
            }
        }
        #endregion

        #region Command
        private ICommand plusBaudCommand;
        public ICommand PlusBaudCommand
        {
            get { return (this.plusBaudCommand) ?? (this.plusBaudCommand = new DelegateCommand(PlusBaud)); }
        }
        private void PlusBaud()
        {
            Baud += 1;
        }

        private ICommand minusBaudCommand;
        public ICommand MiusBaudCommand
        {
            get { return (this.minusBaudCommand) ?? (this.minusBaudCommand = new DelegateCommand(MiusBaud)); }
        }
        private void MiusBaud()
        {
            Baud -= 1;
        }

        private ICommand pageContentCommand;
        public ICommand PageContentCommand
        {
            get { return (this.pageContentCommand) ?? (this.pageContentCommand = new DelegateCommand(PageContent)); }
        }


        private void PageContent()
        { 
        }
        #endregion
    }

    #region DelegateCommand Class
    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;


        public DelegateCommand(Action execute) : this(execute, null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(this.canExecute == null)
            {
                return true;
            }
            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            this.execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if(this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}

#endregion