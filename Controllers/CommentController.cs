using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Comment;
using server.Extensions;
using server.Helpers;
using server.Interfaces;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController: ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo,UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] CommentQuery query){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comments = await _commentRepo.GetAllAsync(query);
            var commentDtos = comments.Select(c => c.ToCommentDto());
            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null){
                return BadRequest("Comment not found");
            }

            return Ok(comment.ToCommentDto());
        }
        
        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId,[FromBody] CreateCommentDto commentDto){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if(!await _stockRepo.StockExists(stockId)){
                return BadRequest("Stock not found");
            }
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return BadRequest("User not found");
            }

            var commentModel = commentDto.ToCommentFromCreate(stockId);
            commentModel.AppUserId = appUser.Id;
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.UpdateAsync(id,commentDto);
            if(comment == null){
                return BadRequest("Comment not found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.DeleteAsync(id);
            if(comment == null){
                return BadRequest("Comment not found");
            }

            return Ok(comment.ToCommentDto());
        }
    }
}