using pidgin.models;

namespace pidgin.services;

public interface IOrganizationService {
	Task<Organization> GetOrganizationById(long id);
	Task<List<User>> GetAllUsersInOrganization(Organization org, int limit=10);
}