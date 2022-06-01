using AutoMapper;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Api.Profiles
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<CreateLoan, Loan>();
            CreateMap<UpdateLoan, Loan>();
            CreateMap<LoanDto, Loan>();
            CreateMap<Loan, LoanDto>();
            CreateMap<UpdateLoan, Loan>();
        }
    }
}