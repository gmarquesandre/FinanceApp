using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.Core.Services.CrudServices.Base;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class PrivateFixedIncomeService : CrudServiceBase<PrivateFixedIncome, PrivateFixedIncomeDto, CreatePrivateFixedIncome, UpdatePrivateFixedIncome>, IPrivateFixedIncomeService
    {
        public PrivateFixedIncomeService(IRepository<PrivateFixedIncome> repository, IMapper mapper) : base(repository, mapper) { }
    }
}