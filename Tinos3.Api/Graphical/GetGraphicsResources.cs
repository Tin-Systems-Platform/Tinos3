using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Api.Internal;

namespace Tinos3.Api.Graphical
{
    public class GetGraphicsResources
    {
        public static byte[] Get(string resourceName)
        {
            return EmbeddedResourceManager.GetEmbeddedResource(
                typeof(Tinos3.Resources.Class1).Assembly,
                resourceName);
        }
    }
}
