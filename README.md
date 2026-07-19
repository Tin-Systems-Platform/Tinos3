# Tinos3
Tinos3 is an Operating System made in C#, powered by the Cosmos OS gen3.

## Building
### Prerequisites
Here's a list of requirements for compiling.

- [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Cosmos OS gen3](https://github.com/valentinbreiz/nativeaot-patcher)

### Build
#### Windows
If you're in windows, You can use the `build.cmd` after installing the tools.

```Bash
build.cmd
```

Or you could manually make the ISO image by doing the same command as in Linux or MacOS.

#### Linux or MacOS
```Bash
cd Tinos3/ #This is required or you would need to modify the dotnet command

dotnet publish Tinos3.csproj -c Debug -r linux-x64 -p:CosmosArch=x64
```

## Development
Relisticly you can use any code editor, but these are what WE recommend.

### Windows
We use and recommend using Microsoft Visual Studio 2026

### Linux or MacOS
Because Visual studio doesn't support Linux and Microsoft killed the Visual Studio for mac, we recommend to use vscode.

## History of Tinos
Tinos started as a OS meant for me to experiment around. Don't ask how it ended. 

Later around 2023, Tinos version 2 was created and development continued on it up until 2025 when the projects discontinuation was announced waiting for Cosmos os gen3.

Now about half way through 2026, Tinos 3 is now in official development onwards to 0.1. we'll see what the future holds for tinos. We're exicited as for what we're working on.

We hope that you enjoy using Tinos 3