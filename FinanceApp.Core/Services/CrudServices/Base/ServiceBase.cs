using AutoMapper;
using FinanceApp.EntityFramework.Auth;

namespace FinanceApp.Core.Services.Base
{
    public class CrudServiceBase
    {
        public FinanceContext _context;
        public IMapper _mapper;

        public CrudServiceBase(FinanceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
