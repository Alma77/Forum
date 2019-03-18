using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public List<PostTopic> PostTopics { get; set; }
    }

    public class PostTopic
    {
        public int PostId { get; set; }
        public ForumPost Post { get; set; }
        public int TopicId { get; set; }
        public Topic Topics { get; set; }
    }
}
