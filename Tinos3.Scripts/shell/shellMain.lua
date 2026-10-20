
print("LSH: Initialized.")

-- Main shell loop
while true do
    io.write("LSH> ")

    local input = ReadLine()

    -- Handle end-of-input
    if input == nil then
        break
    end

    -- Shut down the system
    if input == "poweroff" then
        Shutdown()
        break

    -- Reboot the system
    elseif input == "reboot" then
        Reboot()
        break

    -- Show help
    elseif input == "help" then
        print("Available commands:")
        print("  poweroff  - Shut down the system")
        print("  reboot    - Reboot the system")
        print("  help      - Show this help message")
        print("  ver       - Show Tinos3 and Lua versions")
        print("  fdisk     - Run the FDisk utility")
        print("  clear     - Clear the screen")
        print("  ls        - List directory contents")
        print("  dir       - List directory contents")
        print("  cat       - Display the contents of a file")
        print("  cp        - Copy a file")
        print("  Any other input will be executed as Lua code.")

    -- Show version information
    elseif input == "ver" then
        print("Tinos Version: " .. tostring(GetTinosVersion()))
        print("Lua Version: " .. _VERSION)
        print("Tinos, an OS powered by Lua, C# and determination")

    -- Run FDisk
    elseif input == "fdisk" then
        FDiskMain()

    -- List directory contents
    elseif input == "ls" then
        ListDirectory("/mnt", true)
    elseif input == "dir" then
        ListDirectory("/mnt", false)

    -- Read a file
    elseif input == "cat" then
        io.write("Enter file path: ")
        local filePath = ReadLine()

        if filePath ~= nil and filePath ~= "" then
            ReadFile(filePath)
        else
            print("No file path specified.")
        end

    -- Copy a file
    elseif input == "cp" then
        io.write("Enter source file path: ")
        local sourcePath = ReadLine()

        if sourcePath == nil or sourcePath == "" then
            print("No source file specified.")
        else
            io.write("Enter destination file path: ")
            local destPath = ReadLine()

            if destPath == nil or destPath == "" then
                print("No destination file specified.")
            else
                CopyFile(sourcePath, destPath)
            end
        end

    -- Clear the screen
    elseif input == "clear" then
        Clear()

    -- Skip empty input
    elseif input == "" then
        -- Nothing to do

    -- Execute arbitrary Lua code
    else
        local chunk, compileError = load(input)

        if chunk then
            local ok, result = pcall(chunk)

            if not ok then
                print("Lua runtime error: " .. tostring(result))
            elseif result ~= nil then
                print(result)
            end
        else
            print("Lua syntax error: " .. tostring(compileError))
        end
    end
end