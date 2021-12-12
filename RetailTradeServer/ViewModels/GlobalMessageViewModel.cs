using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class GlobalMessageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMessageStore _messageStore;
        private Visibility _messageBannerVisibility = Visibility.Hidden;

        #endregion

        #region Public Properties

        public string Message => _messageStore.CurrentMessage;
        public MessageType MessageType => _messageStore.CurrentMessageType;
        public Visibility MessageBannerVisibility
        {
            get => _messageBannerVisibility;
            set
            {
                _messageBannerVisibility = value;
                OnPropertyChanged(nameof(MessageBannerVisibility));
            }
        }

        #endregion

        #region Commands

        public ICommand CloseCommand { get; }

        #endregion

        #region Constructor

        public GlobalMessageViewModel(IMessageStore messageStore)
        {
            _messageStore = messageStore;

            CloseCommand = new RelayCommand(Close);

            _messageStore.CurrentMessageChanged += MessageStore_CurrentMessageChanged;
            _messageStore.CurrentMessageTypeChanged += MessageStore_CurrentMessageTypeChanged;
            _messageStore.CloseChanged += MessageStore_CloseChanged;
        }

        #endregion

        #region Private Voids

        private void Close()
        {
            MessageBannerVisibility = Visibility.Hidden;
        }

        private void MessageStore_CurrentMessageChanged()
        {
            MessageBannerVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(Message));
        }

        private void MessageStore_CurrentMessageTypeChanged()
        {
            OnPropertyChanged(nameof(MessageType));
        }

        private void MessageStore_CloseChanged()
        {
            MessageBannerVisibility = Visibility.Hidden;
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _messageStore.CurrentMessageChanged -= MessageStore_CurrentMessageChanged;
            _messageStore.CurrentMessageTypeChanged -= MessageStore_CurrentMessageTypeChanged;
            _messageStore.CloseChanged -= MessageStore_CloseChanged;
            base.Dispose();
        }

        #endregion
    }
}
