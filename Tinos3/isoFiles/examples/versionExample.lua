local version = GetTinosVersion()
print("Tinos version: " .. version)

print("Showing if SetTinosVersion is working...")
SetTinosVersion("1.0.0 backdoored version")
local newVersion = GetTinosVersion()
print("New Tinos version: " .. newVersion)