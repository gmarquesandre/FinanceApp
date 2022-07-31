using AutoMapper;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Base
{
    public class CrudServiceBase : ServiceBase
    {
        public UserContext _context;
        public IMapper _mapper;

        public CrudServiceBase(UserContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

    }
}
