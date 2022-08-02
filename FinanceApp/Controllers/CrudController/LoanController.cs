using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto.Loan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController
{
    [Route("[controller]")]
    [ApiController]
    public class LoanController : CrudControllerBase<ILoanService, LoanDto, CreateLoan, UpdateLoan>
    {
        public LoanController(ILoanService service) : base(service) { }

    }
}