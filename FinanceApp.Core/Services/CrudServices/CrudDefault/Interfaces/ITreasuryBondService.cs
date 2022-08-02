using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.TreasuryBond;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface ITreasuryBondService : ICommand<TreasuryBondDto, CreateTreasuryBond, UpdateTreasuryBond>,
        IQuery<TreasuryBondDto>, IForecast<TreasuryBondDto>
    {
        
    }
}