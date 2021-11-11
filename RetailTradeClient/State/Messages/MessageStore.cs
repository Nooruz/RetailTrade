using System;

namespace RetailTradeClient.State.Messages
{
    public class MessageStore : IMessageStore
    {
        #region Private Members

        private string _currentMessage;
        private MessageType _currentMessageType;

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

        #endregion

        #region Public Voids

        public void SetCurrentMessage(string message, MessageType messageType)
        {
            CurrentMessage = message;
            CurrentMessageType = messageType;
        }

        #endregion        
    }
}
