using Cosmos.Executable.Lua;
using Tinos3.Abstractions.Lua.Tinos3.Helpers;

namespace Tinos3.Abstractions.Lua.Tinos3.Registers
{
    class RegisterGraphicalApi
    {
        public static void RegisterGraphicalApiToLuaInterpeter(ILuaState lua)
        {
            Console.WriteLine("[LUA]: Registering Graphical API functions to Lua interpreter...");

            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.Graphical.DesktopInitialize, "startx");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.CreateWindow, "CreateWindow");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.AddLabel, "AddLabel");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.AddButton, "AddButton");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.AddTextBox, "AddTextBox");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.GetTextBoxText, "GetTextBoxText");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.SetWindowTitle, "SetWindowTitle");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.CloseWindow, "CloseWindow");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.MessageBox, "MessageBox");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.GraphicalLuaApi.OpenFileDialog, "OpenFileDialog");

            Console.WriteLine("[LUA]: Graphical API functions registered.");
        }
    }
}
