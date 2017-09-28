using Microsoft.Wim;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.XPath;

namespace RecoveryDriveBuilderPlus
{
    public class BuildEngine : IDisposable
    {
        public DriveInfo Drive { get; private set; }
        public bool BootloaderPresent { get; private set; }
        public bool SkipLocationSelection { get; private set; }
        public bool UseDeeperLocation { get; set; }
        public bool OverwriteBCD { get; set; }
        public bool OverwriteBootloader { get; set; }

        public BuildEngine(DriveInfo drive)
        {
            Drive = drive;
            BootloaderPresent = File.Exists(Path.Combine(drive.Name, @"boot\BCD"));
            SkipLocationSelection = File.Exists(Path.Combine(drive.Name, @"usb-modboot\core-module.dat"));
            UseDeeperLocation = SkipLocationSelection || File.Exists(Path.Combine(drive.Name, @"Boot\bootmgr")) || File.Exists(Path.Combine(drive.Name, @"efi\microsoft\bootx64.efi"));
            OverwriteBCD = OverwriteBootloader = !BootloaderPresent;
            Unload();
        }

        public void Dispose()
        {
            Unload();
        }

        public string[] AvailableEditions { get; private set; }
        public string SourceISOName { get; private set; }
        public DriveInfo SourceISODrive { get; private set; }
        public string SourceWIM { get; private set; }
        public string SourceWIMMountPoint { get; private set; }
        public string SourceFile { get; private set; }
        public string BootloaderSourceDirectory { get; private set; }
        public string BootMenuEntryName { get; set; }
        public string DestinationFileName { get; set; }
        public string VersionInfo { get; private set; }
        public bool EfiSupported { get; private set; }
        public bool EfiUsed { get; set; }

        public void LoadFromWindowsInstallation()
        {
            Unload();
            BootloaderSourceDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Boot");

            // parse "reagentc /info" to find WinRe.Wim location
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "reagentc.exe";
            p.StartInfo.Arguments = "/info";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string line = output.Split('\r', '\n').First((l) => l.Contains(":  ") && l.Contains('\\'));
            SourceFile = new System.Text.RegularExpressions.Regex(".*:  +").Replace(line, "").Trim() + @"\WinRe.Wim"; ;

            // load edition name from Registry
            using (RegistryKey regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                AvailableEditions = new string[] { (string)regkey.GetValue("ProductName") };
            }
        }

        public void LoadFromExistingRecoveryWIM()
        {
            if (OverwriteBCD || OverwriteBootloader)
                throw new Exception("Existing WIM can only be added to existing menu/bootloader");
            Unload();
            AvailableEditions = new string[] { "Windows 10 (existing WIM)" };
            SourceFile = "(use existing file)";
            VersionInfo = "(unknown)";
            BootMenuEntryName = "Windows 10 Recovery xxxxx.x 64-Bit";
            DestinationFileName = "recoveryXXXXX.Xx64.wim";
            EfiSupported = EfiUsed = File.Exists(Path.Combine(Drive.Name, @"efi\microsoft\boot\BCD"));
        }

        public void LoadFromInstallWIM(string path)
        {
            Unload();
            SourceWIM = path;
            InitializeWIM();
        }

        public void LoadFromISO(string path)
        {
            Unload();
            SourceISOName = path;
            SafeFileHandle handle = null;
            VIRTUAL_STORAGE_TYPE openStorageType = new VIRTUAL_STORAGE_TYPE();
            openStorageType.DeviceId = VIRTUAL_STORAGE_TYPE_DEVICE_ISO;
            openStorageType.VendorId = VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT;
            int result = OpenVirtualDisk(ref openStorageType, SourceISOName, VIRTUAL_DISK_ACCESS_ATTACH_RO | VIRTUAL_DISK_ACCESS_GET_INFO, 0, IntPtr.Zero, out handle);
            if (result != 0)
                throw new Win32Exception(result);
            result = AttachVirtualDisk(handle, IntPtr.Zero, ATTACH_VIRTUAL_DISK_FLAG_READ_ONLY | ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME, 0, IntPtr.Zero, IntPtr.Zero);
            if (result != 0)
                throw new Win32Exception(result);
            StringBuilder sb = new StringBuilder(250);
            int sbSize = sb.Capacity;
            result = GetVirtualDiskPhysicalPath(handle, ref sbSize, sb);
            if (result != 0)
                throw new Win32Exception(result);
            String RawDevName = sb.ToString().Replace(@"\\.\", @"\DEVICE\").ToUpperInvariant();
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                sb = new StringBuilder(250);
                if (QueryDosDevice(di.Name.Replace("\\", ""), sb, 250) == 0)
                    throw new Win32Exception();
                if (sb.ToString().ToUpperInvariant().Equals(RawDevName))
                {
                    SourceISODrive = di;
                }
            }
            handle.Close();
            if (SourceISODrive == null)
                throw new Win32Exception("Mounted ISO not found");
            SourceWIM = Path.Combine(SourceISODrive.Name, @"sources\install.wim");
            if (!File.Exists(SourceWIM))
                SourceWIM = Path.Combine(SourceISODrive.Name, @"sources\install.esd");
            if (!File.Exists(SourceWIM))
                throw new Win32Exception("No Install.Wim found inside ISO");
            InitializeWIM();
        }

