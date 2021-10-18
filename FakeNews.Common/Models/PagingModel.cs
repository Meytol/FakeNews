using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNews.Common.Models
{
    public class PagingModel
    {
        private int take;

        private int page;


        public int Page
        {
            get => page;
            set 
            {
                page = value < 1 ? 1 : value;
            }
        }


        public int Take
        {
            get => take;
            set 
            {
                if (value < 1)
                {
                    value = 1;
                }
                else if (value > 20)
                {
                    value = 20;
                }

                take = value; 
            }
        }

        public int Skip => (Page - 1) * Take;

        public bool OrderByAsc { get; set; } = true;

    }
}
