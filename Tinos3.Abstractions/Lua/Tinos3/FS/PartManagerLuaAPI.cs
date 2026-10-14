using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.FS;
using Tinos3.FS.Helpers;

namespace Tinos3.Abstractions.Lua.Tinos3.FS;

public class PartManagerLuaAPI
{
    // Non destructive: List all partitions on the disk
    public static int ListPartitions(ILuaState lua)
    {
        TinosPartManager partManager = new TinosPartManager();
        partManager.ListPartitions();
        return 0;
    }

    // Destructive: Initialize a new partition table on the disk
    public static int InitializePartitionTable(ILuaState lua)
    {
        string kindStr = lua.ToString(-1);
        PartitionTableKind kind;
        if (kindStr.Equals("GPT", StringComparison.OrdinalIgnoreCase))
        {
            kind = PartitionTableKind.GPT;
        }
        else if (kindStr.Equals("MBR", StringComparison.OrdinalIgnoreCase))
        {
            kind = PartitionTableKind.MBR;
        }
        else
        {
            lua.PushBoolean(false);
            return 1;
        }
        TinosPartManager partManager = new TinosPartManager();
        bool result = partManager.InitializePartitionTable(kind);
        lua.PushBoolean(result);
        return 1;
    }


}