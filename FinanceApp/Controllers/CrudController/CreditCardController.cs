using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto.CreditCard;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController
{
    [Route("[controller]")]
    [ApiController]
    public class CreditCardController : CrudControllerBase<ICreditCardService, CreditCardDto, CreateCreditCard, UpdateCreditCard>
    {

        public CreditCardController(ICreditCardService service) : base(service)
        {
        }
      
    }
}