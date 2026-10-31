using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tinos3.Api.Internal
{
    public class EmbeddedResourceManager
    {
        public static byte[] GetEmbeddedResource(
           Assembly assembly,
           string resourceName)
        {
            using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException(
                    $"Resource '{resourceName}' not found.");

            using var ms = new MemoryStream();
            stream.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
