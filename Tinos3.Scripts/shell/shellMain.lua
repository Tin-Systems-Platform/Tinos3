
print("LSH: Initialized.")

local function ParseCommand(input)
    local command, arguments =
        input:match("^%s*(%S+)%s*(.-)%s*$")

    return command, arguments
end

local function ExecuteLua(source, chunkName)
    local chunk, compileError = load(source, chunkName)

    if not chunk then
        print("Lua syntax error: " .. tostring(compileError))
        return
    end

    local ok, result = pcall(chunk)

    if not ok then
        print("Lua runtime error: " .. tostring(result))
    elseif result ~= nil then
        print(result)
    end
end

while true do
    io.write("LSH> ")

    local input = ReadLine()

    if input == nil then
        break
    end

    local command, arguments = ParseCommand(input)

    if not command or command == "" then
        -- Ignore empty input.

    elseif command == "poweroff" then
        Shutdown()
        break

    elseif command == "reboot" then
        Reboot()
        break

    elseif command == "help" then
        print("Available commands:")
        print("  poweroff             - Shut down")
        print("  reboot               - Reboot")
        print("  help                 - Show help")
        print("  ver                  - Show versions")
        print("  fdisk                - Run FDisk")
        print("  clear                - Clear screen")
        print("  ls [path]            - List files")
        print("  dir [path]           - List files")
        print("  cat <file>           - Display a file")
        print("  cp <source> <dest>   - Copy a file")
        print("  run <script.lua>     - Execute a Lua script")
        print("  Other input          - Execute Lua code")

    elseif command == "ver" then
        print("Tinos Version: " .. tostring(GetTinosVersion()))
        print("Lua Version: " .. _VERSION)
        print("Tinos, an OS powered by Lua, C# and determination")

    elseif command == "fdisk" then
        FDiskMain()

    elseif command == "ls" or command == "dir" then
        local path = arguments ~= "" and arguments or "/mnt"
        ListFiles(path, command == "ls")

    elseif command == "cat" then
        if arguments == "" then
            print("Usage: cat <file>")
        else
            ReadFile(arguments)
        end

    elseif command == "cp" then
        local source, destination =
            arguments:match("^(%S+)%s+(%S+)$")

        if source and destination then
            CopyFile(source, destination)
        else
            print("Usage: cp <source> <destination>")
        end

    elseif command == "run" then
        if arguments == "" then
            print("Usage: run <script.lua>")
        else
            local ok, source = pcall(LoadScript, arguments)

            if not ok then
                print("Unable to load script: " .. tostring(source))
            elseif type(source) ~= "string" then
                print("Unable to load script: invalid file contents")
            else
                ExecuteLua(source, "@" .. arguments)
            end
        end

    elseif command == "clear" then
        Clear()

    else
        ExecuteLua(input, "=LSH")
    end
end

print("LSH: Exited.")