        private void InitializeWIM()
        {
            using (WimHandle fullWIM = WimgApi.CreateFile(SourceWIM, WimFileAccess.Read, WimCreationDisposition.OpenExisting, WimCreateFileOptions.Chunked, WimCompressionType.None))
            {
                XPathNodeIterator iterator = WimgApi.GetImageInformation(fullWIM).CreateNavigator().Select("/WIM/IMAGE/NAME");
                AvailableEditions = new string[iterator.Count];
                while (iterator.MoveNext())
                {
                    AvailableEditions[iterator.CurrentPosition - 1] = "[" + iterator.Current.SelectSingleNode("../@INDEX").Value + "] " + iterator.Current.Value;
                }
            }
        }

        private static readonly string[] FILES_TO_COPY_FROM_WIM = new string[]
        {
            @"Windows\System32\Recovery\Winre.wim", @"Windows\Boot\resources\bootres.dll", @"Windows\Boot\DVD\EFI\boot.sdi",
            @"Windows\Boot\PCAT\memtest.exe", @"Windows\Boot\EFI\memtest.efi", @"Windows\Boot\EFI\bootmgfw.efi",
            @"Windows\Boot\PCAT\bootmgr", @"Windows\Boot\DVD\PCAT\BCD", @"Windows\Boot\DVD\EFI\BCD",
        };

