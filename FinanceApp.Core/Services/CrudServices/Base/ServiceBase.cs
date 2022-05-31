using AutoMapper;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Core.Services.CrudServices.Base
{
    public class CrudServiceBase
    {
        public FinanceContext _context;
        public IMapper _mapper;

        public CrudServiceBase(FinanceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public void CheckValue(SpendingAndIncome model)
        {
            if (model.Recurrence == ERecurrence.NTimes && (model.TimesRecurrence == null || model.TimesRecurrence == 0))
                throw new Exception("A quantidade de repetições deve ser maior que zero para o tipo de recorrência selecionado");

            else if (model.Recurrence != ERecurrence.NTimes && model.Recurrence != ERecurrence.Once && model.EndDate == null)
                throw new Exception("A data final deve ser preenchida");

            else if (model.Amount <= 0.00M)
                throw new Exception("O valor deve ser maior do que zero");

        }
    }
}
