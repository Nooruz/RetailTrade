using System;

namespace RetailTradeClient.State.Messages
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
        MessageType CurrentMessageType { get; }

        void SetCurrentMessage(string message, MessageType messageType);
        event Action CurrentMessageChanged;
        event Action CurrentMessageTypeChanged;
    }
}
