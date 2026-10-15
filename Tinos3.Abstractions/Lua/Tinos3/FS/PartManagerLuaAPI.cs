using Cosmos.Executable.Lua;
using System;
using Tinos3.FS;
using Tinos3.FS.Helpers;

namespace Tinos3.Abstractions.Lua.Tinos3.FS;

public static class PartManagerLuaAPI
{
    // Non-destructive: list partitions.
    public static int ListPartitions(ILuaState lua)
    {
        var diskIndex = lua.ToInteger(1);
        var manager = new TinosPartManager();
        manager.ListPartitions((int)diskIndex);

        // TODO: Return partition data as a Lua table.
        return 0;
    }

    // Destructive: initialize a partition table.
    public static int InitializePartitionTable(ILuaState lua)
    {
        var diskIndex = lua.ToInteger(1);
        string kindStr = lua.ToString(2);

        PartitionTableKind kind;

        if (kindStr.Equals("GPT", StringComparison.OrdinalIgnoreCase))
            kind = PartitionTableKind.GPT;
        else if (kindStr.Equals("MBR", StringComparison.OrdinalIgnoreCase))
            kind = PartitionTableKind.MBR;
        else
        {
            lua.PushBoolean(false);
            return 1;
        }

        var manager = new TinosPartManager();
        bool success = manager.InitializePartitionTable((int)diskIndex, kind);

        lua.PushBoolean(success);
        return 1;
    }

    // Destructive: create a partition.
    public static int CreatePartition(ILuaState lua)
    {
        var diskIndex = lua.ToInteger(1);
        if (!ulong.TryParse(lua.ToString(2), out ulong startSector) ||
            !ulong.TryParse(lua.ToString(3), out ulong sectorCount) ||
            sectorCount == 0)
        {
            lua.PushBoolean(false);
            return 1;
        }

        var manager = new TinosPartManager();
        bool success = manager.CreatePartition((int)diskIndex, startSector, sectorCount);

        lua.PushBoolean(success);
        return 1;
    }

    // Destructive: delete a partition.
    public static int DeletePartition(ILuaState lua)
    {
        var diskIndex = lua.ToInteger(1);
        if (!ulong.TryParse(lua.ToString(2), out ulong startSector) ||
            !ulong.TryParse(lua.ToString(3), out ulong sectorCount) ||
            sectorCount == 0)
        {
            lua.PushBoolean(false);
            return 1;
        }

        var manager = new TinosPartManager();
        bool success = manager.DeletePartition((int)diskIndex, startSector, sectorCount);

        lua.PushBoolean(success);
        return 1;
    }

    // Destructive: resize a partition.
    public static int ResizePartition(ILuaState lua)
    {
        var diskIndex = lua.ToInteger(1);
        if (!ulong.TryParse(lua.ToString(2), out ulong startSector) ||
            !ulong.TryParse(lua.ToString(3), out ulong oldSectorCount) ||
            !ulong.TryParse(lua.ToString(4), out ulong newSectorCount) ||
            oldSectorCount == 0 ||
            newSectorCount == 0)
        {
            lua.PushBoolean(false);
            return 1;
        }

        var manager = new TinosPartManager();
        bool success = manager.ResizePartition(
            (int)diskIndex,
            startSector,
            oldSectorCount,
            newSectorCount);

        lua.PushBoolean(success);
        return 1;
    }
}