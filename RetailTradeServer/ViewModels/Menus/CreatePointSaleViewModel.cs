using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CreatePointSaleViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IPointSaleService _pointSaleService;
        private readonly IMessageStore _messageStore;
        private readonly IWareHouseService _wareHouseService;
        private readonly IUserService _userService;
        private PointSale _createdPointSale = new();
        private IEnumerable<WareHouse> _wareHouses;
        private IEnumerable<User> _users;
        private List<object> _selectedUsers;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public PointSale CreatedPoitnSale
        {
            get => _createdPointSale;
            set
            {
                _createdPointSale = value;
                if (_createdPointSale.Users != null && _createdPointSale.Users.Any())
                {
                    SelectedUsers = _createdPointSale.Users.Select(u => u.Id).Cast<object>().ToList();
                }
                Header = $"Точка продаж ({_createdPointSale.Name})";
                OnPropertyChanged(nameof(CreatedPoitnSale));
            }
        }
        public IEnumerable<WareHouse> WareHouses
        {
            get => _wareHouses;
            set
            {
                _wareHouses = value;
                OnPropertyChanged(nameof(WareHouses));
            }
        }
        public IEnumerable<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public List<object> SelectedUsers
        {
            get => _selectedUsers;
            set
            {
                _selectedUsers = value;
                OnPropertyChanged(nameof(SelectedUsers));
            }
        }

        #endregion

        #region Constructor

        public CreatePointSaleViewModel(IPointSaleService pointSaleService,
            IMessageStore messageStore,
            IWareHouseService wareHouseService,
            IUserService userService)
        {
            _pointSaleService = pointSaleService;
            _messageStore = messageStore;
            _wareHouseService = wareHouseService;
            _userService = userService;

            GlobalMessageViewModel = new(_messageStore);

            Header = "Точка продаж (создание)";

            LoadingData();
        }

        #endregion

        #region Private Voids

        private async void LoadingData()
        {
            Users = await _userService.GetCashiersAsync();
            WareHouses = await _wareHouseService.GetAllAsync();
        }

        #endregion

        #region Public Voids

        [Command]
        public async void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(CreatedPoitnSale.Name))
                {
                    _messageStore.SetCurrentMessage("Введите наименование.", MessageType.Error);
                    return;
                }
                if (CreatedPoitnSale.WareHouseId == 0)
                {
                    _messageStore.SetCurrentMessage("Выберите склад.", MessageType.Error);
                    return;
                }
                if (CreatedPoitnSale.Id == 0)
                {
                    if (SelectedUsers != null && SelectedUsers.Any())
                    {
                        CreatedPoitnSale.UserPointSale = SelectedUsers.Select(u => new UserPointSale { UserId = (int)u }).ToList();
                    }
                    PointSale result = await _pointSaleService.CreateAsync(CreatedPoitnSale);
                    if (result != null)
                    {
                        //if (SelectedUsers != null && SelectedUsers.Any())
                        //{
                        //    if (await _userPointSaleService.AddRangeAsync(SelectedUsers.Select(u => new UserPointSale { UserId = u.Id, PointSaleId = result.Id }).ToList()))
                        //    {
                        //        _messageStore.SetCurrentMessage("Точка продаж создан.", MessageType.Success);
                        //        Header = $"Точка продаж ({CreatedPoitnSale.Name})";
                        //    }
                        //    else
                        //    {
                        //        _messageStore.SetCurrentMessage("Точка продаж создан, но привязать выбранных кассиров не удалось.\nСвяжитесь с разработчиками.", MessageType.Error);
                        //    }
                        //}
                        //else
                        //{
                        //    _messageStore.SetCurrentMessage("Точка продаж создан.", MessageType.Success);
                        //    Header = $"Точка продаж ({CreatedPoitnSale.Name})";
                        //}
                        _messageStore.SetCurrentMessage("Точка продаж создан.", MessageType.Success);
                        Header = $"Точка продаж ({CreatedPoitnSale.Name})";
                    }
                    else
                    {
                        _messageStore.SetCurrentMessage("Не удалось создать точку продаж.", MessageType.Error);
                    }
                }
                else
                {
                    if (SelectedUsers != null && SelectedUsers.Any())
                    {
                        CreatedPoitnSale.UserPointSale = SelectedUsers.Select(u => new UserPointSale { UserId = (int)u, PointSaleId = CreatedPoitnSale.Id }).ToList();
                    }
                    PointSale updatedPointSale = await _pointSaleService.UpdateAsync(CreatedPoitnSale.Id, CreatedPoitnSale);
                    if (updatedPointSale != null)
                    {
                        CreatedPoitnSale = updatedPointSale;
                        _messageStore.SetCurrentMessage("Точка продаж сохранен.", MessageType.Success);
                        Header = $"Точка продаж ({CreatedPoitnSale.Name})";
                    }
                    else
                    {
                        _messageStore.SetCurrentMessage("Не удалось сохранить.", MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                _messageStore.SetCurrentMessage(ex.Message, MessageType.Error);
            }
        }

        [Command]
        public void UserControlLoaded()
        {

        }

        #endregion
    }
}
