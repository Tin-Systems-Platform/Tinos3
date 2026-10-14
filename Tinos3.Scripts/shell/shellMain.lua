print("LSH: Initialized.")

-- Main shell loop
while true do
    io.write("LSH> ")

    local input = ReadLine()


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
        print("  ver       - Show Tinos and Lua versions")
        print("  fdisk     - Run the FDisk utility")
        print("  clear     - Clear the screen")
        print("  Any other input will be executed as Lua code.")

    -- Show version information
    elseif input == "ver" then
        print("Tinos Version: " .. GetTinosVersion())
        print("Lua Version: " .. _VERSION)
        print("Tinos, an OS powered by Lua, C# and determination")

    elseif input == "fdisk" then
        FDiskMain()

    elseif input == "clear" then
        Clear()

    -- Skip empty input
    elseif input == "" then
        -- Nothing to do

    -- Execute arbitrary Lua code
    else
        load(input)
    end
end