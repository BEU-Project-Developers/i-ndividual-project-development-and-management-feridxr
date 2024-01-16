using GalaSoft.MvvmLight.CommandWpf;

using Scheduler.Model.DBEntities;
using Scheduler.Properties;
using Scheduler.Services;
using Scheduler.View;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Scheduler.ViewModel
{
    public class LoginWindowViewModel : ViewModelBase
    {
        public const string LogFile = "logins.txt";

        private string _password;
        private string _userName;
        private Window loginWindow;

        public LoginWindowViewModel()
        {
            LoginCommand = new RelayCommand(async () => await TryLogin());
            CreateNewUserCommand = new RelayCommand(async () => await CreateNewUser());
        }

        public List<User> AllUsers
        {
            get
            {
                var context = new DBContext();
                List<User> users = context.User.ToList();
                return users;
            }
            set
            {
                var context = new DBContext();
                context.User.UpdateRange(value.ToList());
                context.SaveChanges();
            }
        }

        public RelayCommand CreateNewUserCommand { get; private set; }

        public RelayCommand LoginCommand { get; private set; }

        public string Password
        {
            get => _password;
            set
            {
                if (!string.Equals(_password, value))
                {
                    SetProperty(ref _password, value);
                    OnPropertyChanged();
                }
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (!string.Equals(_userName, value))
                {
                    SetProperty(ref _userName, value);
                    OnPropertyChanged();
                }
            }
        }

        public async Task CreateNewUser()
        {
            try
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    await LogLogin(false, Resources.MessageEmptyUserName);
                    throw new Exception(Resources.MessageEmptyUserName);
                }
                if (string.IsNullOrEmpty(Password))
                {
                    await LogLogin(false, Resources.MessageEmptyPassword);
                    throw new Exception(Resources.MessageEmptyPassword);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            var context = new DBContext();

            var user = new User()
            {
                UserName = UserName,
                Password = Password,
                Active = 1,
                CreateDate = DateTime.Now,
                CreatedBy = "Admin",
                LastUpdate = DateTime.Now,
                LastUpdateBy = "Admin"
            };

            try
            {
                context.Add(user);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            await TryLogin();
        }

        public async Task GenerateData()
        {
            var context = new DBContext();

            if (context.Country.Count() <= 0)
            {
                var sampleData = new SampleData();
                await sampleData.PopulateSampleData();
            }
        }

        public async Task LogLogin(bool success, string reason = null)
        {
            var message = new string("");
            if (success)
            {
                message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ff} - Success: {UserName}";
            }
            else if (!success)
            {
                message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ff} - Failure: {UserName} ," +
                    $" ErrorMessage: {reason}";
            }

            await File.AppendAllTextAsync(LogFile, message + Environment.NewLine);
        }

        public async Task TryLogin()
        {
            try
            {
                await ValidateLogin(UserName, Password);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            await LogLogin(true);

            foreach (Window window in Application.Current.Windows)
            {
                if (window.IsActive)
                {
                    loginWindow = window;
                }
            }

            await GenerateData();

            Window AppWindow = new ApplicationWindow();
            AppWindow.Show();
            loginWindow.Close();
            AppWindow.Focus();
        }

        public async Task ValidateLogin(string UserName, string Password)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                await LogLogin(false, Resources.MessageEmptyUserName);
                throw new Exception(Resources.MessageEmptyUserName);
            }
            if (string.IsNullOrEmpty(Password))
            {
                await LogLogin(false, Resources.MessageEmptyPassword);
                throw new Exception(Resources.MessageEmptyPassword);
            }

            //successful login - placed here to allow for faster login if no errors
            if (AllUsers.Exists(usr => usr.UserName == UserName && usr.Password == Password))
            {
                return;
            }

            if (!AllUsers.Exists(usr => usr.UserName == UserName))
            {
                await LogLogin(false, Resources.MessageUserDoesNotExist);
                throw new Exception(Resources.MessageUserDoesNotExist);
            }
            if (AllUsers.Exists(usr => usr.UserName == UserName && usr.Password != Password))
            {
                await LogLogin(false, Resources.MessageWrongPassword);
                throw new Exception(Resources.MessageWrongPassword);
            }
        }
    }
}