using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.Core.Services.CrudServices.Base;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{    
    public class IncomeService : CrudServiceBase<Income, IncomeDto, CreateIncome, UpdateIncome>, IIncomeService
    {
        public IncomeService(IRepository<Income> repository, IMapper mapper) : base(repository, mapper) { }
    }
    
}