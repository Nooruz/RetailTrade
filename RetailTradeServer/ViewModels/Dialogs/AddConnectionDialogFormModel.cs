﻿using DevExpress.Xpf.Editors;
using Microsoft.Win32;
using Newtonsoft.Json;
using RetailTrade.EntityFramework;
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
        private readonly RetailTradeDbContextFactory _contextFactory;
        private string _serverName;
        private ObservableCollection<string> _dataBases;
        private bool _canCreateConnection;

        #endregion

        #region Public Properties

        public static IEnumerable<string> Servers => ListLocalSqlInstances();
        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                OnPropertyChanged(nameof(ServerName));
                OnPropertyChanged(nameof(CanCreateDataBase));
                OnPropertyChanged(nameof(ConnectionString));
            }
        }
        public bool CanCreateDataBase => !string.IsNullOrEmpty(ServerName) && !CanCreateConnection;
        public bool CanCreateConnection
        {
            get => _canCreateConnection;
            set
            {
                _canCreateConnection = value;
                OnPropertyChanged(nameof(CanCreateConnection));
                OnPropertyChanged(nameof(CanCreateDataBase));
            }
        }
        public string ConnectionString => $"Server={ServerName};Trusted_Connection=True;";
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
        public ICommand CreateDataBaseCommand { get; }

        #endregion

        #region Constructor

        public AddConnectionDialogFormModel(IUIManager manager,
            RetailTradeDbContextFactory contextFactory)
        {
            _manager = manager;
            _contextFactory = contextFactory;

            CheckConnectionCommand = new RelayCommand(CheckConnection);
            OkCommand = new RelayCommand(Ok);
            CreateDataBaseCommand = new RelayCommand(CreateDataBase);

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
               _ = _manager.ShowMessage("Проверка подключения выполнена.", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (string.IsNullOrEmpty(ServerName))
            {
                _ = _manager.ShowMessage("Не удается проверить это подключение, поскольку не указано имя сервера.", 
                    "", MessageBoxButton.OK, MessageBoxImage.Information);
            }            
            else
            {
                _ = _manager.ShowMessage(SQLExeption.Message,
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
                    jsonObj["ConnectionStrings"]["DefaultConnection"] = ConnectionString + $"Database=RetailTradeDb;";
                    string outAppSettings = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText("appsettings.json", outAppSettings);
                }
                Settings.Default.IsDataBaseConnectionAdded = true;
                Settings.Default.IsFirstLaunch = false;
                Settings.Default.Save();
                _manager.Close();
            }
            catch (Exception)
            {
                //ignore
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

        private void CreateDataBase()
        {
            if (CheckConnectionString(ConnectionString + "Integrated security=SSPI;database=master;"))
            {
                SqlCommand createDBSQLCommand = new("CREATE DATABASE RetailTradeDb", SqlConnection);

                try
                {
                    createDBSQLCommand.ExecuteNonQuery();
                    _ = _manager.ShowMessage("База данных успешно создана.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    CanCreateConnection = true;
                }
                catch (SqlException e)
                {
                    _ = _manager.ShowMessage(e.Message, "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                finally
                {
                    if (SqlConnection.State == ConnectionState.Open)
                    {
                        SqlConnection.Close();
                    }
                }
            }
        }

        #endregion
    }
}