using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class CategoryService : ICategoryService
    {
        private IRepository<Category> _repository;
        private IMapper _mapper;
        public CategoryService(IRepository<Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> AddAsync(CreateCategory input)
        {
            Category model = _mapper.Map<Category>(input);
            await _repository.InsertAsync(model);
            return _mapper.Map<CategoryDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateCategory input)
        {         
            var model = _mapper.Map<Category>(input);
            await _repository.UpdateAsync(input.Id, model);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }



        public async Task<List<CategoryDto>> GetAsync()
        {            
            var values = await _repository.GetAllListAsync();
            return _mapper.Map<List<CategoryDto>>(values);
        }

        public async Task<CategoryDto> GetAsync(int id)
        {
            var value = await _repository.FirstOrDefaultAsync();

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<CategoryDto>(value);

        }

        public async Task<Result> DeleteAsync(int id)
        {
            var investment = await _repository.GetByIdAsync(id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}