        public void SelectEdition(string editionName)
        {
            if (SourceFile == "(use existing file)")
                return;
            if (SourceWIMMountPoint != null)
            {
                UnmountWIM();
            }
            if (SourceWIM != null)
            {
                int pos = editionName.IndexOf("] ");
                int index = int.Parse(editionName.Substring(1, pos - 1));
                editionName = editionName.Substring(pos + 2);
                string wimTempFile = Path.GetTempFileName();
                SourceWIMMountPoint = wimTempFile + ".mountdir";
                Directory.CreateDirectory(wimTempFile + ".tempdir");
                Directory.CreateDirectory(SourceWIMMountPoint + @"\Windows\System32\Recovery");
                Directory.CreateDirectory(SourceWIMMountPoint + @"\Windows\Boot\resources");
                Directory.CreateDirectory(SourceWIMMountPoint + @"\Windows\Boot\fonts");
                Directory.CreateDirectory(SourceWIMMountPoint + @"\Windows\Boot\DVD\PCAT");
                Directory.CreateDirectory(SourceWIMMountPoint + @"\Windows\Boot\DVD\EFI");
                Directory.CreateDirectory(SourceWIMMountPoint + @"\Windows\Boot\PCAT");
                Directory.CreateDirectory(SourceWIMMountPoint + @"\Windows\Boot\EFI");
                using (WimHandle fullWIM = WimgApi.CreateFile(SourceWIM, WimFileAccess.Read | WimFileAccess.Mount, WimCreationDisposition.OpenExisting, WimCreateFileOptions.Chunked, WimCompressionType.None))
                {
                    WimgApi.SetTemporaryPath(fullWIM, wimTempFile + ".tempdir");
                    using (WimHandle singleWIM = WimgApi.LoadImage(fullWIM, index))
                    {
                        foreach (string filename in FILES_TO_COPY_FROM_WIM)
                        {
                            WimgApi.ExtractImagePath(singleWIM, filename, Path.Combine(SourceWIMMountPoint, filename));
                        }
                        foreach (string font in Directory.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Boot\fonts")))
                        {
                            try
                            {
                                WimgApi.ExtractImagePath(singleWIM, Path.Combine(@"Windows\Boot\fonts", Path.GetFileName(font)), Path.Combine(SourceWIMMountPoint, @"Windows\Boot\fonts", Path.GetFileName(font)));
                            }
                            catch { }
                        }
                        BootloaderSourceDirectory = Path.Combine(SourceWIMMountPoint, @"Windows\Boot");
                        SourceFile = Path.Combine(SourceWIMMountPoint, @"Windows\System32\Recovery\Winre.wim");
                    }
                }
            }

            // extract ntoskrnl.exe information
            bool is64bit;
            FileVersionInfo kernelVersion;
            string tempFile = Path.GetTempFileName();
            Directory.CreateDirectory(tempFile + ".tempdir");
            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                using (WimHandle fullWIM = WimgApi.CreateFile(SourceFile, WimFileAccess.Read, WimCreationDisposition.OpenExisting, WimCreateFileOptions.Chunked, WimCompressionType.None))
                {
                    WimgApi.SetTemporaryPath(fullWIM, tempFile + ".tempdir");
                    using (WimHandle singleWIM = WimgApi.LoadImage(fullWIM, 1))
                    {
                        WimgApi.ExtractImagePath(singleWIM, @"Windows\System32\ntoskrnl.exe", tempFile);
                        kernelVersion = FileVersionInfo.GetVersionInfo(tempFile);
                        try
                        {
                            WimgApi.ExtractImagePath(singleWIM, @"Program Files (x86)\desktop.ini", tempFile);
                            is64bit = true;
                        }
                        catch
                        {
                            is64bit = false;
                        }
                    }
                }
            }
            finally
            {
                Directory.Delete(tempFile + ".tempdir");
                File.Delete(tempFile);
            }

            // fill attributes
            int buildVersion = kernelVersion.FileBuildPart, revisionVersion = kernelVersion.FilePrivatePart;
            string bitness = is64bit ? "64-Bit" : "32-Bit";
            BootMenuEntryName = editionName + " Recovery " + buildVersion + "." + revisionVersion + " " + bitness + (SourceWIM == null ? " for " + Environment.MachineName : "");
            DestinationFileName = "recovery" + buildVersion + "." + revisionVersion + (is64bit ? "x64" : "x86") + (SourceWIM == null ? "_" + Environment.MachineName : "") + ".wim";
            VersionInfo = buildVersion + "." + revisionVersion + " (" + bitness + ")";
            EfiSupported = EfiUsed = is64bit &&
                (OverwriteBCD || File.Exists(Path.Combine(Drive.Name, @"efi\microsoft\boot\BCD"))) &&
                (OverwriteBootloader || File.Exists(Path.Combine(Drive.Name, @"efi\microsoft\boot\resources\bootres.dll")));
        }

        public void Unload()
        {
            if (SourceWIMMountPoint != null)
            {
                UnmountWIM();
            }
            if (SourceISODrive != null)
            {
                VIRTUAL_STORAGE_TYPE openStorageType = new VIRTUAL_STORAGE_TYPE();
                openStorageType.DeviceId = VIRTUAL_STORAGE_TYPE_DEVICE_ISO;
                openStorageType.VendorId = VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT;
                SafeFileHandle handle = null;
                int result = OpenVirtualDisk(ref openStorageType, SourceISOName, VIRTUAL_DISK_ACCESS_DETACH, 0, IntPtr.Zero, out handle);
                if (result != 0)
                    throw new Win32Exception(result);
                result = DetachVirtualDisk(handle, DETACH_VIRTUAL_DISK_FLAG_NONE, 0);
                if (result != 0)
                    throw new Win32Exception(result);
                handle.Close();
            }

            AvailableEditions = null;
            SourceISOName = null;
            SourceISODrive = null;
            SourceWIM = null;
            SourceFile = null;
            BootloaderSourceDirectory = null;
            BootMenuEntryName = null;
            DestinationFileName = null;
            VersionInfo = null;
            EfiSupported = false;
            EfiUsed = false;
        }

        private void UnmountWIM()
        {
            string wimTempFile = SourceWIMMountPoint.Substring(0, SourceWIMMountPoint.Length - 9);
            Directory.Delete(wimTempFile + ".mountdir", true);
            Directory.Delete(wimTempFile + ".tempdir");
            File.Delete(wimTempFile);
            SourceWIMMountPoint = null;
        }

