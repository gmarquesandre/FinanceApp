//using AutoMapper;
//using FinanceApp.Core.Services.CrudServices.Base;
//using FinanceApp.Core.Services.CrudServices.Interfaces;
//using FinanceApp.EntityFramework;
//using FinanceApp.Shared.Dto.Loan;
//using FinanceApp.Shared.Models.CommonTables;
//using FinanceApp.Shared.Models.UserTables;
//using FluentResults;
//using Microsoft.EntityFrameworkCore;

//namespace FinanceApp.Core.Services.CrudServices.Implementations
//{
//    public class LoanService : CrudServiceBase, ILoanService
//    {

//        public LoanService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

//        public async Task<LoanDto> AddAsync(CreateLoan input, CustomIdentityUser user)
//        {
//            Loan model = _mapper.Map<Loan>(input);

//            CheckInvestment(model);


//            model.UserId = user.Id;
//            await _context.Loans.AddAsync(model);
//            await _context.SaveChangesAsync();
//            return _mapper.Map<LoanDto>(model);

//        }
//        public async Task<Result> UpdateAsync(UpdateLoan input, CustomIdentityUser user)
//        {
//            var oldModel = _context.Loans.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

//            if (oldModel == null)
//                return Result.Fail("Já foi deletado");
//            else if (oldModel.UserId != user.Id)
//                return Result.Fail("Usuário Inválido");

//            var model = _mapper.Map<Loan>(input);
//            model.CreationDateTime = oldModel.CreationDateTime;
//            model.User = user;


//            CheckInvestment(model);


//            _context.Loans.Update(model);
//            await _context.SaveChangesAsync();
//            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
//        }

//        public async Task<LoanDto> GetAsync(CustomIdentityUser user, int id)
//        {
//            var value = await _context.Loans.FirstOrDefaultAsync(a => a.User.Id == user.Id && a.Id == id);

//            if (value == null)
//                throw new Exception("Registro Não Encontrado");

//            return _mapper.Map<LoanDto>(value);

//        }

//        private void CheckInvestment(Loan model)
//        {

//            if (model.InitialDate >= model.EndDate)
//            {
//                throw new Exception("A data de vencimento deve ser maior do que a de investimento ");
//            }


//        }

//        public async Task<List<LoanDto>> GetAsync(CustomIdentityUser user)
//        {
//            var values = await _context.Loans.Where(a => a.User.Id == user.Id).ToListAsync();
//            return _mapper.Map<List<LoanDto>>(values);
//        }
//        public async Task<Result> DeleteAsync(int id, CustomIdentityUser user)
//        {
//            var investment = await _context.Loans.FirstOrDefaultAsync(a => a.Id == id);

//            if (investment == null)
//            {
//                return Result.Fail("Não Encontrado");
//            }

//            if (investment.UserId != user.Id)
//            {
//                return Result.Fail("Usuário Inválido");
//            }

//            _context.Loans.Remove(investment);
//            await _context.SaveChangesAsync();
//            return Result.Ok().WithSuccess("Investimento deletado");
//        }
//    }
//}