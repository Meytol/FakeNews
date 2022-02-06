using FakeNews.Common.StaticMethods;
using MD.PersianDateTime;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeNews.Common.Database.Interfaces
{
    public interface IDbTableProperties
    {
        int Id { get; set; }
        Guid PublicId { get; set; }
        DateTime CreatedOn { get; set; }
        int CreatorId { get; set; }

        DateTime? ModifiedOn { get; set; }
        int? ModifierId { get; set; }
        bool IsDeleted { get; set; }

        [NotMapped]
        PersianDateTime PersianCreatedOn => CreatedOn.AsPersianDateTime();
        [NotMapped]
        PersianDateTime PersianModifiedOn => (ModifiedOn ?? CreatedOn).AsPersianDateTime();
    }

    // ReSharper disable once InconsistentNaming
    public abstract class IDbTable : IDbTableProperties
    {
        public virtual int Id { get; set; }
        public virtual Guid PublicId { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual int CreatorId { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual int? ModifierId { get; set; }

        [NotMapped]
        public PersianDateTime PersianCreatedOn => CreatedOn.AsPersianDateTime();
        [NotMapped]
        public PersianDateTime PersianModifiedOn => (ModifiedOn ?? CreatedOn).AsPersianDateTime();

        public bool IsDeleted { get; set; }
    }
}
