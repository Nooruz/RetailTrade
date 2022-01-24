using DevExpress.Xpf.Editors;
using Microsoft.Win32;
using RetailTrade.SQLServerConnectionDialog.Commands;
using RetailTrade.SQLServerConnectionDialog.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;

namespace RetailTrade.SQLServerConnectionDialog.ViewModels
{
    public class ConnectionViewModel : BaseViewModel
    {
        #region Private Members

        private readonly Window _window;
        private bool _isTesting = false;
        private ObservableQueue<string> _dataBases = new();

        #endregion

        #region Public Properties

        public SqlConnectionStringBuilder ConnectionStringBuilder { get; private set; }
        public MessageBoxResult Result => IsConnectionValid ? MessageBoxResult.OK : MessageBoxResult.None;
        public bool IntegratedSecurity
        {
            get => ConnectionStringBuilder.IntegratedSecurity;
            set
            {
                if (value != ConnectionStringBuilder.IntegratedSecurity)
                {
                    ConnectionStringBuilder.IntegratedSecurity = value;
                    OnPropertyChanged(nameof(IntegratedSecurity));
                    OnPropertyChanged(nameof(UsernameEnabled));
                    OnPropertyChanged(nameof(AuthenticationMode));
                }                
            }
        }
        public string WindowUsername { get; set; }
        public string DataSource
        {
            get => ConnectionStringBuilder.DataSource;
            set
            {
                ConnectionStringBuilder.DataSource = value;
                OnPropertyChanged(nameof(DataSource));
                OnPropertyChanged(nameof(DataSourceValid));
                OnPropertyChanged(nameof(TestingEnabled));
            }
        }
        public string InitialCatalog
        {
            get => ConnectionStringBuilder.InitialCatalog;
            set
            {
                ConnectionStringBuilder.InitialCatalog = value;
                OnPropertyChanged(nameof(IsConnectionValid));
                OnPropertyChanged(nameof(Result));
            }
        }
        public bool IsConnectionValid => !string.IsNullOrEmpty(InitialCatalog);
        public bool DataSourceValid => !string.IsNullOrEmpty(DataSource);
        public string Username
        {
            get => IntegratedSecurity ? WindowUsername : ConnectionStringBuilder.UserID;
            set
            {
                ConnectionStringBuilder.UserID = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => IntegratedSecurity ? string.Empty : ConnectionStringBuilder.Password;
            set
            {
                ConnectionStringBuilder.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public bool UsernameEnabled => !IntegratedSecurity;
        public string AuthenticationMode
        {
            get => IntegratedSecurity ? Tags.WindowsAuthentication : Tags.SQLServerAuthentication;
            set
            {
                IntegratedSecurity = value == Tags.WindowsAuthentication;
            }
        }
        public bool IsTesting
        {
            get => _isTesting;
            private set
            {
                _isTesting = value;
                OnPropertyChanged(nameof(IsTesting));
                OnPropertyChanged(nameof(TestingEnabled));
            }
        }
        public bool TestingEnabled => !IsTesting;
        public List<string> AuthenticationModes => new() { Tags.WindowsAuthentication, Tags.SQLServerAuthentication };
        public IEnumerable<string> DataSources => ListLocalSqlInstances();
        public IEnumerable<string> DataBases => _dataBases;

        #endregion

        #region Commands

        public ICommand TestConnectionCommand { get; }
        public ICommand DataBaseNamePopupOpeningCommand { get; }
        public ICommand OkCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion

        #region Constructor

        public ConnectionViewModel(Window window)
        {
            _window = window;
            ConnectionStringBuilder = new();
            WindowUsername = WindowsIdentity.GetCurrent().Name;

            TestConnectionCommand = new RelayCommand(TestConnection);
            DataBaseNamePopupOpeningCommand = new ParameterCommand(p => DataBaseNamePopupOpening(p));
            CloseCommand = new RelayCommand(Close);
            OkCommand = new RelayCommand(Ok);
        }

        #endregion

        #region Private Voids

        private void Close()
        {
            InitialCatalog = string.Empty;
            _window?.Close();
        }

        private void Ok()
        {
            using SqlConnection connection = new(ConnectionStringBuilder.ConnectionString);
            try
            {
                IsTesting = true;
                connection.Open();
                _window.Close();
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message, "Настройка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.Close();
                IsTesting = false;
            }
        }

        private void DataBaseNamePopupOpening(object parameter)
        {
            if (parameter is OpenPopupEventArgs e)
            {
                e.Cancel = true;
                GetDatabaseList();
                e.Cancel = false;
            }
        }

        public void GetDatabaseList()
        {
            _dataBases.Clear();
            using SqlConnection connection = new(ConnectionStringBuilder.ConnectionString);
            try
            {
                IsTesting = true;
                connection.Open();
                using SqlCommand cmd = new("SELECT name from sys.databases", connection);
                IDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string dataBase = dr[0].ToString();
                    if (dataBase == "RetailTradeDb")
                    {
                        _dataBases.Enqueue(dataBase);
                    }
                }

                dr.Close();

                if (_dataBases.Count == 0)
                {
                    if (MessageBox.Show("Не удалось найти базу данных. Создать новую?", "Настройка подключения", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        SqlCommand createCommand = new("CREATE DATABASE RetailTradeDb", connection);
                        var result = createCommand.ExecuteNonQuery();
                        _ = MessageBox.Show("База данных успешно создана.", "Настройка подключения", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message, "Настройка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.Close();
                IsTesting = false;
            }
        }

        private async void TestConnection()
        {
            await using SqlConnection connection = new(ConnectionStringBuilder.ConnectionString);
            try
            {
                IsTesting = true;
                await connection.OpenAsync();
                _ = MessageBox.Show("Проверка подключения выполнена.", "Настройка подключения", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message, "Настройка подключения", MessageBoxButton.OK, MessageBoxImage.Error);                
            }
            finally
            {
                IsTesting = false;
            }
        }

        private IEnumerable<string> ListLocalSqlInstances()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }

                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                foreach (string item in ListLocalSqlInstances(Registry.LocalMachine))
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<string> ListLocalSqlInstances(RegistryKey hive)
        {
            const string keyName = @"Software\Microsoft\Microsoft SQL Server";
            const string valueName = "InstalledInstances";
            const string defaultName = "MSSQLSERVER";

            using (var key = hive.OpenSubKey(keyName, false))
            {
                if (key == null) return Enumerable.Empty<string>();

                var value = key.GetValue(valueName) as string[];
                if (value == null) return Enumerable.Empty<string>();

                for (int index = 0; index < value.Length; index++)
                {
                    if (string.Equals(value[index], defaultName, StringComparison.OrdinalIgnoreCase))
                    {
                        value[index] = ".";
                    }
                    else
                    {
                        value[index] = @".\" + value[index];
                    }
                }

                return value;
            }
        }

        #endregion       
    }

    public class Tags
    {
        public static readonly string DataSource = "DataSource";
        public static readonly string UserName = "UserName";
        public static readonly string Password = "Password";
        public static readonly string IntegratedSecurity = "IntegratedSecurity";
        public static readonly string DataSourceValid = "DataSourceValid";
        public static readonly string UserNameEnabled = "UserNameEnabled";
        public static readonly string WindowsAuthentication = "Windows Authentication";
        public static readonly string SQLServerAuthentication = "SQL Server Authentication";
        public static readonly string AuthenticationMode = "AuthenticationMode";
        public static readonly string TestingEnabled = "TestingEnabled";
        public static readonly string TestResult = "TestResult";
    }
}
