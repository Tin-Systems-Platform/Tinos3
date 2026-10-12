print("LSH: Initialized.")

-- Main shell loop
while true do
    io.write("LSH> ")

    local input = ReadLine()

    -- Exit the shell
    if input == "exit" then
        break

    -- Shut down the system
    elseif input == "poweroff" then
        Shutdown()
        break

    -- Reboot the system
    elseif input == "reboot" then
        Reboot()
        break

    -- Show help
    elseif input == "help" then
        print("Available commands:")
        print("  exit      - Exit the shell")
        print("  poweroff  - Shut down the system")
        print("  reboot    - Reboot the system")
        print("  help      - Show this help message")
        print("  ver       - Show Tinos and Lua versions")
        print("  Any other input will be executed as Lua code.")

    -- Show version information
    elseif input == "ver" then
        print("Tinos Version: " .. GetTinosVersion())
        print("Lua Version: " .. _VERSION)
        print("Tinos, an OS powered by Lua, C# and determination")

    -- Skip empty input
    elseif input == "" then
        -- Nothing to do

    -- Execute arbitrary Lua code
    else
        DoString(input)
    end
end