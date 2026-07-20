using System.Runtime.InteropServices;

namespace Tinos3.Abstractions.FileTypes.TXE
{
    [Flags]
    public enum ExecutableFlags
    {
        None = 0,
        Executable = 1 << 0,
        ReadOnly = 1 << 1,
        RequiresAudio = 1 << 2,
        RequiresGraphics = 1 << 3,
    }
}