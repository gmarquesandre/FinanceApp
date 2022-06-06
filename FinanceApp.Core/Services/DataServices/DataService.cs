using AutoMapper;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.DataServices
{
    public class DataService : ServiceBase, IDataService
    {

        private FinanceContext _context;
        private IMapper _mapper;
        public DataService(FinanceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<ProspectIndexValueDto>> GetIndexesProspect()
        {

            //_context.ProspectIndexValues.Load();
            var values = await _context.ProspectIndexValues.Where(a => a.BaseCalculo == 0).ToListAsync();

            return _mapper.Map<List<ProspectIndexValueDto>>(values);
        }
    }
}