        public string Summary
        {
            get
            {
                StringBuilder summaryBuilder = new StringBuilder();
                if (SourceISODrive != null)
                {
                    summaryBuilder.Append("Installer ISO File: " + SourceISOName + " (mounted as " + SourceISODrive.Name + ")\r\n");
                }
                if (SourceWIMMountPoint != null)
                {
                    summaryBuilder.Append("Installer WIM File: " + SourceWIM + " (mounted at " + SourceWIMMountPoint + ")\r\n");
                }
                summaryBuilder.Append("Recovery WIM File: " + SourceFile + "\r\n");
                summaryBuilder.Append("Recovery WIM File Version: " + VersionInfo + "\r\n\r\n");
                summaryBuilder.Append("Destination Drive: " + Drive.Name + "\r\n\r\n");

                if (OverwriteBootloader)
                {
                    summaryBuilder.Append("Bootloader is rebuilt from: " + BootloaderSourceDirectory + "\r\n");
                    if (UseDeeperLocation)
                    {
                        summaryBuilder.Append("Primary bootloader files are stored in alternative directory (for chainloading).\r\n");
                    }
                }
                else
                {
                    summaryBuilder.Append("Bootloader is not rebuilt.\r\n");
                }

                summaryBuilder.Append("Boot menu is " + (OverwriteBCD ? "" : "not ") + "rebuilt.");
                return summaryBuilder.ToString();
            }
        }

        private const string EFI_GUID_bootmgr = "{9dea862c-5cdd-4e70-acc1-f32b344d4795}";
        private const string EFI_GUID_ramdiskoptions = "{7619dcc8-fafe-11d9-b411-000476eba25f}";
        private const string EFI_GUID_bootloadersettings = "{6efb52bf-1766-41db-a6b3-0ee5eff72bd7}";
        private const string EFI_GUID_memdiag = "{b2721d73-1db4-4c62-bf78-c548a880142d}";

