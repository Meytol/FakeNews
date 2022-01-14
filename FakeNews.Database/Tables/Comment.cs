using FakeNews.Common.Database.Enums;
using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Tables.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeNews.Database.Tables
{
    public class Comment : IDbTable
    {

        [Display(Name = "متن")]
        public string Text { get; set; }

        [Display(Name = "وضعیت انتشار")]
        public CommentStatus Status { get; set; } = CommentStatus.Accepted;

        [Display(Name = "نام نویسنده")]
        public string SenderName { get; set; }

        [Display(Name = "ایمیل نویسنده")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string SenderMail { get; set; }

        [Display(Name = "شناسه خبر")]
        public News News { get; set; }

        [Display(Name = "خبر")]
        public int NewsId { get; set; }

    }
}
