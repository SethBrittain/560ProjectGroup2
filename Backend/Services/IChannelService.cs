using pidgin.models;

namespace pidgin.services
{
    public interface IChannelService
    {
        Task<Channel> GetChannelById(int id);
		Task<List<Channel>> GetAllChannelsOfUser(int userId);     
        int CreateChannel(Channel channel);
        void UpdateChannel(Channel channel);
        void DeleteChannel(int id);
    }
}