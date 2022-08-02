using FinanceApp.Shared.Dto.Base;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base
{
    public interface ICommandSingle<TObjectDto, TOperationObject>
        where TObjectDto : StandardDto
        where TOperationObject : CreateOrUpdateDto
    {
        Task<TObjectDto> AddOrUpdateAsync(TOperationObject input);
        Task<Result> DeleteAsync();
    }
}
