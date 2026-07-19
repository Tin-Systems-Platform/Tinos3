using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Applications.Utils
{
    public class Categories
    {
        public readonly string CategoryName;

        public readonly string Description;

        public Categories(string categoryName, string description)
        {
            this.CategoryName = categoryName;
            this.Description = description;
        }

    }
}
