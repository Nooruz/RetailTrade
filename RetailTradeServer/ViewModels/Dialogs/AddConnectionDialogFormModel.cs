using DevExpress.Xpf.Editors;
using Microsoft.Win32;
using Newtonsoft.Json;
using RetailTradeServer.Commands;
using RetailTradeServer.Properties;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class AddConnectionDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IUIManager _manager;
        private string _serverName;
        private string _username;
        private string _password;
        private string _dataBaseName;
        private ObservableCollection<string> _dataBases;
        private int _authenticationId;

        #endregion

        #region Public Properties

        public static IEnumerable<string> Servers => ListLocalSqlInstances();
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(CanConnection));
                OnPropertyChanged(nameof(ConnectionString));                
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanConnection));
                OnPropertyChanged(nameof(ConnectionString));
            }
        }
        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                OnPropertyChanged(nameof(ServerName));
                OnPropertyChanged(nameof(CanConnection));
                OnPropertyChanged(nameof(ConnectionString));
                OnPropertyChanged(nameof(CanLogin));
            }
        }
        public string DataBaseName
        {
            get => _dataBaseName;
            set
            {
                _dataBaseName = value;
                OnPropertyChanged(nameof(DataBaseName));
                OnPropertyChanged(nameof(CanCreateConnection));
                OnPropertyChanged(nameof(CanSelectDataBase));
            }
        }
        public bool CanConnection => !string.IsNullOrEmpty(ServerName) &&
            !string.IsNullOrEmpty(Username) &&
            !string.IsNullOrEmpty(Password);
        public int AuthenticationId
        {
            get => _authenticationId;
            set
            {
                _authenticationId = value;
                OnPropertyChanged(nameof(AuthenticationId));
                OnPropertyChanged(nameof(IsSQLServerAuthentication));
            }
        }
        public bool IsSQLServerAuthentication => AuthenticationId == 1;
        public bool CanLogin => !string.IsNullOrEmpty(ServerName);
        public bool CanCreateConnection => !string.IsNullOrEmpty(DataBaseName);
        public bool CanSelectDataBase => !string.IsNullOrEmpty(DataBaseName);
        public string ConnectionString => $"Server={ServerName};User Id={Username};Password={Password};";
        public ObservableCollection<string> DataBases
        {
            get => _dataBases ?? (new());
            set
            {
                _dataBases = value;
                OnPropertyChanged(nameof(DataBases));
            }
        }
        public SqlConnection SqlConnection { get; set; }
        public ComboBoxEdit ComboBoxEdit { get; set; }
        public Exception SQLExeption { get; set; }

        #endregion

        #region Commands

        public ICommand CheckConnectionCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand ComboBoxLoadedCommand { get; }

        #endregion

        #region Constructor

        public AddConnectionDialogFormModel(IUIManager manager)
        {
            _manager = manager;

            CheckConnectionCommand = new RelayCommand(CheckConnection);
            OkCommand = new RelayCommand(Ok);
            ComboBoxLoadedCommand = new ParameterCommand(parameter => ComboBoxLoaded(parameter));

            DataBases.CollectionChanged += DataBases_CollectionChanged;
        }

        #endregion

        #region Private Voids

        private void DataBases_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DataBases));
        }

        public static IEnumerable<string> ListLocalSqlInstances()
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

        private static IEnumerable<string> ListLocalSqlInstances(RegistryKey hive)
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

        private void CheckConnection()
        {
            if (CheckConnectionString(ConnectionString))
            {
                _manager.ShowMessage("Проверка подключения выполнена.", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (string.IsNullOrEmpty(ServerName))
            {
                _manager.ShowMessage("Не удается проверить это подключение, поскольку не указано имя сервера.", 
                    "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (string.IsNullOrEmpty(Username))
            {
                _manager.ShowMessage("Не удается проверить это подключение, поскольку не указано имя пользователя.",
                    "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (string.IsNullOrEmpty(Password))
            {
                _manager.ShowMessage("Не удается проверить это подключение, поскольку не указан пароль.",
                    "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _manager.ShowMessage(SQLExeption.Message,
                    "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Ok()
        {
            try
            {
                string appSettings = File.ReadAllText("appsettings.json");
                if (!string.IsNullOrEmpty(appSettings))
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject(appSettings);
                    jsonObj["ConnectionStrings"]["DefaultConnection"] = ConnectionString + $"Database={DataBaseName};";
                    string outAppSettings = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText("appsettings.json", outAppSettings);
                }
                Settings.Default.IsDataBaseConnectionAdded = true;
                Settings.Default.IsFirstLaunch = false;
                Settings.Default.Save();
                _manager.Close();
                System.Windows.Forms.Application.Restart();
            }
            catch (Exception)
            {
                //ignore
            }            
        }

        private void ComboBoxLoaded(object paramter)
        {
            if (paramter is RoutedEventArgs e)
            {
                if (e.Source is ComboBoxEdit comboBoxEdit)
                {
                    ComboBoxEdit = comboBoxEdit;
                    ComboBoxEdit.PopupOpening += ComboBoxEdit_PopupOpening;
                }
            }
        }

        private void ComboBoxEdit_PopupOpening(object sender, RoutedEventArgs e)
        {
            if (CanConnection)
            {
                if (CheckConnectionString(ConnectionString))
                {
                    ComboBoxEdit.Items.Clear();
                    using SqlCommand sqlCommand = new("Select name from master.sys.databases", SqlConnection);
                    using IDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        string dataBaseName = dataReader[0].ToString();
                        if (dataBaseName == "RetailTradeDb")
                        {
                            ComboBoxEdit.Items.Add(dataReader[0].ToString());
                        }                        
                    }

                    if (ComboBoxEdit.Items.Count == 0)
                    {
                        using SqlCommand checkUserSQLCommand = new($"SELECT [Name] FROM sys.sql_logins where [Name] = '{Username}';", SqlConnection);
                        using IDataReader checkUserDataReader = checkUserSQLCommand.ExecuteReader();                        
                    }

                }
                else
                {
                    if (CheckConnectionString($"Server={ServerName};Trusted_Connection=True;"))
                    {
                        using SqlCommand checkUserSQLCommand = new($"SELECT [Name] FROM sys.sql_logins where [Name] = '{Username}';", SqlConnection);
                        using IDataReader checkUserDataReader = checkUserSQLCommand.ExecuteReader();

                        string username = "";

                        while (checkUserDataReader.Read())
                        {
                            username = checkUserDataReader[0].ToString();
                        }

                        checkUserDataReader.Close();

                        if (string.IsNullOrEmpty(username))
                        {
                            try
                            {
                                using SqlCommand userCreateSQLCommand = new($"CREATE LOGIN {Username} WITH PASSWORD = '{Password}';", SqlConnection);
                                using IDataReader userCreateDataReader = userCreateSQLCommand.ExecuteReader();
                                userCreateDataReader.Close();

                                if (CheckConnectionString($"Server={ServerName};User Id={Username};Password={Password}"))
                                {

                                }
                                else
                                {
                                    _ = _manager.ShowMessage("Ошибка. Обратитесь к программистам.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                            }
                            catch (SqlException)
                            {
                                //ignore
                                throw;
                            }
                        }
                        else
                        {
                            _ = _manager.ShowMessage("Ошибка. Обратитесь к программистам.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            } 
            else if (!IsSQLServerAuthentication)
            {
                ComboBoxEdit.Items.Clear();
                using SqlCommand sqlCommand = new("Select name from master.sys.databases", SqlConnection);
                using IDataReader dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    string dataBaseName = dataReader[0].ToString();
                    if (dataBaseName == "RetailTradeDb")
                    {
                        ComboBoxEdit.Items.Add(dataReader[0].ToString());
                    }
                }

            }
        }

        private bool CheckConnectionString(string connectionString)
        {
            try
            {
                SqlConnection = new(connectionString);
                SqlConnection.Open();
                return true;
            }
            catch (SqlException e)
            {
                SQLExeption = e;
                return false;
            }
        }

        #endregion
    }
}
