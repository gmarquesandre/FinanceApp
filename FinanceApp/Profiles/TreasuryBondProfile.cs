using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;

namespace UsuariosApi.Profiles
{
    public class TreasuryBondProfile : Profile
    {
        public TreasuryBondProfile()
        {
            CreateMap<CreateTreasuryBond, TreasuryBond>();
            CreateMap<UpdateTreasuryBond, TreasuryBond>();
            CreateMap<TreasuryBondDto, TreasuryBond>();
            CreateMap<TreasuryBond, TreasuryBondDto>();
            CreateMap<UpdateTreasuryBond, TreasuryBond>();
        }
    }
}