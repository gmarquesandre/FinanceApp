using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;

namespace UsuariosApi.Profiles
{
    public class TreasuryBondProfile : Profile
    {
        public TreasuryBondProfile()
        {
            CreateMap<CreateIncome, TreasuryBond>();
            CreateMap<UpdateIncome, TreasuryBond>();
            CreateMap<IncomeDto, TreasuryBond>();
            CreateMap<TreasuryBond, IncomeDto>();
            CreateMap<UpdateIncome, TreasuryBond>();
        }
    }
}