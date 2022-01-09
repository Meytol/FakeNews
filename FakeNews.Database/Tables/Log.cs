using FakeNews.Common.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FakeNews.Database.Tables
{
    public class Log : IDbTable
    {
        public bool IsError { get; set; }

        [StringLength(maximumLength: 250)]
        public string Title { get; set; }

        [StringLength(maximumLength: 512)]
        public string Description { get; set; }

        public string FullDetail { get; set; }
    }
}
