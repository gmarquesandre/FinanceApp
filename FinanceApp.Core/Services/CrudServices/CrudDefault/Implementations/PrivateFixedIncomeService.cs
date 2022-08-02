using AutoMapper;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Implementations
{
    public class PrivateFixedIncomeService : CrudBase<PrivateFixedIncome, PrivateFixedIncomeDto, CreatePrivateFixedIncome, UpdatePrivateFixedIncome>, IPrivateFixedIncomeService
    {
        public PrivateFixedIncomeService(IRepository<PrivateFixedIncome> repository, IMapper mapper) : base(repository, mapper) { }
    }
}