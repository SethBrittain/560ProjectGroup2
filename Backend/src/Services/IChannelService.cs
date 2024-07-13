using Pidgin.Modules.Channels;

namespace pidgin.services
{
    public interface IChannelService
    {
        Task<Channel> GetChannelById(int id);
		Task<IEnumerable<Channel>> GetAllChannelsOfUser(int userId);     
        int CreateChannel(Channel channel);
        void UpdateChannel(Channel channel);
        void DeleteChannel(int id);
    }
}