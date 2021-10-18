﻿using FakeNews.Common.Database.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;


namespace FakeNews.Database.Tables.Identity
{
    public class User : IdentityUser<int>, IDbTableProperties
    {
        public Guid PublicId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatorId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifierId { get; set; }
    }
}
