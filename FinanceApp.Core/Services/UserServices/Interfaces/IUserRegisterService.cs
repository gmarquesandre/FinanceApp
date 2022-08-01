using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FluentResults;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface IUserRegisterService 
    {
        Task<Result> UserRegister(CreateUsuarioDto createDto);
    }
}