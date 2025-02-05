using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dtos.Stock;
using server.Helpers;
using server.Interfaces;
using server.Mappers;

namespace server.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController :ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAll([FromQuery] StockQuery query){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDtos = stocks.Select(s => s.ToStockDto()).ToList();
            return Ok(stockDtos);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = await _stockRepo.GetStockByIdAsync(id);
            if(stock == null){
                return BadRequest("Stock not found");
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
 
            return CreatedAtAction(nameof(GetById),new {id = stockModel.Id},stockModel.ToStockDto() );
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateDto){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
            if (stockModel == null)
            {
                return BadRequest("Stock not found");
            }

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]

        public async Task<IActionResult> Delete([FromRoute] int id){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stockModel = await _stockRepo.DeleteAsync(id);

            if (stockModel == null)
            {
                return BadRequest("Stock not found");
            }

            return Ok(stockModel);
        }
    }
}