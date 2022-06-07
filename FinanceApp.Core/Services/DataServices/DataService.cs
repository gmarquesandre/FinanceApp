using AutoMapper;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FinanceApp.Core.Services.DataServices
{
    public class DataService : ServiceBase, IDataService
    {

        private FinanceContext _context;
        private IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public DataService(FinanceContext context, IMapper mapper, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache; 
        }


        public async Task<List<ProspectIndexValueDto>> GetIndexesProspect()
        {          
            var cacheKey = "prospectIndex";
            
            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<ProspectIndexValueDto> valuesDto))
            {
                //calling the server
                var values = await _context.ProspectIndexValues.ToListAsync();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromHours(1)                    
                };

                valuesDto = _mapper.Map<List<ProspectIndexValueDto>>(values);

                //setting cache entries
                _memoryCache.Set(cacheKey, valuesDto, cacheExpiryOptions);
            }
            //_context.ProspectIndexValues.Load();
            return valuesDto;
        }
    }
}
