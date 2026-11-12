local windowId
local nameTextBoxId

function greet_from_gui()
    local name = GetTextBoxText(windowId, nameTextBoxId)
    if name == "" then
        name = "Lua user"
    end
    MessageBox("Greeting", "Hello, " .. name .. "!", "information")
end

function open_file_dialog()
    OpenFileDialog("file_selected")
end

function file_selected(path)
    MessageBox("File selected", path, "information")
end

function gui_example()
    windowId = CreateWindow("Lua app", 80, 80, 360, 230)
    AddLabel(windowId, 20, 45, "Enter your name:")
    nameTextBoxId = AddTextBox(windowId, 20, 75, 250, 32, "")
    AddButton(windowId, 20, 120, 140, 38, "Greet", "greet_from_gui")
    AddButton(windowId, 175, 120, 160, 38, "Open file...", "open_file_dialog")
end

startx("gui_example")
