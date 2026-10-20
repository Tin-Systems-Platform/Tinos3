using System;
using Tinos3.Abstractions.Lua.Tinos3.Utils;

namespace Tinos3.Abstractions.Utilities
{
    public static class Version
    {
        private static string _tinosVersion =
            "Tinos3 0.2.0 The Rewritten Tinos3";

        public static void SetTinosVersion(string version)
        {
            if (!string.IsNullOrWhiteSpace(version))
                _tinosVersion = version;
            TinosUtils.tinosVersion = version;
        }

        public static string GetTinosVersion()
        {
            return _tinosVersion;
        }
    }
}