
namespace Tinos3.FS;

public static class SectorCalculator
{
    public static ulong BytesToSectors(
        ulong bytes,
        uint bytesPerSector)
    {
        if (bytesPerSector == 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bytesPerSector));
        }

        // Ceiling division without adding bytesPerSector - 1,
        // which could overflow.
        return bytes / bytesPerSector +
               (bytes % bytesPerSector == 0 ? 0UL : 1UL);
    }

    public static ulong SectorsToBytes(
        ulong sectors,
        uint bytesPerSector)
    {
        if (bytesPerSector == 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bytesPerSector));
        }

        return checked(sectors * bytesPerSector);
    }

    public static ulong MiBToSectors(
        ulong mib,
        uint bytesPerSector)
    {
        ulong bytes = checked(mib * 1024UL * 1024UL);
        return BytesToSectors(bytes, bytesPerSector);
    }

    public static ulong GiBToSectors(
        ulong gib,
        uint bytesPerSector)
    {
        ulong bytes = checked(gib * 1024UL * 1024UL * 1024UL);
        return BytesToSectors(bytes, bytesPerSector);
    }
}
