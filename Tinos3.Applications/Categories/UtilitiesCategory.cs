using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Applications.Utils;

namespace Tinos3.Applications.Categories
{

    public class UtilitiesCategory : Tinos3.Applications.Utils.Categories
    {
        public UtilitiesCategory(string categoryName, string description) : base(categoryName, description)
        {
            categoryName = "Utilities";

            description = "Utilities App Category";
        }
    }
}
