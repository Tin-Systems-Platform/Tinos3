print("LSH: Initialized.")

-- LSH: The example shell for tinos3's custom lua APIs registered via C#.
while true do
    io.write("lsh> ")

    local input = ReadLine()
    -- Exit out of LSH
    if input == "exit" then
        break
    end
    
    -- Shutdown the system
    if input == "poweroff" then
        Shutdown()
        break
    end
    -- Reboot the machine
    if input == "reboot" then
        Reboot()
        break
    end

    -- Help command
    if input == "help" then
        print("Available commands:")
        print("  exit      - Exit the shell")
        print("  poweroff  - Shutdown the system")
        print("  reboot    - Reboot the system")
        print("  help      - Show this help message")
        print("  Any other input will be executed as Lua code.")
    end
    -- Execute the input as Lua code here
    DoString(input)
end