using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController.Base
{
    public class CrudSingleControllerBase<TService, TDto, TCreateOrUpdate> : ControllerBase, ICrudSingleController<TCreateOrUpdate>
        where TService : ICrudSingleBase<TDto, TCreateOrUpdate>
        where TDto : StandardDto
        where TCreateOrUpdate: CreateOrUpdateDto
    {
        public TService _service { get; set; }

        public CrudSingleControllerBase(TService service)
        {
            _service = service;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var resultado = await _service.GetAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateOrUpdate")]
        [Authorize]
        public async Task<IActionResult> CreateOrUpdateAsync(TCreateOrUpdate input)
        {
            try
            {

                var resultado = await _service.AddOrUpdateAsync(input);
                return Created("", resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync()
        {
            try
            {

                var resultado = await _service.DeleteAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}