public interface IRepository<K, T> where T: class
{
    List<T> GetAll();
    T? GetById(K id);
    void Add(T entity);
    void Update(T entity);
    void Delete(K id);
}
