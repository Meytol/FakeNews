using System;

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
    }

    // ReSharper disable once InconsistentNaming
    public abstract class IDbTable : IDbTableProperties
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatorId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifierId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
