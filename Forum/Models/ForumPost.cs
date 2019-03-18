using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class ForumPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Posted { get; set; }
        public List<PostTopic> PostTopics { get; set; }
        public List<Comment> Comments { get; set; }
        public string TopicsString { get; set; }
    }
}
