using AutoMapper;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Models.CommonTables;
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


        public async Task<List<ProspectIndexValue>> GetIndexesProspect()
        {

            //_context.ProspectIndexValues.Load();

            return await _context.ProspectIndexValues.Where(a => a.BaseCalculo == 0).ToListAsync();
        }
    }
}
