using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.TreasuryBond;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface ITreasuryBondService : ICrudBase<TreasuryBondDto, CreateTreasuryBond, UpdateTreasuryBond>, IForecast<TreasuryBondDto>
    {
        
    }
}