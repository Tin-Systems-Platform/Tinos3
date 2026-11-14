using Cosmos.Kernel.HAL.Devices.Storage;
using Cosmos.Kernel.System.Storage;

namespace Tinos3.FS;

internal sealed class MemoryBlockDevice : IBlockDevice
{
    private readonly byte[] _storage;

    public MemoryBlockDevice(string name, ulong blockSize, ulong blockCount)
    {
        Name = name;
        BlockSize = blockSize;
        BlockCount = blockCount;
        _storage = new byte[blockSize * blockCount];
    }

    public string Name { get; }
    public ulong BlockSize { get; }
    public ulong BlockCount { get; }

    public void ReadBlock(ulong blockNo, ulong blockCount, Span<byte> data)
        => _storage.AsSpan((int)(blockNo * BlockSize), (int)(blockCount * BlockSize)).CopyTo(data);

    public void WriteBlock(ulong blockNo, ulong blockCount, ReadOnlySpan<byte> data)
        => data.Slice(0, (int)(blockCount * BlockSize)).CopyTo(_storage.AsSpan((int)(blockNo * BlockSize)));

    public void Flush() { }
}

public class RamDisk
{
 

    public static void Create()
    {
        MemoryBlockDevice ramDisk = new("RAMDISK", 512, 65536);  
        FatFileSystemType fat = new(ramDisk);
        
        if (!VfsManager.RegisterFileSystem("ramfat", fat)
            || !VfsManager.TryFormat("ramfat", "", new FatFormatOptions { Type = FatType.Fat16 })
            || !VfsManager.TryMount("ramfat", "", MountFlags.None, "/mnt", out _))
        {
            Console.WriteLine("FS: RAM disk setup failed.");
            return;
        }
    }
}
