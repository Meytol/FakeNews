using System.ComponentModel;

namespace FakeNews.Common.Database.Enums
{
    public enum CommentStatus
    {
        [Description("نامعتبر")]
        ///<summary>
        ///نامعتبر
        ///</summary>
        Invalid = 0,
        [Description("تأیید شده")]
        ///<summary>
        ///تأیید شده
        ///</summary>
        Accepted = 1,

        [Description("رد شده")]
        ///<summary>
        ///رد شده
        ///</summary>
        Rejected = 2,

        [Description("منتظر تأیید")]
        ///<summary>
        ///منتظر تأیید
        ///</summary>
        Pending = 3
    }
}
