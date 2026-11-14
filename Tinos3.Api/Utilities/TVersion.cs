using System;
namespace Tinos3.Api.Utilities
{
    public static class TVersion
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