using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public ForumPost Post { get; set; }
        public List<PostComment> PostComments { get; set; }
    }

    public class PostComment
    {
        public int PostId { get; set; }
        public ForumPost Post { get; set; }
        public int CommentId { get; set; }
        public Comment Comments { get; set; }
    }
}
