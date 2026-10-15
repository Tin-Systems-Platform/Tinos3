using System;
using System.IO;
using System.Reflection;

namespace Tinos3.Abstractions
{
    public static class SysEmbeddedResourceManager
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