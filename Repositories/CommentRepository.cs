using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dtos.Comment;
using server.Helpers;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if(commentModel == null){
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQuery query)
        {
            var comments = _context.Comments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                comments = comments.Where(s => s.Stock != null && s.Stock.Symbol == query.Symbol);
            };
            if (query.IsDescending == true)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            return await comments.ToListAsync();
        }
        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto updateDto)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (existingComment == null)
            {
                return null;
            }

            existingComment.Content = updateDto.Content;
            existingComment.Title = updateDto.Title;

            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}