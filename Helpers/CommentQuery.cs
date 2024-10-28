using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Helpers
{
    public class CommentQuery
    {
        public string Symbol { get; set; }
        public bool IsDescending { get; set; } = true;

    }
}