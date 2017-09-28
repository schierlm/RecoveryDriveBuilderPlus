using System;
using System.Drawing;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace RecoveryDriveBuilderPlus
{
    public partial class MainWizard : Form
    {
        private BuildEngine engine = null;

        public MainWizard()
        {
            InitializeComponent();
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3");
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += InsertWatcher_EventArrived;
            insertWatcher.Start();
        }

        private void InsertWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                DriveItem selected = (DriveItem)driveSelect.SelectedItem;
                driveSelect.Items.Clear();
                wizardPageDriveSelect_Initialize(null, null);
                if (selected != null)
                {
                    foreach (DriveItem item in driveSelect.Items)
                    {
                        if (item.DriveInfo.Name == selected.DriveInfo.Name)
                        {
                            driveSelect.SelectedItem = item;
                        }
                    }
                }
            }));
        }

        private void wizardPageDriveSelect_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                if (di.DriveType == DriveType.Removable)
                {
                    driveSelect.Items.Add(new DriveItem(di));
                }
            }
            driveSelect_SelectedIndexChanged(null, null);
        }

        private void driveSelect_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index != -1)
            {
                DriveItem item = (DriveItem)driveSelect.Items[e.Index];
                e.Graphics.DrawIcon(item.Icon, e.Bounds.X, e.Bounds.Y);
                e.Graphics.DrawString(item.Label + " (" + item.DriveInfo.Name + ")", driveSelect.Font, Brushes.Black, new RectangleF(e.Bounds.X + driveSelect.ItemHeight, e.Bounds.Y, driveSelect.DropDownWidth, driveSelect.ItemHeight));
            }
            e.DrawFocusRectangle();
        }

        private void wizardPageBootStyle_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (e.PreviousPage == wizardPageDriveSelect || e.PreviousPage == wizardPageOptions)
            {
                try
                {
                    DriveItem drive = (DriveItem)driveSelect.SelectedItem;
                    if (drive == null)
                        throw new Exception("No drive selected");
                    if (engine != null)
                    {
                        engine.Dispose();
                        engine = null;
                    }
                    engine = new BuildEngine(drive.DriveInfo);
                    if (engine.BootloaderPresent)
                    {
                        bootStatusLabel.Text = "Existing bootloader found on " + drive.DriveInfo.Name;
                        actionNewMenu.Enabled = actionKeepAll.Enabled = true;
                    }
                    else
                    {
                        bootStatusLabel.Text = "No bootloader found on " + drive.DriveInfo.Name;
                        actionNewMenu.Enabled = actionKeepAll.Enabled = false;
                    }
                    bootFilesDeeperLocationCheckbox.Enabled = !engine.SkipLocationSelection;
                    bootFilesDeeperLocationCheckbox.Checked = engine.UseDeeperLocation;
                    if (!engine.BootloaderPresent && engine.SkipLocationSelection)
                    {
                        wizardControl.NextPage();
                    }
                }
#if !DEBUG
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    wizardControl.PreviousPage();
                }
#endif
                finally { }
            }
            else if (e.PreviousPage == wizardPageSelectSource && !bootFilesDeeperLocationCheckbox.Enabled && !actionKeepAll.Enabled)
            {
                wizardControl.PreviousPage();
            }
        }

        private void driveSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            wizardPageDriveSelect.AllowNext = driveSelect.SelectedItem != null;
        }

        private void MainWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (engine != null)
                engine.Dispose();
        }

        private void bootFilesDeeperLocationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            engine.UseDeeperLocation = bootFilesDeeperLocationCheckbox.Checked;
        }

        private void actionKeepAll_Click(object sender, EventArgs e)
        {
            engine.OverwriteBootloader = engine.OverwriteBCD = !engine.BootloaderPresent;
            wizardControl.NextPage();
        }

        private void actionNewMenu_Click(object sender, EventArgs e)
        {
            engine.OverwriteBootloader = !engine.BootloaderPresent;
            engine.OverwriteBCD = true;
            wizardControl.NextPage();
        }

        private void actionNewBootloader_Click(object sender, EventArgs e)
        {
            engine.OverwriteBootloader = engine.OverwriteBCD = true;
            wizardControl.NextPage();
        }

        private void sourceWindowsInstallation_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (!engine.OverwriteBCD && !engine.OverwriteBootloader && Control.ModifierKeys == (Keys.Control | Keys.Shift))
                {
                    // undocumented feature
                    engine.LoadFromExistingRecoveryWIM();
                }
                else
                {
                    engine.LoadFromWindowsInstallation();
                }
                engine.SelectEdition(engine.AvailableEditions[0]);
                wizardPageSelectEdition.Suppress = true;
                wizardControl.NextPage();
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
#endif
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void sourceInstallWimEsd_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectWIMDialog.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    engine.LoadFromInstallWIM(selectWIMDialog.FileName);
                    wizardPageSelectEdition.Suppress = false;
                    wizardControl.NextPage();
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
#endif
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void sourceISOImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectISODialog.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    engine.LoadFromISO(selectISODialog.FileName);
                    wizardPageSelectEdition.Suppress = false;
                    wizardControl.NextPage();
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
#endif
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        private void wizardPageSelectEdition_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            selectEditionList.Items.Clear();
            selectEditionList.Items.AddRange(engine.AvailableEditions);
            selectEditionList.SelectedIndex = -1;
        }

        private void wizardPageSelectEdition_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                engine.SelectEdition((string)selectEditionList.SelectedItem);
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                e.Cancel = true;
            }
#endif
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void wizardPageOptions_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            optionsBootMenuEntry.Text = engine.BootMenuEntryName;
            optionsFileName.Text = engine.DestinationFileName;
            optionsEFI.Enabled = engine.EfiSupported;
            optionsEFI.Checked = engine.EfiUsed;
            optionsSummary.Text = engine.Summary;
        }

        private void wizardPageOptions_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                engine.BootMenuEntryName = optionsBootMenuEntry.Text;
                engine.DestinationFileName = optionsFileName.Text;
                engine.EfiUsed = optionsEFI.Checked;
                engine.Build();
                engine.Dispose();
                engine = null;
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                e.Cancel = true;
                return;
            }
#endif
            finally
            {
                Cursor = Cursors.Default;
            }
            if (MessageBox.Show("Recovery drive created.\r\nDo you want to add another image to this recovery drive?", "Recovery Drive Builder Plus", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                e.Cancel = true;
                wizardControl.NextPage(wizardPageBootStyle, true);
            }
        }

        private void selectEditionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            wizardPageSelectEdition.AllowNext = selectEditionList.SelectedItem != null;
        }

        private void selectEditionList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            wizardControl.NextPage();
        }
    }

    class DriveItem
    {
        public DriveInfo DriveInfo { get; private set; }
        public Icon Icon { get; private set; }
        public string Label { get; private set; }

        public DriveItem(DriveInfo di)
        {
            DriveInfo = di;
            Icon = ExtractIcon.GetIcon(di.Name, true);
            Label = "";
            try
            {
                Label = di.VolumeLabel;
            }
            catch { }
        }
    }
}
