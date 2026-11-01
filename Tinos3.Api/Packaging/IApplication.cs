using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Api.Packaging
{
    /// <summary>Represents a runnable application.</summary>
    public interface IApplication
    {
        /// <summary>Gets the name of the application.</summary>
        string Name { get; }

        /// <summary>Gets the version of the application.</summary>
        string Version { get; }

        /// <summary>Gets the description of the application.</summary>
        string Description { get; }

        /// <summary>Runs the application. Management of the application's lifecycle is the responsibility of the caller.</summary>
        void Run();

        /// <summary>Stops the application and ends its execution from the system. Management of the application's lifecycle is the responsibility of the caller.</summary>
        void Stop();

        /// <summary>Runs some initialization code before the application starts.</summary>
        void Initialize();

        bool IsRunning { get; }

        bool IsInitialized { get; }

        bool IsStopped { get; }

        bool HasGraphicalContent { get; }
    }
}
