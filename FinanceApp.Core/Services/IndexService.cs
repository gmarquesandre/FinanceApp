using AutoMapper;
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
            new Iof(0, 1.00),
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
            return valuesDto;
        }
        public async Task<List<IndexValueDto>> GetIndex(EIndex index, DateTime dateStart, DateTime? dateEnd = null)
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.Indexes) + index.ToString();

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<IndexValueDto> valuesDto))
            {
                |List<IndexValue> values = new();

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

            if (dateEnd.HasValue)
            {

                var returnValues = valuesDto.Where(a => a.Date >= dateStart && a.Date <= dateEnd).ToList();
                return returnValues;
            }

            return valuesDto.Where(a => a.Date >= dateStart).ToList();

        }
        private async Task<IndexValueDto> GetIndexLastValue(EIndex index)
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.IndexesLastValue) + index.ToString();

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out IndexValueDto valueDto))
            {

                if (index == EIndex.TR)
                {                    
                    
                    var values = await GetIndex(index, DateTime.Now.AddMonths(-1));

                    var value = values.OrderByDescending(a => a.Date)
                            .Where(a => a.Index == index && a.Date.Day == 1 && a.DateEnd.Day == 1)
                    .FirstOrDefault();


                    valueDto = _mapper.Map<IndexValueDto>(value);
                }
                else
                {
                    var values = await GetIndex(index, DateTime.Now.AddMonths(-1));

                    var value = values.OrderByDescending(a => a.Date).FirstOrDefault();

                    valueDto = _mapper.Map<IndexValueDto>(value);
                }
                
                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromHours(1)
                };


                //setting cache entries
                _memoryCache.Set(cacheKey, valueDto, cacheExpiryOptions);
            }

            //_context.ProspectIndexValues.Load();
            return valueDto;
        }
        public async Task<double> GetIndexValueBetweenDates(EIndex index, DateTime startDate, DateTime endDate, double indexPercentage = 1)
        {
                                    
            var indexLastValue = await GetIndexLastValue(index);
            double cumRealValue = 0.00;
            
            var indexRecurrence = indexLastValue.IndexRecurrence;

            if (indexRecurrence == EIndexRecurrence.Daily)
                cumRealValue = await GetDailyIndexCumValueAsync(index, startDate, endDate, indexPercentage);            
            else if (indexRecurrence == EIndexRecurrence.Monthly)
            {
                throw new NotImplementedException();

                //Terminar a parte de adaptar para valores futuros e quando a data inicial é no meio do mês
                //cumRealValue = await GetMonthlyIndexRealValueAsync(index, startDate, endDate);

            }
            else if (indexRecurrence == EIndexRecurrence.Yearly)
                throw new NotImplementedException();
            
            return cumRealValue;

        }
        private async Task<double> GetMonthlyIndexRealValueAsync(EIndex index, DateTime dateStart, DateTime dateEnd)
        {
            var indexLastValue = await GetIndexLastValue(index);

            var indexRealValues = await GetIndex(index, dateStart, dateEnd.AddDays(-1));

            double cumIndexValue = 1.00;           

            if(dateStart.Day != 1)
                //corrige o valor para apenas os dias corridos
            if(indexLastValue.Date.Year < dateEnd.Year || 
                    (indexLastValue.Date.Year == dateEnd.Year 
                    && indexLastValue.Date.Year < dateEnd.Date.Year))
                    //Adicionar valores futuros



            indexRealValues
                .Select(a => a.Value)
                .ToList()
                .ForEach(a => cumIndexValue *= (1 + a));

             return cumIndexValue;



        }
        private async Task<double> GetDailyIndexCumValueAsync(EIndex index, DateTime dateStart, DateTime dateEnd, double indexPercentage)
        {
            var indexLastValue = await GetIndexLastValue(index);

            var indexRealValues = await GetIndex(index, dateStart, dateEnd.AddDays(-1));
            
            double cumIndexValue = 1.00;
            
            indexRealValues
                .Select(a => a.Value * indexPercentage)
                .ToList()
                .ForEach(a => cumIndexValue *= (1 + a));
            //Alterar para add business days
            if (await _datesService.AddWorkingDays(dateEnd, -1) > indexLastValue.Date)
            {
                var prospect = await GetIndexProspect(index);

                prospect.Min(a => a.DateStart);
                DateTime dateStartProspect = dateStart > indexLastValue.Date ? dateStart : indexLastValue.Date;
                for(DateTime date = dateStartProspect.AddDays(1); date < dateEnd; date = date.AddDays(1))
                {
                    if (await _datesService.IsHoliday(date) || 
                        date.DayOfWeek == DayOfWeek.Sunday || 
                        date.DayOfWeek == DayOfWeek.Saturday)
                        continue;
                    cumIndexValue = await AddMissingFutureValues(indexLastValue, cumIndexValue, prospect, date, indexPercentage);

                }


            }


            return cumIndexValue;
        }
        private async Task<double> AddMissingFutureValues(IndexValueDto indexLastValue, double cumIndexValue, List<ProspectIndexValueDto> prospect, DateTime date, double indexPercentage)
        {
            var indexProspectDate = prospect.OrderBy(a => a.DateEnd).FirstOrDefault(a => date >= a.DateStart);

            if (indexProspectDate == null)
            {
                cumIndexValue *= (1 + indexLastValue.Value * indexPercentage);
            }
            else
            {
                if (indexProspectDate.IndexRecurrence == EIndexRecurrence.Yearly)
                {
                    var indexDayValue = await ConvertYearValueToDayValueAsync(indexProspectDate.Median / 100, date.Year);
                    cumIndexValue *= (1 + indexDayValue * indexPercentage);
                }
                else if (indexProspectDate.IndexRecurrence == EIndexRecurrence.Monthly)
                {
                    var indexDayValue = await ConvertMonthValueToDayValueAsync(indexProspectDate.Median / 100, date.Year, date.Month);
                    cumIndexValue *= (1 + indexDayValue * indexPercentage) ;
                }
                else
                {
                    cumIndexValue *= (1 + indexProspectDate.Median * indexPercentage) ;
                }

            }

            return cumIndexValue;
        }
        private async Task<double> ConvertMonthValueToDayValueAsync(double value, int year, int month)
        {

            DateTime dateStart = new(year, month, 1);

            DateTime dateEnd = dateStart.AddMonths(1).AddDays(-1);

            var workingDays = await _datesService.GetWorkingDaysBetweenDates(dateStart, dateEnd);

            var dailyValue = Math.Pow((1.00 + value), (1.00 / Convert.ToDouble(workingDays)));

            return dailyValue;

        }
        private async Task<double> ConvertYearValueToDayValueAsync(double value, int year)
        {
            var workingDays = await _datesService.GetWorkingDaysOfAYear(year);

            var dailyValue = Math.Pow((1.00 + value), (1.00 / Convert.ToDouble(workingDays.WorkingDays)));

            return (dailyValue - 1);

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
