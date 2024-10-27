using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Dtos.Stock;
using server.Mappers;

namespace server.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController :ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult GetAll(){
            var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){
            var stock = _context.Stocks.Find(id);
            if(stock == null){
                return BadRequest("Stock not found");
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto){
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById),new {id = stockModel.Id},stockModel );
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateDto){
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if(stockModel == null){
                return BadRequest("Stock not found");
            }
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Industry = updateDto.Industry;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.MarketCap = updateDto.MarketCap;
            stockModel.Symbol = updateDto.Symbol;

            _context.SaveChanges();
            return Ok(stockModel);
        }

        [HttpDelete]
        [Route("{id}")]

        public IActionResult Delete([FromRoute] int id){
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if(stockModel == null){
                return BadRequest("Stock not found");
            }
            _context.Stocks.Remove(stockModel);
            _context.SaveChanges();

            return Ok(stockModel);
        }
    }
}