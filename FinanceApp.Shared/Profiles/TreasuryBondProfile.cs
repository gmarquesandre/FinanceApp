using AutoMapper;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Shared.Profiles
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