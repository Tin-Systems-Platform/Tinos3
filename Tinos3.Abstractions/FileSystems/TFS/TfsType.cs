using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.FileSystems.TFS
{
    /// <summary>
    /// The type of the TFS (Tinos3 File System).
    /// This also will be used to determine the version of the TFS.
    /// Currently, TFS1 is the only TFS type available, but in the future, there may be more types of TFS.
    /// </summary>
    public enum TfsType
    {
        Unknown,
        Tfs1,
    }
}
