using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Shared;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class PrivateFixedIncomeService : IPrivateFixedIncomeService
    {
        private IMapper _mapper;
        private IRepository<PrivateFixedIncome> _repository;
        public PrivateFixedIncomeService(IRepository<PrivateFixedIncome> repository, IMapper mapper) 
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<PrivateFixedIncomeDto> AddAsync(CreatePrivateFixedIncome input)
        {
            PrivateFixedIncome model = _mapper.Map<PrivateFixedIncome>(input);

            CheckInvestment(model);

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;

            await _repository.InsertAsync(model);
            return _mapper.Map<PrivateFixedIncomeDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdatePrivateFixedIncome input)
        {            
            var model = _mapper.Map<PrivateFixedIncome>(input);
            
            CheckInvestment(model);

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;

            _repository.Update(model.Id, model);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<PrivateFixedIncomeDto> GetAsync(int id)
        {
            var value = await _repository.GetById(id);
            
            return _mapper.Map<PrivateFixedIncomeDto>(value);

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

        public async Task<List<PrivateFixedIncomeDto>> GetAsync()
        {
            var values = await _repository.GetAllAsync();
            return _mapper.Map<List<PrivateFixedIncomeDto>>(values);
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var investment = await _repository.GetById(id);

            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}