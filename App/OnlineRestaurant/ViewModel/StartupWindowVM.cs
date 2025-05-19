using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace OnlineRestaurant.UI.ViewModel
{
    public class StartupWindowVM : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        private string _usernameText;
        private string _passwordText;
        private string _errorMessage;
        private bool _isLoading;

        #region Properties
        public string UsernameText
        {
            get => _usernameText;
            set
            {
                _usernameText = value;
                OnPropertyChanged(nameof(UsernameText));
            }
        }

        public string PasswordText
        {
            get => _passwordText;
            set
            {
                _passwordText = value;
                OnPropertyChanged(nameof(PasswordText));
            }
        }

        #endregion

        #region Commands
        public ICommand LoginAsGuestCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        #endregion

        #region Constructor
        public StartupWindowVM(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            // Initialize commands
            LoginAsGuestCommand = new RelayCommand(Login_Execute);
            LoginCommand = new RelayCommand(Login_Execute);
            RegisterCommand = new RelayCommand(Register_Execute);
        }
        #endregion

        #region Command Methods
        public void Login_Execute()
        {
            _navigationService.NavigateTo<LoginWindow>();
        }

        public void Register_Execute()
        {
            _navigationService.NavigateTo<RegisterWindow>();
        }
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
