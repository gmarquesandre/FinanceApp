using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.Spending;
using Microsoft.EntityFrameworkCore;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.Core.Services.CrudServices.Base;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class SpendingService : CrudServiceBase<Spending, SpendingDto, CreateSpending, UpdateSpending>, ISpendingService
    {
        public SpendingService(IRepository<Spending> repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task<List<SpendingDto>> GetAsync()
        {
            var values = await _repository.GetAll()
                .Include(a => a.Category)
                .Include(a => a.CreditCard)                
                .ToListAsync();

            return _mapper.Map<List<SpendingDto>>(values);
        }

        public override async Task<SpendingDto> GetAsync(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return _mapper.Map<SpendingDto>(value);

        }      
    }
}