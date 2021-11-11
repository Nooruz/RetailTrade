using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        private string _message;
        private string _color = "#b50939";

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
                OnPropertyChanged(nameof(HasMessage));
            }
        }

        public string MessageColor
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        public bool HasMessage => !string.IsNullOrEmpty(Message);
    }
}
