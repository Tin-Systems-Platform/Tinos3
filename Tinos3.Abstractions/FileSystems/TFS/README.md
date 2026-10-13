# TFS (Tin File System)
TFS is a custom file system designed and developed for the Tinos3 project. TFS is also VERY experimental.

It is built on top of the FAT32 file system and is a "proprietary" component of Tinos3.

## Usage
TFS is part of the Tinos3 project's Tinos3.Abstractions assembly. It is used to manage files and directories within the Tinos3 OS.

## Virtual Disk reading on hosts
TFS can be read on host systems using the Tinos3 Virtual Disk Reader tool. This tool allows users to mount and explore TFS virtual disks on their host operating system.

## Why TFS?
TFS was created to be a custom filesystem that meets the specific needs of the Tinos3 project.

It provides a simple and efficient way to manage files and directories within the Tinos3 OS, while also allowing for easy integration with host systems through the Virtual Disk Reader tool.

## License
TFS follows the same licensing terms as the Tinos3 project. Please refer to the Tinos3 license for more information.

Tinos3 is licensed under the MIT License. You can find the full license text in the LICENSE file of the Tinos3 repository.

## Third-Party Code

Parts of TFS are derived from CosmosOS, which is licensed under the BSD 3-Clause License. The full license text is available in the CosmosOS repository's `LICENSE` file and in this repository under `THIRD-PARTY-NOTICES/CosmosOS-BSD-3-Clause.txt`.

Tinos3-specific modifications and original contributions remain subject to the applicable Tinos3 licensing terms. Copyright and license notices for derived code are preserved where applicable.