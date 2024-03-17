using pidgin.models;

namespace pidgin.services
{
    public interface IMessageService
    {
        Task<Message> GetMessageById(int id);  
        Task<List<Message>> GetMessagesByChannelId(int channelId, int limit = 100);  
        int CreateMessage(Message message);
        void UpdateMessage(Message message);
        void DeleteMessage(int id);
    }
}