﻿using AutoMapper;
using FinanceApp.EntityFramework;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
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
        public async Task<List<ProspectIndexValueDto>> GetIndexProspect(EIndex index)
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.TreasuryBond)+index.ToString();

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<ProspectIndexValueDto> valuesDto))
            {
                //calling the server
                var values = await _context.ProspectIndexValues.Where(a => a.Index == index).ToListAsync();

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
        public async Task<List<IndexValueDto>> GetIndex(EIndex index, DateTime dateStart)
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.IndexesProspect) + index.ToString();

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<IndexValueDto> valuesDto))
            {
                //calling the server
                var values = await _context.IndexValues.Where(a=> a.Index == index).ToListAsync();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                valuesDto = _mapper.Map<List<IndexValueDto>>(values);

                //setting cache entries
                _memoryCache.Set(cacheKey, valuesDto, cacheExpiryOptions);
            }
            var returnList = valuesDto.Where(a => a.Date >= dateStart).ToList();
            //_context.ProspectIndexValues.Load();
            return returnList;
        }
        public async Task<List<TreasuryBondValue>> GetTreasuryBondLastValue()
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.TreasuryBond);

            //Converter em Dto
            if (!_memoryCache.TryGetValue(cacheKey, out List<TreasuryBondValue> valuesDto))
            {
                //calling the server
                DateTime maxDate = await _context.TreasuryBondValues.MaxAsync(a => a.Date);
                var values = await _context.TreasuryBondValues.Where(a=> a.Date == maxDate).ToListAsync();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                //valuesDto = _mapper.Map<List<IndexValueDto>>(values);

                valuesDto = values;
                //setting cache entries
                _memoryCache.Set(cacheKey, valuesDto, cacheExpiryOptions);
            }
            var returnList = valuesDto;
            //_context.ProspectIndexValues.Load();
            return returnList;
        }
    }
}
