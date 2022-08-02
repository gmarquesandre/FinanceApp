using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto.TreasuryBond;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController
{
    [Route("[controller]")]
    [ApiController]
    public class TreasuryBondController : CrudControllerBase<ITreasuryBondService, TreasuryBondDto, CreateTreasuryBond, UpdateTreasuryBond>
    {

        public TreasuryBondController(ITreasuryBondService service) : base(service)
        {
        }

    }
}
