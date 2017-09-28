RecoveryDriveBuilderPlus
========================

*Build Windows 10 Recovery Drive USB with more options than the original from Microsoft*

(c) 2017 Michael Schierl

Introduction
------------

When your Windows 10 stops booting (or you installed another operating system
that broke the bootloader), it is very useful to have a recovery environment
available that can be used to repair it. Up to Windows 8.1, the easiest and
most useful way was to have an installation DVD around. But as Windows 10 gets
updated about twice a year, this means a lot of wasted blank DVDs. Also, many
recent machines do not have a DVD drive any more, but are bootable from USB
only.

Microsoft noticed this too and provides their `RecoveryDrive.exe` utility
which enabled everyone to easily create a recovery USB key. It will
automatically slipstream required drivers into the image, therefore making
sure that the image boots on the machine it was created - also having the
disadvantage that technically you need a separate recovery drive for every PC
you own. It also has a few more disadvantages: First of all, it will always
format and erase the USB key, so if you stored other data on it, you have to
copy them off and back again whenever you want to refresh your recovery drive
(at least after each Windows 10 release). Second, you can only add one single
recovery drive to the USB key, so you need a separate USB key for every
recovery drive. Third, it is impossible to create a generic recovery drive
without slipstreamed drivers. Such a drive could still boot the majority of
machines in a way that is somehow useful; instead you have to try the recovery
drives you have and hope no slipstreamed driver has an incompatibility with
your devices. Last, it is also impossible to create a recovery drive for a
Windows version/edition you do not have installed (you'd have to install it to
a VM and create the recovery drive there).

You can work around most of these these limitations by creating your recovery
drive on a spare USB key, later copy the images to your real USB key and update
the Boot Configuration Data manually; but that is not fun.

Features
--------

RecoveyDriveBuilderPlus is a wizard (like the original `RecoveryDrive.exe`),
providing the following features.

- Choose to take the recovery drive from the current installation (including
  slipstreamed drivers), or from an installation media
  (Install.Wim/Install.ESD), or from an installation ISO (with choice of
  edition for multi-edition install media)
- Decide whether to overwrite the boot menu or to add the new recovery drive
  to an existing boot menu
- Include Windows Memory Diagnostic on your recovery drive
- Option to choose an alternative directory layout optimized for chainloading
  the recovery drive boot loader from another boot loader (e.g. GRUB2)
- Both EFI and BIOS booting supported

It **lacks** the following "features" of `RecoveryDrive.exe`:

- Cannot wipe your data on the USB drive or reformat it to FAT32
- Cannot install BIOS bootsector or mark partition as active/bootable

If you are missing these features (or having trouble getting your USB key
bootable), try running `RecoveryDrive.exe` first and then overwriting the
bootloader with RecoveryDriveBuilderPlus.


Requirements
------------

RecoveryDriveBuilderPlus should work out of the box on any Windows 10
machine (32-bit or 64-bit). If not, feel free to report a bug.

RecoveryDriveBuilderPlus needs administrative privileges to run (to
access the Recovery Partition, access WIM files and update Boot Configuration
Data). It embeds a manifest so it will ask for elevation automatically if UAC
is enabled.

RecoveryDriveBuilderPlus requires the .NET Framework Version 4 (or
above) to run, but that is already included in Windows 10.

To compile the source code yourself, you will need Microsoft Visual Studio
2017 Community (or higher).
