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
        private readonly IUserPointSaleService _userPointSaleService;
        private PointSale _createdPointSale = new();
        private IEnumerable<WareHouse> _wareHouses;
        private IEnumerable<User> _users;
        private List<User> _selectedUsers;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public PointSale CreatedPoitnSale
        {
            get => _createdPointSale;
            set
            {
                _createdPointSale = value;
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
        public List<User> SelectedUsers
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
            IUserService userService,
            IUserPointSaleService userPointSaleService)
        {
            _pointSaleService = pointSaleService;
            _messageStore = messageStore;
            _wareHouseService = wareHouseService;
            _userService = userService;
            _userPointSaleService = userPointSaleService;

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
                    CreatedPoitnSale.UserPointSale.ForEach(u =>
                    {
                        u.User = null;
                    });
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
                    if (await _pointSaleService.UpdateAsync(CreatedPoitnSale.Id, CreatedPoitnSale) != null)
                    {
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
