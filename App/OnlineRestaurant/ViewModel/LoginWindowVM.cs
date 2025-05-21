using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class LoginWindowVM 
    {
        public string EmailText { get; set; }
        public string PasswordText { get; set; }
        public bool IsAdminChecked { get; set; }
        public bool IsUserChecked { get; set; }


        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly INavigationServiceAsync _navigationServiceAsync;
        public ObservableCollection<string> UserTypesItems { get; set; }

        public LoginWindowVM(INavigationService navigationService,INavigationServiceAsync navigationServiceAsync, IUserService userService)
        {
            _navigationService = navigationService;
            _navigationServiceAsync = navigationServiceAsync;
            _userService = userService;

            LoginCommand = new AsyncRelayCommand(Login_Execute, Login_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);
        }

        public async Task Login_Execute()
        {
            User user = null;
            if (IsAdminChecked)
                user = await _userService.CanLoginUserAsync(EmailText, PasswordText, UserType.Admin);
            else if (IsUserChecked)
                user = await _userService.CanLoginUserAsync(EmailText, PasswordText, UserType.Registered);

            if (user != null)
            {
                if (IsAdminChecked)
                    _navigationService.NavigateTo<AdministrationWindow>();
                else if (IsUserChecked)
                {
                    _navigationServiceAsync.NavigateToAsync<UserWindow>(user);
                }
                else
                {
                    MessageBox.Show("This user does not exist in the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public bool Login_CanExecute()
        {
            return EmailText != string.Empty && PasswordText != string.Empty;
        }

        public void Cancel_Execute()
        {
            _navigationService.NavigateTo<StartupWindow>();
        }
    }
}
