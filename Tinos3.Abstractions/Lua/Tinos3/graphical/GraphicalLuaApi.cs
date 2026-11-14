using Cosmos.Executable.Lua;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.FileDialog;
using Tinos3.Api.Graphical.Utils.MsgBox;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Graphical.Desktop;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical;

/// <summary>Lua-callable window and widget operations for consumer GUI apps.</summary>
public static class GraphicalLuaApi
{
    private sealed class WindowEntry
    {
        public int Id { get; init; }
        public Window Window { get; init; }
        public LuaWindowContent? Content { get; init; }
    }

    private static readonly List<WindowEntry> Windows = new();
    private static WindowManager _windowManager;
    private static ILuaState _lua;
    private static int _nextWindowId = 1;

    public static void Initialize(ILuaState lua, WindowManager windowManager)
    {
        _lua = lua;
        _windowManager = windowManager;
        Windows.Clear();
        _nextWindowId = 1;
    }

    public static int CreateWindow(ILuaState lua)
    {
        if (!IsInitialized()) return PushError(lua, "The desktop is not running.");
        if (!ReadString(lua, 1, out string title) || title.Length == 0
            || !ReadInt(lua, 2, out int x) || !ReadInt(lua, 3, out int y)
            || !ReadPositiveInt(lua, 4, out int width) || !ReadPositiveInt(lua, 5, out int height))
        {
            return PushError(lua, "Usage: CreateWindow(title, x, y, width, height)");
        }

        LuaWindowContent content = new();
        Window window = new(x, y, width, height, title, content);
        int id = _nextWindowId++;
        TrackWindow(window, content, id);
        lua.PushInteger(id);
        return 1;
    }

    public static int AddLabel(ILuaState lua)
    {
        if (!TryGetWindow(lua, 1, out WindowEntry entry) || entry.Content == null
            || !ReadInt(lua, 2, out int x) || !ReadInt(lua, 3, out int y)
            || !ReadString(lua, 4, out string text))
        {
            return PushError(lua, "Usage: AddLabel(windowId, x, y, text)");
        }

        entry.Content.AddLabel(x, y, text);
        entry.Window.IsDirty = true;
        lua.PushBoolean(true);
        return 1;
    }

    public static int AddButton(ILuaState lua)
    {
        if (!TryGetWindow(lua, 1, out WindowEntry entry) || entry.Content == null
            || !ReadInt(lua, 2, out int x) || !ReadInt(lua, 3, out int y)
            || !ReadPositiveInt(lua, 4, out int width) || !ReadPositiveInt(lua, 5, out int height)
            || !ReadString(lua, 6, out string text) || !ReadString(lua, 7, out string callbackName)
            || callbackName.Length == 0)
        {
            return PushError(lua, "Usage: AddButton(windowId, x, y, width, height, text, callbackName)");
        }

        ButtonWidget button = new()
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Text = text,
            OnClick = () => InvokeLuaCallback(callbackName)
        };

