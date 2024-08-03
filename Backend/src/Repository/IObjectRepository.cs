namespace Pidgin.Repository;

public interface IObjectRepository<T> {
	public Task<int> Create(T obj);
	public Task<T> Get(int id, int uid);
	public Task Update(T obj, int uid);
	public Task Delete(int id, int uid);
	public Task<IEnumerable<T>> GetAll(int uid);
}