using Cosmos.Executable.Lua;
using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Input;
using System;
using System.Drawing;
using System.IO;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Api.Utilities;
using Tinos3.Graphical.Apps.Clock;
using Tinos3.Graphical.Apps.System.About;
using Tinos3.Graphical.Apps.System.Calc;
using Tinos3.Graphical.Apps.System.PowerManagement;
using Tinos3.Graphical.Apps.System.Run;
using Tinos3.Graphical.Apps.System.Notepad;
using Tinos3.Graphical.Apps.System.FileExplorer;

namespace Tinos3.Graphical.Desktop.StartMenu
{
    public class StartMenu : IWindowInputHandler
    {
        private readonly ButtonWidget _clockAppLaunch;

        private readonly ButtonWidget _aboutAppLaunch;

        private readonly ButtonWidget _powerMgrAppLaunch;

        private readonly ButtonWidget _RunAppLaunch;

        private readonly ButtonWidget _calcAppLaunch;

        private readonly ButtonWidget _notepadAppLaunch;

        private readonly ButtonWidget _fileExplorerAppLaunch;

        private readonly Action _closeStartMenu;

        private readonly int _x;
        private readonly int _y;

        private Window _window;

        public StartMenu(WindowManager windowManager, int x, int y, Action closeStartMenu)
        {
            _window = new Window();
            _x = x;
            _y = y;
            _closeStartMenu = closeStartMenu;


            _clockAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 35,
                Width = 200,
                Height = 40,
                Text = "Open Clock App",
                OnClick = () =>
                {
                    Window clockWindow = new Window(100, 100, 300, 200, "Clock", new ClockContent());
                    windowManager.AddWindow(clockWindow);
                    _closeStartMenu();
                }
            };

            _aboutAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 85,
                Width = 200,
                Height = 40,
                Text = "About Tinos3",
                OnClick = () =>
                {
                    AboutAppContent aboutContent = new AboutAppContent(windowManager.RemoveWindow);
                    Window aboutWindow = new Window(150, 150, 700, 300, "About Tinos3", aboutContent);
                    aboutContent.SetWindow(aboutWindow);
                    windowManager.AddWindow(aboutWindow);
                    _closeStartMenu();

                }
            };

            _powerMgrAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 135,
                Width = 200,
                Height = 40,
                Text = "Power Management",
                OnClick = () =>
                {
                    Window powerWindow = new Window(150, 150, 400, 250, "Power Management", new PowerManagementContent());
                    windowManager.AddWindow(powerWindow);
                    _closeStartMenu();
                }
            };


            _RunAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 185,
                Width = 200,
                Height = 40,
                Text = "Run",
                OnClick = () =>
                {
                    RunContent runContent = new RunContent(command => ExecuteRunCommand(command, windowManager));
                    Window runWindow = new Window(200, 200, 420, 150, "Run", runContent);
                    windowManager.AddWindow(runWindow);
                    _closeStartMenu();
                }
            };

            _calcAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 235,
                Width = 200,
                Height = 40,
                Text = "Calculator",
                OnClick = () =>
                {
                    Window calcWindow = new Window(200, 200, 310, 400, "Calculator", new CalcContent());
                    windowManager.AddWindow(calcWindow);
                    calcWindow.WindowTitle("Calculator");
                    _closeStartMenu();
                }
            };

            _notepadAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 285,
                Width = 200,
                Height = 40,
                Text = "Notepad",
                OnClick = () =>
                {
                    windowManager.AddWindow(new Window(100, 100, 540, 400, "Notepad", new NotepadContent(windowManager.AddWindow, windowManager.RemoveWindow)));
                    _closeStartMenu();
                }
            };

            _fileExplorerAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 335,
                Width = 200,
                Height = 40,
                Text = "File Explorer",
                OnClick = () =>
                {
                    windowManager.AddWindow(new Window(100, 100, 560, 460, "File Explorer", new FileExplorerContent(windowManager.AddWindow, windowManager.RemoveWindow)));
                    _closeStartMenu();
                }
            };
        }

        private void ExecuteRunCommand(string rawCommand, WindowManager windowManager)
        {
            if (string.IsNullOrWhiteSpace(rawCommand))
            {
                return;
            }

            string command = rawCommand.Trim();

            if (command.StartsWith("run ", StringComparison.OrdinalIgnoreCase))
            {
                ExecuteLuaFile(command[4..].Trim());
                return;
            }

            switch (command.ToLowerInvariant())
            {
                case "clock":
                    windowManager.AddWindow(new Window(100, 100, 300, 200, "Clock", new ClockContent()));
                    break;

                case "about":
                    AboutAppContent aboutContent = new AboutAppContent(windowManager.RemoveWindow);
                    Window aboutWindow = new Window(150, 150, 700, 300, "About Tinos3", aboutContent);
                    aboutContent.SetWindow(aboutWindow);
                    windowManager.AddWindow(aboutWindow);
                    break;

                case "power":
                case "powermanagement":
                    windowManager.AddWindow(new Window(150, 150, 400, 250, "Power Management", new PowerManagementContent()));
                    break;

                case "shutdown":
                    Power.Shutdown();
                    break;

                case "reboot":
                case "restart":
                    Power.Reboot();
                    break;

                case "halt":
                    Power.Halt();
                    break;

                case "panic":
                    PanicHandler.Panic("Intentional panic from Run command");
                    break;

                case "calc":
                case "calculator":
                    windowManager.AddWindow(new Window(200, 200, 310, 400, "Calculator", new CalcContent()));
                    CalcContent calc = new CalcContent();
                    calc.WindowTitle("Calculator");
                    break;

                default:
                    Console.WriteLine($"[Run] Unknown command: {command}");
                    Console.WriteLine("[Run] Built-in: clock, about, power, shutdown, reboot, halt, panic, run <path>");
                    break;
            }
        }

        private static void ExecuteLuaFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("[Run] Usage: run <path>");
                return;
            }

            if (!File.Exists(path))
            {
                Console.WriteLine($"[Run] Lua file not found: {path}");
                return;
            }

            try
            {
                string script = File.ReadAllText(path);
                LuaInterpreter lua = new LuaInterpreter();
                lua.DoString(script);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Run] Failed to execute Lua file '{path}': {ex.Message}");
            }
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            // Close the start menu when clicking outside of it.
            bool isMouseXInside = mouseX >= _x && mouseX <= _x + 400;
            bool isMouseYInside = mouseY >= _y && mouseY <= _y + 500;

            if (leftButtonDown && (!isMouseXInside || !isMouseYInside))
            {
                _closeStartMenu();
                return;
            }

            _clockAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _aboutAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _powerMgrAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _RunAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _calcAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _notepadAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _fileExplorerAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public void RenderStartMenu(Canvas canvas)
        {
            _window.IsActive = false; // Set the window that was before start menu to inactive so it is accurate like in Windows. This is important for the window manager to know which window is active and which is not.
            // Render the start menu background
            canvas.DrawFilledRectangle(Color.LightGray, _x, _y, 400, 500);
            // Render the start menu items
            Label startMenuLabel = new Label
            {
                X = _x + 10,
                Y = _y + 10,
                Text = "Start Menu"
            };
            startMenuLabel.Draw(canvas);
            _clockAppLaunch.Draw(canvas);
            _aboutAppLaunch.Draw(canvas);
            _powerMgrAppLaunch.Draw(canvas);
            _RunAppLaunch.Draw(canvas);
            _calcAppLaunch.Draw(canvas);
            _notepadAppLaunch.Draw(canvas);
            _fileExplorerAppLaunch.Draw(canvas);
            // Additional menu items can be added here
        }
    }
}
