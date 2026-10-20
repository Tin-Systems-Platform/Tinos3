using System.Reflection;
using Tinos3.Abstractions;

namespace Tinos3.Scripts;

public static class ScriptResources
{
    public static byte[] Get(string resourceName)
    {
        return SysEmbeddedResourceManager.GetEmbeddedResource(
            typeof(ScriptResources).Assembly,
            resourceName);
    }
}