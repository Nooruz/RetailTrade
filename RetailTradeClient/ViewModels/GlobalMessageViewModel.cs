using RetailTradeClient.Commands;
using RetailTradeClient.State.Messages;
using RetailTradeClient.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels
{
    public class GlobalMessageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMessageStore _messageStore;
        private Visibility _messageBanerVisibility = Visibility.Hidden;

        #endregion

        #region Public Properties

        public string Message => _messageStore.CurrentMessage;
        public MessageType MessageType => _messageStore.CurrentMessageType;
        public Visibility MessageBanerVisibility
        {
            get => _messageBanerVisibility;
            set
            {
                _messageBanerVisibility = value;
                OnPropertyChanged(nameof(MessageBanerVisibility));
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
        }

        #endregion

        #region Private Voids

        private void Close()
        {
            MessageBanerVisibility = Visibility.Hidden;
        }

        private void MessageStore_CurrentMessageChanged()
        {
            MessageBanerVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(Message));
        }

        private void MessageStore_CurrentMessageTypeChanged()
        {
            OnPropertyChanged(nameof(MessageType));
        }

        #endregion
    }
}
