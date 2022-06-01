using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.CrudServices
{
    public class CategoryService : CrudServiceBase, ICategoryService
    {

        public CategoryService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<CategoryDto> AddAsync(CreateCategory input, CustomIdentityUser user)
        {

            Category model = _mapper.Map<Category>(input);

            model.UserId = user.Id;
            await _context.Categories.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateCategory input, CustomIdentityUser user)
        {
            var oldModel = _context.Categories.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            else if (oldModel.UserId != user.Id)
                return Result.Fail("Usuário Inválido");

            var model = _mapper.Map<Category>(input);

            model.User = user;
            model.CreationDateTime = oldModel.CreationDateTime;

            _context.Categories.Update(model);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }



        public async Task<List<CategoryDto>> GetAsync(CustomIdentityUser user)
        {
            var values = await _context.Categories.Where(a => a.User.Id == user.Id).ToListAsync();
            return _mapper.Map<List<CategoryDto>>(values);
        }

        public async Task<CategoryDto> GetAsync(CustomIdentityUser user, int id)
        {
            var value = await _context.Categories.FirstOrDefaultAsync(a => a.User.Id == user.Id && a.Id == id);

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<CategoryDto>(value);

        }

        public async Task<Result> DeleteAsync(int id, CustomIdentityUser user)
        {
            var investment = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            if (investment.UserId != user.Id)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.Categories.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}