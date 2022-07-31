using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Entities.CommonTables;
using FinanceApp.Shared.Entities.UserTables;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Shared.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUsuarioDto, Usuario>();
            CreateMap<Usuario, IdentityUser<int>>();
            CreateMap<Usuario, CustomIdentityUser>();
        }
    }
}