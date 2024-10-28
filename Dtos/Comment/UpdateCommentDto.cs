using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 characters long")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 300 characters long")]
        public string Title {get;set;} = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 characters long")]
        [MaxLength(280, ErrorMessage = "Content cannot be over 300 characters long")]
        public string Content { get; set; } = string.Empty;
    }
}