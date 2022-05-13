using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using FinancialApi.WebAPI.Data;
using FinancialAPI.Dto;
using System.Collections.Generic;
using FinancialAPI.Data;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class ValueController : ControllerBase
    {
        public readonly FinanceContext _context;

        public ValueController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("mvp/GetTreasuryBond")]
        public ActionResult GetTreasuryBond()
        {
            var values = _context.TreasuryBondValues
                .Where(a => a.ExpirationDate > DateTime.Today)
                .OrderBy(a=> a.ExpirationDate)
                .ToList();
            return Ok(values);
        }

        [HttpGet("mvp/GetProspect")]
        public ActionResult GetProspect()
        {
            var values = _context.ProspectIndexValues
                .Where(a => a.BaseCalculo == 0)
                .ToList();
            return Ok(values);
        }


        [HttpGet("mvp/GetStockWithList")]
        public ActionResult GetStockWithList(string listStocks)
        {
            try { 
                var list = listStocks.Split(",");
                if(list.Length > 0) {

                    var values = _context.Assets
                                    .Where(a=> list.Contains(a.AssetCode))
                                    .ToList();
                    return Ok(values);
            }
            return NoContent();
            }
            catch(Exception e)
            {
                Console.Write(e);
                return NoContent();
            }
        }

        [HttpGet("mvp/GetStock")]
        public ActionResult GetStock(string stock)
        {
            stock = stock.ToUpper();
            if(stock.Length > 1) { 

                var values = _context.Assets
                                .Where(a => a.AssetCode.Contains(stock))
                                .ToList();

                return Ok(values);
            }
            return NoContent();
        }

        [HttpGet("mvp/GetLastValueIndex")]
        public ActionResult GetLastValueIndex()
        {
            var values = _context.IndexLastValues.ToList();

            return Ok(values);
        }

        [HttpGet("mvp/GetIndex")]
        public ActionResult GetIndex(string indexName, DateTime minDate, DateTime maxDate)
        {
            var values = _context.IndexValues.Where(a => a.IndexName == indexName && a.Date >= minDate && a.Date <= maxDate).ToList();
            return Ok(values);
        }

        [HttpGet("mvp/GetHolidays")]
        public ActionResult GetHolidays(DateTime? lastUpdateDate)
        {
            DateTime lastUpdateDateLocal = _context.Holidays.Select(a => a.DateLastUpdate).Max();

            if(lastUpdateDateLocal > lastUpdateDate)
            {
                var values = _context.Holidays.ToList();
                return Ok(values);
            }
            return NoContent();
            
        }

        [HttpGet("mvp/GetWorkingDaysByYear")]
        public ActionResult GetWorkingDaysByYear(DateTime? lastUpdateDate)
        {
            DateTime lastUpdateDateLocal = _context.Holidays.Select(a => a.DateLastUpdate).Max();

            if (lastUpdateDateLocal > lastUpdateDate)
            {
                var values = _context.WorkingDaysByYear.ToList();
                return Ok(values);
            }
            return NoContent();

        }


        [HttpGet("mvp/GetFundValueDay")]
        public ActionResult GetFundValueDay(string cnpj, DateTime date)
        {

            cnpj = cnpj.Replace("%2F", "/");
            try
            {
                var values = _context.InvestmentFundValueHistoric
                    .Where(a => a.CNPJ == cnpj && a.Date.Date == date.Date)
                    .First();

                return Ok(values);
            }
            catch(Exception e)
            {
                return NoContent();
            }
        }


        [HttpGet("mvp/GetTreasuryBondValueDay")]
        public ActionResult GetTreasuryBondValueDay(string codeisin, DateTime date)
        {
            try
            {
                var values = _context.TreasuryBondValueHistoric
                    .First(a => a.CodeISIN == codeisin && a.Date.Date == date.Date);

                return Ok(values);
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpGet("mvp/GetFundWithList")]
        public ActionResult GetFundWithList(string listFund)
        {
            try
            {
                var list = listFund.Replace(".","").Replace("-","").Replace("%2F","").Split(",");
                if (list.Length > 0)
                {
                    //var listaCandidato = _context.Candidatos.ToList();
                    var values = _context.InvestmentFundValues
                                    .Where(a => list.Contains(a.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "")))
                                    .ToList();
                    return Ok(values);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }
        [HttpGet("mvp/GetFundList")]
        public ActionResult GetFund(string value) 
        { 
            try
            {
                if(value.Length > 5) {

                    
                    string valueNumber = Regex.Replace(value, "[^0-9]", "");
                    if(valueNumber.Length == 0)
                    {
                        valueNumber = "11111111111111";
                    }
                    value = value.ToUpper();
                    //var listaCandidato = _context.Candidatos.ToList();
                    var values = _context.InvestmentFundValues
                                    .Where(a => a.CNPJ.Replace("-","").Replace(".","").Replace("/","").StartsWith(valueNumber)
                                    || a.Name.Contains(value)
                                    || a.NameShort.Contains(value)).Take(10)
                                    .ToList();
                   return Ok(values);
                }
                return NoContent();

            }
            catch (Exception e)
            {
                Console.Write(e);
                return NoContent();
            }
        }

        [HttpPost("mvp/PostAssetChanges")]
        public ActionResult GetAssetChanges(List<AssetChangeInput> list)
        {

            var listComplete = _context.AssetChanges
                .Include(a => a.Asset).ToList()
                .Where(a => list.Any( b=> a.Asset.AssetCode == b.AssetCode && a.ExDate.Date >= b.DateStart.Date))
                .ToList();


            List<AssetChangeDto> listReturn = listComplete.Select(a => new AssetChangeDto
            {
                AssetCode = a.Asset.AssetCode,
                AssetCodeISIN = a.Asset.AssetCodeISIN,
                ExDate = a.ExDate,
                DeclarationDate = a.DeclarationDate,
                Id = a.Id,
                Hash = a.Hash,
                Notes = a.Notes,
                Type = a.Type,
                GroupingFactor = a.GroupingFactor,
                ToAssetISIN = a.ToAssetISIN
            }).ToList();


            return Ok(listReturn);

        }


        [HttpPost("mvp/PostAssetEarnings")]
        public ActionResult GetAssetEarnings(List<AssetChangeInput> list)
        {

            var listComplete = _context.AssetEarnings
                .Include(a => a.Asset).ToList()
                .Where(a => list.Any(b => a.Asset.AssetCode == b.AssetCode && a.ExDate.Date >= b.DateStart.Date))
                .ToList();

            List<AssetEarningDto> listReturn = listComplete.Select(a => new AssetEarningDto
            {
                AssetCode = a.Asset.AssetCode,
                AssetCodeISIN = a.Asset.AssetCodeISIN,
                ExDate = a.ExDate,
                CashAmount = a.CashAmount,
                DeclarationDate = a.DeclarationDate,
                Id = a.Id,
                Hash = a.Hash,
                Notes = a.Notes,
                PaymentDate = a.PaymentDate,
                Period = a.Period,
                Type = a.Type
            }).ToList();

            return Ok(listReturn);

        }

    }
}