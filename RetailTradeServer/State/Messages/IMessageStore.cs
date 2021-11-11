using System;
using System.Windows;

namespace RetailTradeServer.State.Messages
{
    public enum MessageType
    {
        Status,
        Success,
        Error
    }
    public interface IMessageStore
    {
        string CurrentMessage { get; }
        void Close();
        MessageType CurrentMessageType { get; }

        void SetCurrentMessage(string message, MessageType messageType);
        event Action CurrentMessageChanged;
        event Action CurrentMessageTypeChanged;
        event Action CloseChanged;
    }
}
