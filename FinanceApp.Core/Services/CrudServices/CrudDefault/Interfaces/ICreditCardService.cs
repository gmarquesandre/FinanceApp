using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.CreditCard;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface ICreditCardService: ICrudBase<CreditCardDto, CreateCreditCard, UpdateCreditCard>        
    {
    }
}