        entry.Content.AddButton(button);
        entry.Window.IsDirty = true;
        lua.PushBoolean(true);
        return 1;
    }

    public static int SetWindowTitle(ILuaState lua)
    {
        if (!TryGetWindow(lua, 1, out WindowEntry entry) || !ReadString(lua, 2, out string title))
        {
            return PushError(lua, "Usage: SetWindowTitle(windowId, title)");
        }

        entry.Window.WindowTitle(title);
        lua.PushBoolean(true);
        return 1;
    }

    public static int CloseWindow(ILuaState lua)
    {
        if (!IsInitialized() || !ReadInt(lua, 1, out int id))
        {
            return PushError(lua, "Usage: CloseWindow(windowId)");
        }

        WindowEntry entry = FindWindow(id);
        if (entry == null) return PushError(lua, "No GUI window has that id.");

        _windowManager.RemoveWindow(entry.Window);
        Windows.Remove(entry);
        lua.PushBoolean(true);
        return 1;
    }

    public static int AddTextBox(ILuaState lua)
    {
        if (!TryGetWindow(lua, 1, out WindowEntry entry) || entry.Content == null
            || !ReadInt(lua, 2, out int x) || !ReadInt(lua, 3, out int y)
            || !ReadPositiveInt(lua, 4, out int width) || !ReadPositiveInt(lua, 5, out int height)
            || (!lua.IsNoneOrNil(6) && !ReadString(lua, 6, out _)))
        {
            return PushError(lua, "Usage: AddTextBox(windowId, x, y, width, height, [initialText])");
        }

        TextBox textBox = new() { X = x, Y = y, Width = width, Height = height };
        if (!lua.IsNoneOrNil(6))
        {
            ReadString(lua, 6, out string initialText);
            textBox.SetText(initialText);
        }

        int textBoxId = entry.Content.AddTextBox(textBox);
        entry.Window.IsDirty = true;
        lua.PushInteger(textBoxId);
        return 1;
    }

    public static int GetTextBoxText(ILuaState lua)
    {
        if (!TryGetWindow(lua, 1, out WindowEntry entry) || entry.Content == null
            || !ReadInt(lua, 2, out int textBoxId)
            || !entry.Content.TryGetTextBox(textBoxId, out TextBox textBox))
        {
            return PushError(lua, "Usage: GetTextBoxText(windowId, textBoxId)");
        }

        lua.PushString(textBox.Text);
        return 1;
    }

    public static int MessageBox(ILuaState lua)
    {
        if (!IsInitialized() || !ReadString(lua, 1, out string title)
            || !ReadString(lua, 2, out string message))
        {
            return PushError(lua, "Usage: MessageBox(title, message, [type])");
        }

        MsgBoxType type = MsgBoxType.Information;
        if (!lua.IsNoneOrNil(3))
        {
            if (!ReadString(lua, 3, out string typeName))
            {
                return PushError(lua, "MessageBox type must be 'information', 'warning', or 'error'.");
            }

            if (string.Equals(typeName, "warning", StringComparison.OrdinalIgnoreCase)) type = MsgBoxType.Warning;
            else if (string.Equals(typeName, "error", StringComparison.OrdinalIgnoreCase)) type = MsgBoxType.Error;
            else if (!string.Equals(typeName, "information", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(typeName, "info", StringComparison.OrdinalIgnoreCase))
            {
                return PushError(lua, "MessageBox type must be 'information', 'warning', or 'error'.");
            }
        }

        int width = Math.Min(360, _windowManager.ScreenWidth);
        int height = Math.Min(170, _windowManager.ScreenHeight);
        MsgBoxContent content = new(title, message, type, _windowManager.RemoveWindow);
        Window window = new(
            (_windowManager.ScreenWidth - width) / 2,
            (_windowManager.ScreenHeight - height) / 2,
            width,
            height,
            title,
            content);
        content.SetWindow(window);
        int id = _nextWindowId++;
        TrackWindow(window, null, id);
        lua.PushInteger(id);
        return 1;
    }

    public static int OpenFileDialog(ILuaState lua)
    {
        if (!IsInitialized() || !ReadString(lua, 1, out string callbackName) || callbackName.Length == 0)
        {
            return PushError(lua, "Usage: OpenFileDialog(callbackName)");
        }

        int width = Math.Min(500, _windowManager.ScreenWidth);
        int height = Math.Min(400, _windowManager.ScreenHeight);
        Window window = null!;
        FileDialogContent content = new(
            closedWindow => _windowManager.RemoveWindow(closedWindow),
            path => InvokeLuaCallback(callbackName, path));
        window = new Window(
            (_windowManager.ScreenWidth - width) / 2,
            (_windowManager.ScreenHeight - height) / 2,
            width,
            height,
            "Open File",
            content);
        content.SetWindow(window);
        int id = _nextWindowId++;
        TrackWindow(window, null, id);
        lua.PushInteger(id);
        return 1;
    }

    private static void TrackWindow(Window window, LuaWindowContent? content, int id)
    {
        Windows.Add(new WindowEntry { Id = id, Window = window, Content = content });
        _windowManager.AddWindow(window);
    }

    public static void InvokeStartupCallback(string callbackName)
    {
        if (!string.IsNullOrEmpty(callbackName))
        {
            InvokeLuaCallback(callbackName);
        }
    }

    private static bool InvokeLuaCallback(string callbackName, string? argument = null)
    {
        if (_lua == null) return false;

        int top = _lua.GetTop();
        LuaType type = _lua.GetGlobal(callbackName);
        if (type != LuaType.LUA_TFUNCTION)
        {
            Console.WriteLine("[GUI]: Lua callback is not a function: " + callbackName);
            _lua.SetTop(top);
            return false;
        }

        int argumentCount = 0;
        if (argument != null)
        {
            _lua.PushString(argument);
            argumentCount = 1;
        }

        ThreadStatus status = _lua.PCall(argumentCount, 0, 0);
        if (status != ThreadStatus.LUA_OK)
        {
            Console.WriteLine("[GUI]: Lua callback failed: " + _lua.ToString(-1));
            _lua.SetTop(top);
            return false;
        }

        _lua.SetTop(top);
        return true;
    }

    private static bool IsInitialized() => _lua != null && _windowManager != null;

    private static bool TryGetWindow(ILuaState lua, int index, out WindowEntry entry)
    {
        entry = null;
        if (!IsInitialized() || !ReadInt(lua, index, out int id)) return false;
        entry = FindWindow(id);
        return entry != null;
    }

    private static WindowEntry FindWindow(int id)
    {
        for (int i = Windows.Count - 1; i >= 0; i--)
        {
            WindowEntry entry = Windows[i];
            if (!_windowManager.ContainsWindow(entry.Window))
            {
                Windows.RemoveAt(i);
                continue;
            }

            if (entry.Id == id) return entry;
        }

        return null;
    }

    private static bool ReadInt(ILuaState lua, int index, out int value)
    {
        value = 0;
        if (!lua.IsInteger(index)) return false;
        long raw = lua.ToInteger(index);
        if (raw < int.MinValue || raw > int.MaxValue) return false;
        value = (int)raw;
        return true;
    }

    private static bool ReadPositiveInt(ILuaState lua, int index, out int value)
    {
        return ReadInt(lua, index, out value) && value > 0;
    }

    private static bool ReadString(ILuaState lua, int index, out string value)
    {
        value = null;
        if (!lua.IsString(index)) return false;
        value = lua.ToString(index);
        return value != null;
    }

    private static int PushError(ILuaState lua, string message)
    {
        lua.PushNil();
        lua.PushString(message);
        return 2;
    }
}
