using AutoMapper;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using FinanceApp.EntityFramework;
using FinanceApp.Core.Services.DefaultServices.Interfaces;

namespace FinanceApp.Core.Services
{
    public class PrivateFixedIncomeService : IPrivateFixedIncomeService1
    {
        private IRepository<PrivateFixedIncome> _repository;
        private IMapper _mapper;
        public PrivateFixedIncomeService(IRepository<PrivateFixedIncome> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PrivateFixedIncomeDto> AddInvestmentAsync(CreatePrivateFixedIncome input)
        {
            PrivateFixedIncome model = _mapper.Map<PrivateFixedIncome>(input);

            CheckInvestment(model);

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;
            
            await _repository.InsertAsync(model);
            
            return _mapper.Map<PrivateFixedIncomeDto>(model);

        }
        public async Task<Result> UpdateInvestmentAsync(UpdatePrivateFixedIncome input)
        {
            var oldModel = await _repository.GetByIdAsync(input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");

            var model = _mapper.Map<PrivateFixedIncome>(input);
           

            CheckInvestment(model);

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;

            await _repository.UpdateAsync(model.Id, model);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        private void CheckInvestment(PrivateFixedIncome model)
        {
            if (model.InvestmentDate > DateTime.Now.Date)
            {
                throw new Exception("A data de investimento não pode ser maior do que hoje");
            }
            else if (model.InvestmentDate >= model.ExpirationDate)
            {
                throw new Exception("A data de vencimento deve ser maior do que a de investimento ");
            }
            else if (model.ExpirationDate <= DateTime.Now.Date)
            {
                throw new Exception("A data de vencimento deve ser maior do que hoje");
            }


        }

        public async Task<List<PrivateFixedIncomeDto>> GetAllFixedIncomeAsync()
        {
            var values = await _repository.GetAllListAsync();
            return _mapper.Map<List<PrivateFixedIncomeDto>>(values);
        }
        public async Task<Result> DeleteInvestmentAsync(int id)
        {
            var investment = await _repository.GetByIdAsync(id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}