using pidgin.models;
using Pidgin.Util;

namespace pidgin.services
{
    public interface IMessageService
    {
        Task<Message> GetMessageById(int id);  
        Task<List<object>> GetAllChannelMessages(int channelId, int uid, int limit = 100);  
        int CreateMessage(Message message);
        void UpdateMessage(Message message);
        void DeleteMessage(int id);
        Task<SendableMessage> CreateChannelMessageReturningSendable(Message m);
    }
}