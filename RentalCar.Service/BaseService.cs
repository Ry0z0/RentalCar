using AutoMapper;
using Microsoft.Extensions.Logging;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Service
{
    public class BaseService<TEntity, TModel> : IBaseService<TEntity, TModel>
            where TEntity : class
            where TModel : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        public BaseService(IUnitOfWork unitOfWork, ILogger<BaseService<TEntity, TModel>> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.BaseRepository<TEntity>();
            _mapper = mapper;
            _logger = logger;
        }

        public virtual async Task<int> AddAsync(TModel dto)
        {
            _logger.LogInformation("Adding a new entity.");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<TEntity>(dto);

                var idProperty = typeof(TEntity).GetProperty("Id");
                if (idProperty != null)
                {
                    idProperty.SetValue(entity, Guid.NewGuid());
                }

                _repository.Add(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Entity added successfully.");
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error adding entity.");
                throw;
            }
        }


        public virtual async Task<bool> UpdateAsync(TModel dto)
        {
            _logger.LogInformation("Updating an entity.");
            //await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                _repository.Detach(entity);
                _repository.Update(entity);
                var result = await _unitOfWork.SaveChangesAsync() > 0;
                //await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Entity updated successfully.");
                return result;
            }
            catch (Exception ex)
            {
                //await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating entity.");
                throw;
            }
        }

        public virtual bool Delete(Guid id)
        {
            _logger.LogInformation("Deleting an entity with ID: {Id}", id);
            try
            {
                _repository.Delete(id);
                var result = _unitOfWork.SaveChangesAsync().Result > 0;
                _logger.LogInformation("Entity deleted successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity.");
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting an entity asynchronously with ID: {Id}", id);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _repository.Delete(id);
                var result = await _unitOfWork.SaveChangesAsync() > 0;
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Entity deleted successfully.");
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting entity.");
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(TModel dto)
        {
            _logger.LogInformation("Deleting an entity asynchronously.");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                _repository.Delete(entity);
                var result = await _unitOfWork.SaveChangesAsync() > 0;
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Entity deleted successfully.");
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting entity.");
                throw;
            }
        }

        public virtual async Task<TModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching entity by ID: {Id}", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Entity with ID: {Id} not found.", id);
                return null;
            }

            _logger.LogInformation("Entity with ID: {Id} fetched successfully.", id);
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all entities.");
            var entities = await _repository.GetAllAsync();
            _logger.LogInformation("{Count} entities fetched successfully.", entities.Count());
            return _mapper.Map<IEnumerable<TModel>>(entities);
        }

    }
}
