using AutoMapper;
using FinanceApp.Core.Services.DataServices;
using FinanceApp.EntityFramework;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FinanceApp.Core.Services
{
    public class IndexService : ServiceBase, IIndexService
    {

        private FinanceContext _context;
        private IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public readonly IDatesService _datesService;

        public IndexService(FinanceContext context, 
            IMapper mapper, 
            IMemoryCache memoryCache, 
            IDatesService datesService)
        {
            _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _datesService = datesService;
        }


        private readonly List<Iof> IofValues = new() {
            new Iof(0,1.00),
            new Iof(1, 0.96),
            new Iof(2, 0.93),
            new Iof(3, 0.9),
            new Iof(4, 0.86),
            new Iof(5, 0.83),
            new Iof(6, 0.8),
            new Iof(7, 0.76),
            new Iof(8, 0.73),
            new Iof(9, 0.7),
            new Iof(10, 0.66),
            new Iof(11, 0.63),
            new Iof(12, 0.6),
            new Iof(13, 0.56),
            new Iof(14, 0.53),
            new Iof(15, 0.5),
            new Iof(16, 0.46),
            new Iof(17, 0.43),
            new Iof(18, 0.4),
            new Iof(19, 0.36),
            new Iof(20, 0.33),
            new Iof(21, 0.3),
            new Iof(22, 0.26),
            new Iof(23, 0.23),
            new Iof(24, 0.2),
            new Iof(25, 0.16),
            new Iof(26, 0.13),
            new Iof(27, 0.1),
            new Iof(28, 0.06),
            new Iof(29, 0.03)

        };
        public double GetIof(int day)
        {
            var item = IofValues.FirstOrDefault(x => x.Day == day);

            if (item == null)
                return 0;

            return item.Value;
        }
        public async Task<List<ProspectIndexValueDto>> GetIndexProspect(EIndex index)
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.TreasuryBond) + index.ToString();

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<ProspectIndexValueDto> valuesDto))
            {
                //calling the server
                var values = await _context.ProspectIndexValues.Where(a => a.Index == index && a.BaseCalculo == 0).ToListAsync();

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
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.Indexes) + index.ToString();

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<IndexValueDto> valuesDto))
            {
                List<IndexValue> values = new();

                if (index == EIndex.TR)
                    values = await _context.IndexValues.Where(a => a.Index == index && a.Date.Day == 1 && a.DateEnd.Day == 1).ToListAsync();
                else
                    values = await _context.IndexValues.Where(a => a.Index == index).ToListAsync();

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
        public async Task<IndexValueDto> GetIndexLastValue(EIndex index)
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.IndexesLastValue) + index.ToString();

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out IndexValueDto valueDto))
            {
                IndexValue value = new();

                if (index == EIndex.TR)
                {
                    DateTime maxDate = _context.IndexValues.Where(a=> a.Date.Day == 1).Max(a => a.Date);
                    value = await _context.IndexValues.FirstOrDefaultAsync(a => a.Index == index && a.Date.Day == 1 && a.DateEnd.Day == 1);
                }
                else
                {

                    DateTime maxDate = _context.IndexValues.Max(a => a.Date);
                    value = await _context.IndexValues.FirstOrDefaultAsync(a => a.Index == index && a.Date == maxDate);
                }

                if (value == null)
                    throw new Exception($"Erro ao buscar ultimo valor do indice {index}");

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                valueDto = _mapper.Map<IndexValueDto>(value);

                //setting cache entries
                _memoryCache.Set(cacheKey, valueDto, cacheExpiryOptions);
            }

            //_context.ProspectIndexValues.Load();
            return valueDto;
        }
        public async Task<List<IndexValueDailyWithProspect>> GetIndexProspectDaily(EIndex index, DateTime startDate, DateTime endDate)
        {
            if (index == EIndex.TR)
                return new List<IndexValueDailyWithProspect>();

            return new List<IndexValueDailyWithProspect>();

            List<IndexValueDailyWithProspect> returnList = new();

            var indexRealValues = await GetIndex(index, startDate);
            var indexLastValue = await GetIndexLastValue(index);
            
            DateTime maxDateRealIndex = indexLastValue.Date;

            indexRealValues.OrderBy(a => a.Date);

            returnList.AddRange(indexRealValues.Where(a => a.Date >= startDate && a.Date <= endDate).Select(a =>
            new IndexValueDailyWithProspect{
                Date = a.Date,
                
            }).ToList());

            var prospects = await GetIndexProspect(index);



            prospects.ForEach(async prospect =>
            {
                //while (date <= endDate)
                //{
                //    if (date <= maxDateRealIndex);

                }
                //    bool isHoliday = await IsHoliday(date);

                //    if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                //        return;
                //    else if (isHoliday)
                //        return;



                //    prospectDaily.Add(new IndexValueDailyWithProspect()
                //    {
                //        Date = date,
                //        Index = index,
                //        IsProspect = true,
                //        Value = valueDaily
                //    });

                //    date.AddDays(1);
                //}

            });

            //for (DateTime date = DateTime.Now.Date; date <= endDate; date = date = date.AddMonths(1))
            //{

            //    if (periodValue == null)
            //    {
            //        periodValue == prospects.Where()
            //    }

            //    ConvertIndexToDaily(date, periodValue.Median);


            //}


        }
        
        
        //public async Task<List<TreasuryBondValue>> GetTreasuryBondLastValue()
        //{
        //    var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.TreasuryBond);

        //    //Converter em Dto
        //    if (!_memoryCache.TryGetValue(cacheKey, out List<TreasuryBondValue> valuesDto))
        //    {
        //        //calling the server
        //        DateTime maxDate = await _context.TreasuryBondValues.MaxAsync(a => a.Date);
        //        var values = await _context.TreasuryBondValues.Where(a => a.Date == maxDate).ToListAsync();

        //        //setting up cache options
        //        var cacheExpiryOptions = new MemoryCacheEntryOptions
        //        {
        //            AbsoluteExpiration = DateTime.Now.AddHours(1),
        //            Priority = CacheItemPriority.High,
        //            SlidingExpiration = TimeSpan.FromHours(1)
        //        };

        //        //valuesDto = _mapper.Map<List<IndexValueDto>>(values);

        //        valuesDto = values;
        //        //setting cache entries
        //        _memoryCache.Set(cacheKey, valuesDto, cacheExpiryOptions);
        //    }
        //    var returnList = valuesDto;
        //    //_context.ProspectIndexValues.Load();
        //    return returnList;
        //}
    }
}
