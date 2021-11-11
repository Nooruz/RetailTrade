using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels;
using RetailTradeServer.ViewModels.Factories;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailTradeServer.Commands
{
    public class AdminRegistrationCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IRoleService _roleService;
        private readonly IAuthenticator _authenticator;
        private readonly RegistrationViewModel _viewModel;
        private PasswordBox _password;
        private PasswordBox _confirmPassword;

        #endregion

        #region Commands

        public ICommand UpdateCurrentViewModelCommand { get; }

        #endregion

        #region Constructor

        public AdminRegistrationCommand(IRoleService roleService,
            INavigator navigator,
            IAuthenticator authenticator,
            RegistrationViewModel viewModel,
            IRetailTradeViewModelFactory viewModelFactory)
        {
            _roleService = roleService;
            _authenticator = authenticator;
            _viewModel = viewModel;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, viewModelFactory);
        }

        #endregion        

        public override bool CanExecute(object parameter)
        {
            var objects = (object[])parameter;

            if (objects[0] is PasswordBox password)
            {
                _password = password;
                _password.PasswordChanged += Password_PasswordChanged;
            }
            else
            {
                return false;
            }

            if (objects[1] is PasswordBox confirmPassword)
            {
                _confirmPassword = confirmPassword;
                _confirmPassword.PasswordChanged += ConfirmPassword_PasswordChanged;
            }
            else
            {
                return false;
            }

            return _viewModel.CanCreate
                && !string.IsNullOrEmpty(_password.Password)
                && !string.IsNullOrEmpty(_confirmPassword.Password);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _viewModel.ErrorMessage = string.Empty;

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

                    var registrationResult = await _authenticator.Register(user, _password.Password, _confirmPassword.Password);

                    switch (registrationResult)
                    {
                        case RegistrationResult.Success:
                            await _authenticator.Login(_viewModel.Username, _password.Password);
                            UpdateCurrentViewModelCommand.Execute(ViewType.Organization);
                            Properties.Settings.Default.AdminCreated = true;
                            Properties.Settings.Default.Save();
                            break;
                        case RegistrationResult.PasswordsDoNotMatch:
                            _viewModel.ErrorMessage = "Пароль не совпадает с паролем подтверждения.";
                            break;
                        case RegistrationResult.UsernameAlreadyExists:
                            _viewModel.ErrorMessage = "Имя пользователя уже существует.";
                            break;
                        case RegistrationResult.UsernameDoesNotRequirements:
                            _viewModel.ErrorMessage = "Имя пользователя должно соответствовать нижеследующим требованиям:\n" +
                                                      "1) Должно содержать только латинские буквы (a-z) и цифры (0-9).\n" +
                                                      "2) Должно начинатся с буквы.\n" +
                                                      "3) Длина должно быть от 3 до 10 символов.";
                            break;
                        case RegistrationResult.PasswordDoesNotRequirements:
                            _viewModel.ErrorMessage = "Пароль должно соответствовать нижеследующим требованиям:\n" +
                                                      "1) \n" +
                                                      "2) \n" +
                                                      "3) ";
                            break;
                        case RegistrationResult.OtherError:
                            _viewModel.ErrorMessage = "Не удалось создать администратора.";
                            break;
                        default:
                            _viewModel.ErrorMessage = "Регистрация не удалась.";
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                _viewModel.ErrorMessage = e.Message;
            }
        }

        private void ConfirmPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        private void Password_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
