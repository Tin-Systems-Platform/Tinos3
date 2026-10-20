using System;

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
        }

        public static string GetTinosVersion()
        {
            return _tinosVersion;
        }
    }
}