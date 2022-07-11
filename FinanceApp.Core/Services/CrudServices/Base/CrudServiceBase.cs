using AutoMapper;
using FinanceApp.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace FinanceApp.Core.Services.CrudServices.Base
{
    public class CrudServiceBase : ServiceBase
    {
        public FinanceContext _context;
        public IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;

        public CrudServiceBase(FinanceContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

    }
}