        public void Build()
        {
            if (SourceFile == null)
                throw new Exception("No recovery WIM file found.");
            if (OverwriteBootloader)
            {
                Directory.CreateDirectory(Path.Combine(Drive.Name, @"boot\resources"));
                Directory.CreateDirectory(Path.Combine(Drive.Name, @"boot\fonts"));
                File.Copy(Path.Combine(BootloaderSourceDirectory, @"resources\bootres.dll"), Path.Combine(Drive.Name, @"boot\resources\bootres.dll"), true);
                File.Copy(Path.Combine(BootloaderSourceDirectory, @"DVD\EFI\boot.sdi"), Path.Combine(Drive.Name, @"boot\boot.sdi"), true);
                foreach (string font in Directory.GetFiles(Path.Combine(BootloaderSourceDirectory, @"fonts")))
                    File.Copy(font, Path.Combine(Drive.Name, @"boot\fonts", Path.GetFileName(font)), true);
                File.Copy(Path.Combine(BootloaderSourceDirectory, @"PCAT\memtest.exe"), Path.Combine(Drive.Name, @"boot\memtest.exe"), true);
                if (EfiUsed)
                {
                    Directory.CreateDirectory(Path.Combine(Drive.Name, @"efi\microsoft\boot\resources"));
                    Directory.CreateDirectory(Path.Combine(Drive.Name, @"efi\microsoft\boot\fonts"));
                    File.Copy(Path.Combine(BootloaderSourceDirectory, @"resources\bootres.dll"), Path.Combine(Drive.Name, @"efi\microsoft\boot\resources\bootres.dll"), true);
                    foreach (string font in Directory.GetFiles(Path.Combine(BootloaderSourceDirectory, @"fonts")))
                        File.Copy(font, Path.Combine(Drive.Name, @"efi\microsoft\boot\fonts", Path.GetFileName(font)), true);
                    File.Copy(Path.Combine(BootloaderSourceDirectory, @"EFI\memtest.efi"), Path.Combine(Drive.Name, @"efi\microsoft\boot\memtest.efi"), true);
                    if (!UseDeeperLocation)
                        Directory.CreateDirectory(Path.Combine(Drive.Name, @"efi\boot"));
                    File.Copy(Path.Combine(BootloaderSourceDirectory, @"EFI\bootmgfw.efi"), Path.Combine(Drive.Name, UseDeeperLocation ? @"efi\microsoft\bootx64.efi" : @"efi\boot\bootx64.efi"), true);
                }
                File.Copy(Path.Combine(BootloaderSourceDirectory, @"PCAT\bootmgr"), Path.Combine(Drive.Name, UseDeeperLocation ? @"boot\bootmgr" : @"bootmgr"), true);
            }
            if (OverwriteBCD)
            {
                for (int bcdIndex = 0; bcdIndex < (EfiUsed ? 2 : 1); bcdIndex++)
                {
                    string storeName = Path.Combine(Drive.Name, bcdIndex == 0 ? @"boot\BCD" : @"efi\microsoft\boot\BCD");
                    File.Copy(Path.Combine(BootloaderSourceDirectory, bcdIndex == 0 ? @"DVD\PCAT\BCD" : @"DVD\EFI\BCD"), storeName, true);
                    string[] oldDisplayOrder;
                    using (ManagementObject entry = new ManagementObject(@"root\WMI", "BcdObject.Id=\"" + EFI_GUID_bootmgr + "\",StoreFilePath=\"" + storeName.Replace(@"\", @"\\") + "\"", null))
                    {
                        oldDisplayOrder = (string[])((ManagementBaseObject)(InvokeWMIMethod(entry, "GetElement", "Type", 0x24000001U).Properties["Element"].Value)).Properties["Ids"].Value;
                        InvokeWMIMethod(entry, "SetObjectListElement", "Type", 0x24000001U, "Ids", new string[] { EFI_GUID_memdiag });
                        InvokeWMIMethod(entry, "SetObjectElement", "Type", 0x23000003U, "Id", EFI_GUID_memdiag);
                    }
                    using (ManagementObject entry = new ManagementObject(@"root\WMI", "BcdObject.Id=\"" + EFI_GUID_memdiag + "\",StoreFilePath=\"" + storeName.Replace(@"\", @"\\") + "\"", null))
                    {
                        InvokeWMIMethod(entry, "SetStringElement", "Type", 0x12000002U, "String", bcdIndex == 0 ? @"\boot\memtest.exe" : @"\efi\microsoft\boot\memtest.efi");
                    }
                    using (ManagementObject store = new ManagementObject(@"root\WMI", "BcdStore.FilePath=\"" + storeName.Replace(@"\", @"\\") + "\"", null))
                    {
                        InvokeWMIMethod(store, "DeleteObject", "Id", oldDisplayOrder[0]);
                    }
                }
            }
            if (SourceFile != "(use existing file)")
            {
                if (SourceFile.StartsWith(@"\\?\GLOBALROOT"))
                {
                    SafeFileHandle fileHandle = new SafeFileHandle(CreateFile(SourceFile, FileAccess.Read, FileShare.Read, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero), true);
                    using (FileStream inStream = new FileStream(fileHandle, FileAccess.Read))
                    using (FileStream outStream = new FileStream(Path.Combine(Drive.Name, "boot", DestinationFileName), FileMode.Create, FileAccess.Write))
                    {
                        inStream.CopyTo(outStream);
                    }
                    fileHandle.Close();
                }
                else
                {
                    File.Copy(SourceFile, Path.Combine(Drive.Name, "boot", DestinationFileName), true);
                }
            }
            string newEntryGUID = "{" + Guid.NewGuid().ToString().ToLowerInvariant() + "}";
            for (int bcdIndex = 0; bcdIndex < (EfiUsed ? 2 : 1); bcdIndex++)
            {
                string storeName = Path.Combine(Drive.Name, bcdIndex == 0 ? @"boot\BCD" : @"efi\microsoft\boot\BCD");
                using (ManagementObject store = new ManagementObject(@"root\WMI", "BcdStore.FilePath=\"" + storeName.Replace(@"\", @"\\") + "\"", null))
                {
                    InvokeWMIMethod(store, "CreateObject", "Id", newEntryGUID, "Type", 0x10200003U);
                }
                using (ManagementObject entry = new ManagementObject(@"root\WMI", "BcdObject.Id=\"" + newEntryGUID + "\",StoreFilePath=\"" + storeName.Replace(@"\", @"\\") + "\"", null))
                {
                    InvokeWMIMethod(entry, "SetFileDeviceElement", "Type", 0x11000001U, "DeviceType", 4U, "AdditionalOptions", EFI_GUID_ramdiskoptions, "Path", @"\boot\" + DestinationFileName, "ParentDeviceType", 1U, "ParentAdditionalOptions", "", "ParentPath", "");
                    InvokeWMIMethod(entry, "SetFileDeviceElement", "Type", 0x21000001U, "DeviceType", 4U, "AdditionalOptions", EFI_GUID_ramdiskoptions, "Path", @"\boot\" + DestinationFileName, "ParentDeviceType", 1U, "ParentAdditionalOptions", "", "ParentPath", "");
                    InvokeWMIMethod(entry, "SetStringElement", "Type", 0x12000002U, "String", bcdIndex == 0 ? @"\windows\system32\boot\winload.exe" : @"\windows\system32\boot\winload.efi");
                    InvokeWMIMethod(entry, "SetStringElement", "Type", 0x12000004U, "String", BootMenuEntryName);
                    InvokeWMIMethod(entry, "SetStringElement", "Type", 0x12000005U, "String", @"en-US");
                    InvokeWMIMethod(entry, "SetObjectListElement", "Type", 0x14000006U, "Ids", new string[] { EFI_GUID_bootloadersettings });
                    InvokeWMIMethod(entry, "SetStringElement", "Type", 0x22000002U, "String", @"\windows");
                    InvokeWMIMethod(entry, "SetIntegerElement", "Type", 0x250000C2U, "Integer", 1UL);
                    InvokeWMIMethod(entry, "SetBooleanElement", "Type", 0x26000010U, "Boolean", true);
                    InvokeWMIMethod(entry, "SetBooleanElement", "Type", 0x26000022U, "Boolean", true);
                    InvokeWMIMethod(entry, "SetBooleanElement", "Type", 0x260000b0U, "Boolean", false);
                }
                using (ManagementObject entry = new ManagementObject(@"root\WMI", "BcdObject.Id=\"" + EFI_GUID_bootmgr + "\",StoreFilePath=\"" + storeName.Replace(@"\", @"\\") + "\"", null))
                {
                    string[] oldDisplayOrder = (string[])((ManagementBaseObject)(InvokeWMIMethod(entry, "GetElement", "Type", 0x24000001U).Properties["Element"].Value)).Properties["Ids"].Value;
                    string[] newDisplayOrder = new string[oldDisplayOrder.Length + 1];
                    Array.Copy(oldDisplayOrder, newDisplayOrder, oldDisplayOrder.Length);
                    newDisplayOrder[newDisplayOrder.Length - 1] = newEntryGUID;
                    InvokeWMIMethod(entry, "SetObjectListElement", "Type", 0x24000001U, "Ids", newDisplayOrder);
                }
                File.Delete(storeName + ".LOG");
                File.Delete(storeName + ".LOG1");
                File.Delete(storeName + ".LOG2");
            }
            Unload();
        }

        private static ManagementBaseObject InvokeWMIMethod(ManagementObject reference, string name, params object[] nameValuePairs)
        {
            var inParams = reference.GetMethodParameters(name);
            for (int i = 0; i < nameValuePairs.Length; i += 2)
            {
                inParams[(string)nameValuePairs[i]] = nameValuePairs[i + 1];
            }
            return reference.InvokeMethod(name, inParams, null);
        }

        #region PInvoke declarations to access recovery partition

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPTStr)] string filename, [MarshalAs(UnmanagedType.U4)] FileAccess access, [MarshalAs(UnmanagedType.U4)] FileShare share, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes, IntPtr templateFile);

        #endregion

        #region PInvoke declarations to mount/unmount ISO files

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 OpenVirtualDisk(ref VIRTUAL_STORAGE_TYPE VirtualStorageType, String Path, int VirtualDiskAccessMask, int Flags, IntPtr Parameters, out SafeFileHandle Handle);

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 AttachVirtualDisk(SafeFileHandle VirtualDiskHandle, IntPtr SecurityDescriptor, int Flags, Int32 ProviderSpecificFlags, IntPtr Parameters, IntPtr Overlapped);

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 DetachVirtualDisk(SafeFileHandle VirtualDiskHandle, int Flags, Int32 ProviderSpecificFlags);

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 GetVirtualDiskPhysicalPath(SafeFileHandle VirtualDiskHandle, ref Int32 DiskPathSizeInBytes, StringBuilder DiskPath);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        private const int OPEN_VIRTUAL_DISK_RW_DEPTH_DEFAULT = 1, VIRTUAL_STORAGE_TYPE_DEVICE_ISO = 1;
        private static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT = new Guid("EC984AEC-A0F9-47e9-901F-71415A66345B");
        private const int ATTACH_VIRTUAL_DISK_FLAG_READ_ONLY = 1, ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME = 4;
        private const int DETACH_VIRTUAL_DISK_FLAG_NONE = 0;
        private const int VIRTUAL_DISK_ACCESS_ATTACH_RO = 0x00010000, VIRTUAL_DISK_ACCESS_DETACH = 0x00040000, VIRTUAL_DISK_ACCESS_GET_INFO = 0x00080000;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct VIRTUAL_STORAGE_TYPE
        {
            public Int32 DeviceId;
            public Guid VendorId;
        }

        #endregion
    }
}
