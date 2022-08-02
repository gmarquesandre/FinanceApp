using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto.FGTS;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces
{
    public interface IFGTSService : ICrudSingleBase<FGTSDto, CreateOrUpdateFGTS>
    {
    }
}