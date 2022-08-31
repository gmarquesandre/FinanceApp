using AutoMapper;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Implementations
{
    public class CreditCardService : CrudBase<CreditCard, CreditCardDto, CreateCreditCard, UpdateCreditCard>, ICreditCardService
    {
        public CreditCardService(IRepository<CreditCard> repository, IMapper mapper) : base(repository, mapper) { }
    }
}