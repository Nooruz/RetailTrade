using System;
using System.Timers;

namespace RetailTradeServer.State.Messages
{
    public class MessageStore : IMessageStore
    {
        #region Private Members

        private string _currentMessage;
        private MessageType _currentMessageType;
        private Timer _timer = new Timer(5000);

        #endregion

        #region Public Properties

        public string CurrentMessage
        {
            get => _currentMessage;
            private set
            {
                _currentMessage = value;
                CurrentMessageChanged?.Invoke();
            }
        }
        public MessageType CurrentMessageType
        {
            get => _currentMessageType;
            private set
            {
                _currentMessageType = value;
                CurrentMessageTypeChanged?.Invoke();
            }
        }

        #endregion

        #region Event Actions

        public event Action CurrentMessageChanged;
        public event Action CurrentMessageTypeChanged;
        public event Action CloseChanged;

        #endregion

        #region Constructor

        public MessageStore()
        {
            _timer.Elapsed += (s, e) => Close();
        }

        #endregion

        #region Public Voids

        public void SetCurrentMessage(string message, MessageType messageType)
        {
            CurrentMessage = message;
            CurrentMessageType = messageType;
            _timer.Start();
        }

        public void Close()
        {
            CloseChanged?.Invoke();
        }

        #endregion        
    }
}
