using FinanceApp.Shared.Dto;
using FluentResults;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface IUserRegisterService : ITransientService
    {
        Task<Result> UserRegister(CreateUsuarioDto createDto);
    }
}