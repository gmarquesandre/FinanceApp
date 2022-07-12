using AutoMapper;
using FinanceApp.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace FinanceApp.Core.Services.CrudServices.Base
{
    public class CrudServiceBase : ServiceBase
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
