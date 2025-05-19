using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OnlineRestaurant.UI.ViewModel
{
    public class RegisterWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;

        #region Private Members
        private string _firstNameText;
        private string _lastNameText;
        private string _emailText;
        private string _passwordText;  
        private string _phoneNumberText;
        private string _addressText;
        #endregion

        #region Data binding Properties
        public string FirstNameText
        {
            get { return _firstNameText; }
            set
            {
                _firstNameText = value;
                OnPropertyChanged(nameof(FirstNameText));
            }
        }

        public string LastNameText
        {
            get { return _lastNameText; }
            set
            {
                _lastNameText = value; 
                OnPropertyChanged(nameof(LastNameText));
            }
        }
        
        public string EmailText
        {
            get { return _emailText; }
            set
            {
                _emailText = value;
                OnPropertyChanged(nameof(EmailText));
            }
        }

        public string PasswordText
        {
            get { return _passwordText; }
            set
            {
                _passwordText = value;
                OnPropertyChanged(nameof(PasswordText));
            }
        }

        public string PhoneNumberText
        {
            get { return _phoneNumberText; }
            set
            {
                _phoneNumberText = value;
                OnPropertyChanged(nameof(PhoneNumberText));
            }
        }

        public string AddressText
        {
            get { return _addressText; }
            set
            {
                _addressText = value;
                OnPropertyChanged(nameof(AddressText));
            }
        }
        #endregion

        #region Commands
        public ICommand CreateAccountCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RegisterWindowVM(IUserService userService, INavigationService navigationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            CreateAccountCommand = new AsyncRelayCommand(CreateAccount_Execute, CreateAccount_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);
        }

        public async Task CreateAccount_Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(FirstNameText) || string.IsNullOrEmpty(LastNameText) ||
                    string.IsNullOrEmpty(EmailText) || string.IsNullOrEmpty(PasswordText))
                {
                    MessageBox.Show("Please fill all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newUser = new User
                {
                    FirstName = FirstNameText,
                    LastName = LastNameText,
                    Email = EmailText,
                    Phone = PhoneNumberText ?? string.Empty,
                    Address = AddressText ?? string.Empty,
                    Password = PasswordText,
                    Type = UserType.Registered
                };

                await _userService.AddUserAsync(newUser);

                ClearTextBoxes();
                MessageBox.Show("User account Created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException?.Message.Contains("duplicate") == true)
                {
                    MessageBox.Show("An account with this email already exists.");
                }
                else
                {
                    MessageBox.Show("Database error occurred. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating account. Please try again.");
            }
        }

        public void Cancel_Execute()
        {
            _navigationService.NavigateTo<StartupWindow>();
        }

        public bool CreateAccount_CanExecute()
        {
            return !(string.IsNullOrEmpty(FirstNameText) || string.IsNullOrEmpty(LastNameText) ||
                    string.IsNullOrEmpty(EmailText) || string.IsNullOrEmpty(PasswordText));
        }

        public void ClearTextBoxes()
        {
            FirstNameText = LastNameText = EmailText = PasswordText = AddressText = PhoneNumberText = string.Empty;
        }
    }
}
