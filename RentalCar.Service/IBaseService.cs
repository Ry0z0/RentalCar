namespace RentalCar.Service
{
    public interface IBaseService<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        Task<int> AddAsync(TModel dto);
        Task<bool> UpdateAsync(TModel dto);
        bool Delete(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(TModel dto);
        Task<TModel?> GetByIdAsync(Guid id);
        Task<IEnumerable<TModel>> GetAllAsync();
    }
}
