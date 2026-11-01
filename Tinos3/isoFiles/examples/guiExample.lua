function Message_box_popup()
    print("Hello from the GUI")
end

function gui_example()
    local windowId = CreateWindow("Lua app", 80, 80, 320, 180)
    AddLabel(windowId, 20, 50, "This window was created from Lua")
    AddButton(windowId, 20, 90, 180, 40, "Greet", "Message_box_popup")
end

startx("gui_example")