# Tinos3 Lua API

This reference covers the custom Lua globals registered by Tinos3. The
globals are installed during `SystemInitializer.SysInit`, after the file
system is initialized and before the shell starts. Standard Lua globals,
including `print`, `load`, `pcall`, `io`, and `_VERSION`, are not part of this
reference.

## API index

### System

- [`GetCurrentDateTime`](#getcurrentdatetime)
- [`SetTinosVersion`](#settinosversion)
- [`GetTinosVersion`](#gettinosversion)
- [`Shutdown`](#shutdown)
- [`Reboot`](#reboot)
- [`ReadLine`](#readline)
- [`Clear`](#clear)

### Disk and partition management

- [`ListPartitions`](#listpartitions)
- [`InitializePartitionTable`](#initializepartitiontable)
- [`CreatePartition`](#createpartition)
- [`DeletePartition`](#deletepartition)
- [`ResizePartition`](#resizepartition)
- [`ListDisks`](#listdisks)
- [`GetDiskCount`](#getdiskcount)

### File system and interactive tools

- [`FDiskMain`](#fdiskmain)
- [`ListFiles`](#listfiles)
- [`ReadFile`](#readfile)
- [`CopyFile`](#copyfile)
- [`MakeDirectory`](#makedirectory)
- [`RemoveDirectory`](#removedirectory)
- [`RemoveFile`](#removefile)
- [`MoveFile`](#movefile)
- [`MoveDirectory`](#movedirectory)
- [`RenameFile`](#renamefile)
- [`RenameDirectory`](#renamedirectory)
- [`MakeFile`](#makefile)
- [`NanoMain`](#nanomain)
- [`LoadScript`](#loadscript)

## System API

### `GetCurrentDateTime`

```lua
dateTime = GetCurrentDateTime()
```

Returns the current local system time as a string formatted
`yyyy-MM-dd HH:mm:ss`.

- **Returns:** one string.
- **Side effects:** reads `DateTime.Now`.

### `SetTinosVersion`

```lua
result = SetTinosVersion(version)
```

Stores `version` as the process-wide Tinos version returned by
`GetTinosVersion`. The wrapper reads its argument from Lua stack index `-1`.

- **Parameters:** `version` — value converted by the Lua runtime to a string.
- **Returns:** the wrapper reports one return value, but it does not push a
  value. The resulting Lua value is therefore runtime-dependent and should not
  be used.
- **Side effects:** changes the process-wide version string.

### `GetTinosVersion`

```lua
version = GetTinosVersion()
```

Returns the current Tinos version string. The initial value is
`Tinos3 0.2.0 The Rewritten Tinos3`; system initialization later replaces it
with `Tinos3 0.2.0 the Rewritten Tinos3 beta 1`.

- **Returns:** one string.

### `Shutdown`

```lua
Shutdown()
```

Requests a system shutdown through the Cosmos power API.

- **Returns:** no values.
- **Side effects:** initiates system shutdown.

### `Reboot`

```lua
Reboot()
```

Requests a system reboot through the Cosmos power API.

- **Returns:** no values.
- **Side effects:** initiates system reboot.

### `ReadLine`

```lua
line = ReadLine()
```

Reads one line from the console.

- **Returns:** one string containing the line, or the Lua runtime's
  representation of a `null` console result. The exact null conversion is not
  verified from the available Lua runtime source.
- **Side effects:** blocks waiting for console input.

### `Clear`

```lua
Clear()
```

Clears the console.

- **Returns:** no values.
- **Side effects:** calls `Console.Clear`.

## Disk and partition management

Disk indexes are zero-based. Partition sector arguments are converted from Lua
values to strings and then parsed as unsigned decimal integers by the
wrappers.

### `ListPartitions`

```lua
ListPartitions(diskIndex)
```

Prints the detected partition-table kind and partition count for the selected
disk. It prints a message for an invalid disk, a missing disk, or an
unrecognized partition table.

- **Parameters:** `diskIndex` — integer disk index.
- **Returns:** no values. The wrapper returns a zero count and prints results
  to the console; it does not return a Lua table despite the implementation
  TODO.
- **Side effects:** reads disk metadata and prints to the console.

### `InitializePartitionTable`

```lua
success = InitializePartitionTable(diskIndex, kind)
```

Initializes a disk that does not already have a recognized partition table.

- **Parameters:**
  - `diskIndex` — integer disk index.
  - `kind` — `"GPT"` or `"MBR"`, case-insensitive.
- **Returns:** one boolean. Invalid kinds and failed initialization return
  `false`; successful initialization returns `true`.
- **Side effects:** writes partition-table metadata and rescans partitions.

### `CreatePartition`

```lua
success = CreatePartition(diskIndex, startSector, sectorCount)
```

Creates a basic-data partition at the requested sector range.

- **Parameters:**
  - `diskIndex` — integer disk index.
  - `startSector` — positive unsigned integer accepted as a decimal string.
  - `sectorCount` — positive unsigned integer accepted as a decimal string.
- **Returns:** one boolean. Invalid arguments or a failed operation return
  `false`.
- **Side effects:** writes partition metadata and rescans partitions.
- **Limitations:** the requested range must be inside the disk, non-overlapping,
  and valid for its partition-table format.

### `DeletePartition`

```lua
success = DeletePartition(diskIndex, startSector, sectorCount)
```

Deletes the partition identified by its starting sector and sector count.

- **Parameters:** the same `diskIndex`, `startSector`, and `sectorCount`
  constraints as [`CreatePartition`](#createpartition).
- **Returns:** one boolean. Invalid arguments or a failed operation return
  `false`.
- **Side effects:** deletes partition metadata and rescans partitions.

### `ResizePartition`

```lua
success = ResizePartition(
    diskIndex, startSector, oldSectorCount, newSectorCount
)
```

Resizes a partition identified by its current start and size.

- **Parameters:**
  - `diskIndex` — integer disk index.
  - `startSector` — positive unsigned integer accepted as a decimal string.
  - `oldSectorCount` — positive unsigned integer accepted as a decimal string.
  - `newSectorCount` — positive unsigned integer accepted as a decimal string.
- **Returns:** one boolean. Invalid arguments or a failed operation return
  `false`.
- **Side effects:** updates partition metadata and rescans partitions.
- **Limitations:** both the existing and new ranges must fit within the disk.

### `ListDisks`

```lua
result = ListDisks()
```

Prints each detected disk's index, name, block count, block size, capacity,
and detected partition-table type.

- **Returns:** the wrapper reports one return value but does not push one.
  Do not use the result; the intended observable behavior is console output.
- **Side effects:** enumerates storage devices and updates the internal disk
  manager used by [`GetDiskCount`](#getdiskcount).
- **Limitations:** the exact Lua value left on the stack by the mismatched
  return count is not verified from the Lua runtime implementation.

### `GetDiskCount`

```lua
count = GetDiskCount()
```

Returns the disk count as a string prefixed with `"Disk Count: "`.

- **Returns:** one string, for example `"Disk Count: 2"`.
- **Prerequisite:** call [`ListDisks`](#listdisks) first to initialize the
  internal disk-manager field. The behavior before that call is not verified
  and may fail because the field is initially unset.

## File system and interactive tools

Relative paths are resolved below `/mnt`. Absolute paths are used as rooted
paths. File-system commands print status and error messages to the console and
do not return success values unless noted below.

### `FDiskMain`

```lua
FDiskMain()
```

Starts the interactive FDisk console. It accepts commands for creating,
deleting, listing, initializing, resizing, and formatting partitions.

- **Returns:** no values.
- **Side effects:** blocks until the FDisk console exits and may modify disk
  partition metadata.

### `ListFiles`

```lua
ListFiles(path, hasColor)
```

Lists directory and file names in `path`. Directories are printed before files.

- **Parameters:**
  - `path` — directory path. A blank path is treated as `"."`.
  - `hasColor` — boolean controlling directory/file console coloring.
- **Returns:** no values.
- **Errors:** missing directories and listing failures are printed to the
  console.

### `ReadFile`

```lua
ReadFile(path)
```

Prints the complete text contents of a file without adding a terminating
newline.

- **Parameters:** `path` — file path.
- **Returns:** no values.
- **Errors:** blank paths, missing files, and read failures are printed to the
  console.

### `CopyFile`

```lua
CopyFile(sourcePath, destinationPath, overwrite)
```

Copies a file to a destination.

- **Parameters:**
  - `sourcePath` — source file path.
  - `destinationPath` — destination file path.
  - `overwrite` — boolean passed to the underlying copy operation.
- **Returns:** no values.
- **Errors:** invalid paths, missing sources, and copy failures are printed to
  the console.

### `MakeDirectory`

```lua
MakeDirectory(path)
```

Creates a directory, including missing parent directories.

- **Parameters:** `path` — directory path.
- **Returns:** no values.
- **Errors:** blank paths and creation failures are printed to the console.

### `RemoveDirectory`

```lua
RemoveDirectory(path)
```

Deletes a directory recursively.

- **Parameters:** `path` — directory path.
- **Returns:** no values.
- **Errors:** blank paths and deletion failures are printed to the console.

### `RemoveFile`

```lua
RemoveFile(path)
```

Deletes a file.

- **Parameters:** `path` — file path.
- **Returns:** no values.
- **Errors:** blank paths and deletion failures are printed to the console.
  The underlying delete operation does not explicitly report a missing file.

### `MoveFile`

```lua
MoveFile(sourcePath, destinationPath)
```

Moves a file to a destination path.

- **Parameters:** `sourcePath` and `destinationPath` — file paths.
- **Returns:** no values.
- **Errors:** blank paths, missing sources, and move failures are printed to
  the console.

### `MoveDirectory`

```lua
MoveDirectory(sourcePath, destinationPath)
```

Moves a directory to a destination path.

- **Parameters:** `sourcePath` and `destinationPath` — directory paths.
- **Returns:** no values.
- **Errors:** blank paths, missing sources, and move failures are printed to
  the console.

### `RenameFile`

```lua
RenameFile(path, newName)
```

Renames a file within its current parent directory.

- **Parameters:**
  - `path` — existing file path.
  - `newName` — new name, not a destination path.
- **Returns:** no values.
- **Errors:** blank arguments, missing sources, and rename failures are
  printed to the console.

### `RenameDirectory`

```lua
RenameDirectory(path, newName)
```

Renames a directory within its current parent directory.

- **Parameters:**
  - `path` — existing directory path.
  - `newName` — new name, not a destination path.
- **Returns:** no values.
- **Errors:** blank arguments, missing sources, and rename failures are
  printed to the console.

### `MakeFile`

```lua
MakeFile(path)
```

Creates an empty file, replacing/truncating an existing file according to the
underlying `File.Create` behavior.

- **Parameters:** `path` — file path.
- **Returns:** no values.
- **Errors:** blank paths and creation failures are printed to the console.

### `NanoMain`

```lua
NanoMain(path)
```

Starts the interactive Nano clone for `path`. Escape or Ctrl+Q exits; Ctrl+S
saves and Ctrl+C displays the line count.

- **Parameters:** `path` — file path. A blank path is internally replaced by
  `"default.txt"`, but the wrapper returns immediately instead of launching
  Nano in that case.
- **Returns:** no values.
- **Side effects:** blocks for interactive editing and can write file contents.

### `LoadScript`

```lua
source, errorMessage = LoadScript(path)
```

Reads a script file and returns its source text.

- **Parameters:** `path` — script file path.
- **Returns:**
  - On success: one string containing the file contents.
  - For a blank path, missing file, or read exception: `nil` followed by one
    error string.
- **Errors:** error strings include `"No file specified."`, a missing-file
  message, or an exception message prefixed with `"Unable to read file ..."`.
- **Side effects:** reads the resolved file; it does not execute the script.

## Coverage and unresolved issues

The registration chain was traced from
[`SystemInitializer`](Tinos3/SystemInitializer.cs) through
[`Tinos3Lua`](Tinos3.Abstractions/Lua/Tinos3/Tinos3Lua.cs),
[`RegisterFSApi`](Tinos3.Abstractions/Lua/Tinos3/Registers/RegisterFSApi.cs),
and [`RegisterHelper`](Tinos3.Abstractions/Lua/Tinos3/Helpers/RegisterHelper.cs)
to each delegate implementation.

- **Verified custom globals:** 28.
- **Documented custom globals:** 28.
- **Undocumented registered globals:** 0.
- **Missing registration paths:** none found in the inspected source.
- **Unverified or problematic behavior:**
  - `SetTinosVersion` reports one return value but pushes none.
  - `ListDisks` reports one return value but pushes none.
  - `GetDiskCount` depends on the private disk-manager field initialized by
    `ListDisks`; behavior before initialization needs maintainer confirmation.
  - Lua conversion behavior for `null` returned by `Console.ReadLine` and for
    invalid argument types depends on the external Cosmos Lua runtime and was
    not verified from this repository.
  - `ListPartitions` intentionally prints only a summary; its source contains
    a TODO for returning partition data as a Lua table.
  - File-system path normalization uses host-style `Path` behavior after
    mapping relative paths under `/mnt`; platform-specific path edge cases
    need runtime verification.

The reference describes the current implementation, including these
limitations. The return-count mismatches and the `GetDiskCount` initialization
dependency should be reviewed before treating those APIs as stable contracts.

## Note!

This documentation was initially generated by GitHub Copilot and reviewed by [@randomusert](https://github.com/randomusert).