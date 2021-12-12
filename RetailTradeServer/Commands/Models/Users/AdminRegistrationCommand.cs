using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels;
using SalePageServer.Properties;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RetailTradeServer.Commands
{
    public class AdminRegistrationCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IRoleService _roleService;
        private readonly IAuthenticator _authenticator;
        private readonly IMessageStore _messageStore;
        private readonly IRenavigator _organizationRenavigator;
        private readonly RegistrationViewModel _viewModel;

        #endregion

        #region Constructor

        public AdminRegistrationCommand(IRoleService roleService,
            IRenavigator organizationRenavigator,
            IAuthenticator authenticator,
            RegistrationViewModel viewModel,
            IMessageStore messageStore)
        {
            _roleService = roleService;
            _authenticator = authenticator;
            _viewModel = viewModel;
            _messageStore = messageStore;
            _organizationRenavigator = organizationRenavigator;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return _viewModel.CanCreate
                && !string.IsNullOrEmpty(_viewModel.Password)
                && !string.IsNullOrEmpty(_viewModel.ConfirmPassword);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                //Администратор ролун алуу
                var role = await _roleService.GetFirstOrDefaultAsync();
                if (role != null)
                {
                    //Жаңы колдонуучуну
                    var user = new User
                    {
                        Username = _viewModel.Username,
                        JoinedDate = DateTime.Now,
                        RoleId = role.Id
                    };

                    var registrationResult = await _authenticator.Register(user, _viewModel.Password, _viewModel.ConfirmPassword);

                    switch (registrationResult)
                    {
                        case RegistrationResult.Success:
                            await _authenticator.Login(_viewModel.Username, _viewModel.ConfirmPassword);
                            Settings.Default.AdminCreated = true;
                            Settings.Default.Save();
                            _organizationRenavigator.Renavigate();
                            break;
                        case RegistrationResult.PasswordsDoNotMatch:
                            _messageStore.SetCurrentMessage("Пароль не совпадает с паролем подтверждения.", MessageType.Error);
                            break;
                        case RegistrationResult.UsernameAlreadyExists:
                            _messageStore.SetCurrentMessage("Имя пользователя уже существует.", MessageType.Error);
                            break;
                        case RegistrationResult.PasswordDoesNotRequirements:
                            _messageStore.SetCurrentMessage("Пароль должно соответствовать нижеследующим требованиям:\n" +
                                                      "1) \n" +
                                                      "2) \n" +
                                                      "3) ", MessageType.Error);
                            break;
                        case RegistrationResult.OtherError:
                            _messageStore.SetCurrentMessage("Не удалось создать администратора.", MessageType.Error);
                            break;
                        default:
                            _messageStore.SetCurrentMessage("Регистрация не удалась.", MessageType.Error);
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                _messageStore.SetCurrentMessage(e.Message, MessageType.Error);
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.CanCreate))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
