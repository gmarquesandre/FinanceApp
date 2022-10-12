using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController.Base
{
    [Authorize]
    public class CrudControllerBase<TService, TDto, TCreate, TUpdate> : ControllerBase, ICrudController<TCreate, TUpdate>
        where TService : ICrudBase<TDto, TCreate, TUpdate>
        where TDto : StandardDto
        where TCreate : CreateDto
        where TUpdate : UpdateDto
    {
        public TService _service { get; set; }

        public CrudControllerBase(TService service)
        {
            _service = service;
        }



        [HttpGet("Get")]
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


        [HttpGet("Get/{id:int}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {

                var resultado = await _service.GetAsync(id);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> AddAsync(TCreate input)
        {
            try
            {

                var resultado = await _service.AddAsync(input);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {

                var resultado = await _service.DeleteAsync(id);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(TUpdate input)
        {
            try
            {
                var resultado = await _service.UpdateAsync(input);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            try
            {
                var resultado = await _service.DeleteAllAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteBatch")]
        public IActionResult DeleteBatch(List<Guid> ids)
        {
            try
            {
                var resultado = _service.DeleteBatch(ids);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}