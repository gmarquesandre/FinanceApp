using AutoMapper;
using FinanceApp.Shared.Dto.CurrentBalance;
using FluentResults;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Implementations
{
    public class CurrentBalanceService : CrudSingleBase<CurrentBalance, CurrentBalanceDto, CreateOrUpdateCurrentBalance>, ICurrentBalanceService
    {
        public CurrentBalanceService(IMapper mapper, IRepository<CurrentBalance> repository) : base(repository, mapper)
        {
        }
        
        public override async Task<CurrentBalanceDto> GetAsync()
        {

            var value = await _repository.FirstOrDefaultAsync();
            if (value != null)
            {
                return _mapper.Map<CurrentBalanceDto>(value);
            }
            else
            {
                return new CurrentBalanceDto()
                {
                    Id = 0,
                    PercentageCdi = null,
                    UpdateValueWithCdiIndex = false,
                    Value = 0.00,
                    UpdateDateTime = new DateTime(1900, 1, 1),

                };
            }
        }
    }
}