using AutoMapper;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Shared.Profiles
{
    public class CreditCardProfile : Profile
    {
        public CreditCardProfile()
        {
            CreateMap<CreateCreditCard, CreditCard>();
            CreateMap<UpdateCreditCard, CreditCard>();
            CreateMap<CreditCardDto, CreditCard>();
            CreateMap<CreditCard, CreditCardDto>();
            CreateMap<UpdateCreditCard, CreditCard>();
        }
    }
}