using System;

namespace Domain.Entities
{
    public class GetReviewDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int Rating { get; set; }
        public virtual string ReviewerUserName { get; set; }
        public virtual string ReviewerAvatarPath { get; set; }
    }
}