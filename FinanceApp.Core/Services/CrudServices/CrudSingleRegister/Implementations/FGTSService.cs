using AutoMapper;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Implementations
{
    public class FGTSService : CrudSingleBase<FGTS, FGTSDto, CreateOrUpdateFGTS>, 
        IFGTSService
    {
      
        public FGTSService(IRepository<FGTS> repository, IMapper mapper) : base(repository, mapper)
        {
        }
      
        public override async Task<FGTSDto> GetAsync()
        {
            var value = await _repository.FirstOrDefaultAsync();
            if (value != null)
            {
                return _mapper.Map<FGTSDto>(value);
            }
            else
            {
                return new FGTSDto()
                {
                    AnniversaryWithdraw = false,
                    CurrentBalance = 0.00,
                    MonthlyGrossIncome = 0.00,
                    MonthAniversaryWithdraw = 1
                    
                };
            }
        }
    }
}