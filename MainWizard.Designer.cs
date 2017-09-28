namespace RecoveryDriveBuilderPlus
{
    partial class MainWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWizard));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            RecoveryDriveBuilderPlus.CommandLink actionNewBootloader;
            RecoveryDriveBuilderPlus.CommandLink sourceISOImage;
            RecoveryDriveBuilderPlus.CommandLink sourceInstallWimEsd;
            RecoveryDriveBuilderPlus.CommandLink sourceWindowsInstallation;
            this.wizardControl = new AeroWizard.WizardControl();
            this.wizardPageDriveSelect = new AeroWizard.WizardPage();
            this.driveSelect = new System.Windows.Forms.ComboBox();
            this.wizardPageBootStyle = new AeroWizard.WizardPage();
            this.actionNewMenu = new RecoveryDriveBuilderPlus.CommandLink();
            this.actionKeepAll = new RecoveryDriveBuilderPlus.CommandLink();
            this.bootFilesDeeperLocationCheckbox = new System.Windows.Forms.CheckBox();
            this.bootStatusLabel = new System.Windows.Forms.Label();
            this.wizardPageSelectSource = new AeroWizard.WizardPage();
            this.wizardPageSelectEdition = new AeroWizard.WizardPage();
            this.selectEditionList = new System.Windows.Forms.ListBox();
            this.wizardPageOptions = new AeroWizard.WizardPage();
            this.optionsEFI = new System.Windows.Forms.CheckBox();
            this.optionsSummary = new System.Windows.Forms.TextBox();
            this.optionsFileName = new System.Windows.Forms.TextBox();
            this.optionsBootMenuEntry = new System.Windows.Forms.TextBox();
            this.selectWIMDialog = new System.Windows.Forms.OpenFileDialog();
            this.selectISODialog = new System.Windows.Forms.OpenFileDialog();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            actionNewBootloader = new RecoveryDriveBuilderPlus.CommandLink();
            sourceISOImage = new RecoveryDriveBuilderPlus.CommandLink();
            sourceInstallWimEsd = new RecoveryDriveBuilderPlus.CommandLink();
            sourceWindowsInstallation = new RecoveryDriveBuilderPlus.CommandLink();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl)).BeginInit();
            this.wizardPageDriveSelect.SuspendLayout();
            this.wizardPageBootStyle.SuspendLayout();
            this.wizardPageSelectSource.SuspendLayout();
            this.wizardPageSelectEdition.SuspendLayout();
            this.wizardPageOptions.SuspendLayout();
            this.SuspendLayout();
            //
            // label1
            //
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            label1.Location = new System.Drawing.Point(3, 10);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(590, 215);
            label1.TabIndex = 0;
            label1.Text = resources.GetString("label1.Text");
            //
            // label2
            //
            label2.Location = new System.Drawing.Point(3, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(590, 56);
            label2.TabIndex = 0;
            label2.Text = resources.GetString("label2.Text");
            //
            // label3
            //
            label3.Location = new System.Drawing.Point(3, 6);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(73, 20);
            label3.TabIndex = 0;
            label3.Text = "&Menu Entry:";
            //
            // label4
            //
            label4.Location = new System.Drawing.Point(3, 35);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(73, 20);
            label4.TabIndex = 2;
            label4.Text = "&File Name:";
            //
            // label5
            //
            label5.Location = new System.Drawing.Point(3, 64);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(73, 20);
            label5.TabIndex = 4;
            label5.Text = "&Summary:";
            //
            // label6
            //
            label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            label6.Location = new System.Drawing.Point(3, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(590, 37);
            label6.TabIndex = 1;
            label6.Text = "Some source media may contain multiple Windows editions, each with their own Reco" +
    "very Drive. Select the edition below:";
            //
            // label7
            //
            label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
            label7.Location = new System.Drawing.Point(6, 218);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(590, 37);
            label7.TabIndex = 2;
            label7.Text = "After this step, the recovery image will be analyzed. This process may take sever" +
    "al seconds and cannot be cancelled.";
            //
            // actionNewBootloader
            //
            actionNewBootloader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            actionNewBootloader.FlatStyle = System.Windows.Forms.FlatStyle.System;
            actionNewBootloader.Location = new System.Drawing.Point(3, 130);
            actionNewBootloader.Name = "actionNewBootloader";
            actionNewBootloader.NoteText = "The bootloader is freshly installed on the drive; note that this does not make th" +
    "e drive bootable if it has not already been before.";
            actionNewBootloader.Size = new System.Drawing.Size(590, 73);
            actionNewBootloader.TabIndex = 4;
            actionNewBootloader.Text = "Install new boot loader files and create new boot menu";
            actionNewBootloader.UseVisualStyleBackColor = true;
            actionNewBootloader.Click += new System.EventHandler(this.actionNewBootloader_Click);
            //
            // sourceISOImage
            //
            sourceISOImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sourceISOImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            sourceISOImage.Location = new System.Drawing.Point(0, 189);
            sourceISOImage.Name = "sourceISOImage";
            sourceISOImage.NoteText = "Use this if you have an ISO file of the Windows installation media.";
            sourceISOImage.Size = new System.Drawing.Size(590, 59);
            sourceISOImage.TabIndex = 6;
            sourceISOImage.Text = "Extract recovery image from installer ISO image";
            sourceISOImage.UseVisualStyleBackColor = true;
            sourceISOImage.Click += new System.EventHandler(this.sourceISOImage_Click);
            //
            // sourceInstallWimEsd
            //
            sourceInstallWimEsd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sourceInstallWimEsd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            sourceInstallWimEsd.Location = new System.Drawing.Point(0, 124);
            sourceInstallWimEsd.Name = "sourceInstallWimEsd";
            sourceInstallWimEsd.NoteText = "You will in general find these in the Sources directory on installation media.";
            sourceInstallWimEsd.Size = new System.Drawing.Size(590, 59);
            sourceInstallWimEsd.TabIndex = 5;
            sourceInstallWimEsd.Text = "Extract recovery image from Install.Wim or Install.Esd file";
            sourceInstallWimEsd.UseVisualStyleBackColor = true;
            sourceInstallWimEsd.Click += new System.EventHandler(this.sourceInstallWimEsd_Click);
            //
            // sourceWindowsInstallation
            //
            sourceWindowsInstallation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sourceWindowsInstallation.FlatStyle = System.Windows.Forms.FlatStyle.System;
            sourceWindowsInstallation.Location = new System.Drawing.Point(0, 59);
            sourceWindowsInstallation.Name = "sourceWindowsInstallation";
            sourceWindowsInstallation.NoteText = "The recovery drive behaves as if it was created with RecoveryDrive.exe";
            sourceWindowsInstallation.Size = new System.Drawing.Size(590, 59);
            sourceWindowsInstallation.TabIndex = 4;
            sourceWindowsInstallation.Text = "Take recovery image from current Windows installation";
            sourceWindowsInstallation.UseVisualStyleBackColor = true;
            sourceWindowsInstallation.Click += new System.EventHandler(this.sourceWindowsInstallation_Click);
            //
            // wizardControl
            //
            this.wizardControl.BackColor = System.Drawing.Color.White;
            this.wizardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardControl.Location = new System.Drawing.Point(0, 0);
            this.wizardControl.Name = "wizardControl";
            this.wizardControl.Pages.Add(this.wizardPageDriveSelect);
            this.wizardControl.Pages.Add(this.wizardPageBootStyle);
            this.wizardControl.Pages.Add(this.wizardPageSelectSource);
            this.wizardControl.Pages.Add(this.wizardPageSelectEdition);
            this.wizardControl.Pages.Add(this.wizardPageOptions);
            this.wizardControl.Size = new System.Drawing.Size(643, 409);
            this.wizardControl.TabIndex = 0;
            this.wizardControl.Title = "Recovery Drive Builder Plus";
            //
            // wizardPageDriveSelect
            //
            this.wizardPageDriveSelect.AllowBack = false;
            this.wizardPageDriveSelect.AllowNext = false;
            this.wizardPageDriveSelect.Controls.Add(this.driveSelect);
            this.wizardPageDriveSelect.Controls.Add(label1);
            this.wizardPageDriveSelect.Name = "wizardPageDriveSelect";
            this.wizardPageDriveSelect.Size = new System.Drawing.Size(596, 255);
            this.wizardPageDriveSelect.TabIndex = 0;
            this.wizardPageDriveSelect.Text = "Build Recovery Drive";
            this.wizardPageDriveSelect.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageDriveSelect_Initialize);
            //
            // driveSelect
            //
            this.driveSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driveSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.driveSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driveSelect.Location = new System.Drawing.Point(3, 228);
            this.driveSelect.Name = "driveSelect";
            this.driveSelect.Size = new System.Drawing.Size(590, 24);
            this.driveSelect.TabIndex = 1;
            this.driveSelect.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.driveSelect_DrawItem);
            this.driveSelect.SelectedIndexChanged += new System.EventHandler(this.driveSelect_SelectedIndexChanged);
            //
            // wizardPageBootStyle
            //
            this.wizardPageBootStyle.AllowNext = false;
            this.wizardPageBootStyle.Controls.Add(actionNewBootloader);
            this.wizardPageBootStyle.Controls.Add(this.actionNewMenu);
            this.wizardPageBootStyle.Controls.Add(this.actionKeepAll);
            this.wizardPageBootStyle.Controls.Add(this.bootFilesDeeperLocationCheckbox);
            this.wizardPageBootStyle.Controls.Add(this.bootStatusLabel);
            this.wizardPageBootStyle.Name = "wizardPageBootStyle";
            this.wizardPageBootStyle.ShowNext = false;
            this.wizardPageBootStyle.Size = new System.Drawing.Size(596, 255);
            this.wizardPageBootStyle.TabIndex = 1;
            this.wizardPageBootStyle.Text = "Choose Action";
            this.wizardPageBootStyle.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageBootStyle_Initialize);
            //
            // actionNewMenu
            //
            this.actionNewMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionNewMenu.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.actionNewMenu.Location = new System.Drawing.Point(3, 80);
            this.actionNewMenu.Name = "actionNewMenu";
            this.actionNewMenu.NoteText = "The recovery drive becomes the only entry of the boot menu.";
            this.actionNewMenu.Size = new System.Drawing.Size(590, 59);
            this.actionNewMenu.TabIndex = 3;
            this.actionNewMenu.Text = "Keep bootloader, but create a new boot menu";
            this.actionNewMenu.UseVisualStyleBackColor = true;
            this.actionNewMenu.Click += new System.EventHandler(this.actionNewMenu_Click);
            //
            // actionKeepAll
            //
            this.actionKeepAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionKeepAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.actionKeepAll.Location = new System.Drawing.Point(3, 30);
            this.actionKeepAll.Name = "actionKeepAll";
            this.actionKeepAll.NoteText = "The recovery drive is added as a new entry to the boot loader\'s boot menu.";
            this.actionKeepAll.Size = new System.Drawing.Size(590, 59);
            this.actionKeepAll.TabIndex = 2;
            this.actionKeepAll.Text = "Keep Bootloader, add to existing boot menu";
            this.actionKeepAll.UseVisualStyleBackColor = true;
            this.actionKeepAll.Click += new System.EventHandler(this.actionKeepAll_Click);
            //
            // bootFilesDeeperLocationCheckbox
            //
            this.bootFilesDeeperLocationCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bootFilesDeeperLocationCheckbox.Location = new System.Drawing.Point(6, 213);
            this.bootFilesDeeperLocationCheckbox.Name = "bootFilesDeeperLocationCheckbox";
            this.bootFilesDeeperLocationCheckbox.Size = new System.Drawing.Size(587, 39);
            this.bootFilesDeeperLocationCheckbox.TabIndex = 1;
            this.bootFilesDeeperLocationCheckbox.Text = "Place primary boot files at \\Boot\\Bootmgr and \\Efi\\Microsoft\\bootx64.efi (to be c" +
    "hainloaded from another bootloader)";
            this.bootFilesDeeperLocationCheckbox.UseVisualStyleBackColor = true;
            this.bootFilesDeeperLocationCheckbox.CheckedChanged += new System.EventHandler(this.bootFilesDeeperLocationCheckbox_CheckedChanged);
            //
            // bootStatusLabel
            //
            this.bootStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bootStatusLabel.Location = new System.Drawing.Point(3, 0);
            this.bootStatusLabel.Name = "bootStatusLabel";
            this.bootStatusLabel.Size = new System.Drawing.Size(590, 26);
            this.bootStatusLabel.TabIndex = 0;
            this.bootStatusLabel.Text = "No boot loader found.";
            //
            // wizardPageSelectSource
            //
            this.wizardPageSelectSource.AllowNext = false;
            this.wizardPageSelectSource.Controls.Add(sourceISOImage);
            this.wizardPageSelectSource.Controls.Add(sourceInstallWimEsd);
            this.wizardPageSelectSource.Controls.Add(sourceWindowsInstallation);
            this.wizardPageSelectSource.Controls.Add(label2);
            this.wizardPageSelectSource.Name = "wizardPageSelectSource";
            this.wizardPageSelectSource.ShowNext = false;
            this.wizardPageSelectSource.Size = new System.Drawing.Size(596, 255);
            this.wizardPageSelectSource.TabIndex = 2;
            this.wizardPageSelectSource.Text = "Select Source";
            //
            // wizardPageSelectEdition
            //
            this.wizardPageSelectEdition.AllowNext = false;
            this.wizardPageSelectEdition.Controls.Add(this.selectEditionList);
            this.wizardPageSelectEdition.Controls.Add(label7);
            this.wizardPageSelectEdition.Controls.Add(label6);
            this.wizardPageSelectEdition.Name = "wizardPageSelectEdition";
            this.wizardPageSelectEdition.Size = new System.Drawing.Size(596, 255);
            this.wizardPageSelectEdition.TabIndex = 4;
            this.wizardPageSelectEdition.Text = "Select Windows Edition";
            this.wizardPageSelectEdition.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPageSelectEdition_Commit);
            this.wizardPageSelectEdition.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageSelectEdition_Initialize);
            //
            // selectEditionList
            //
            this.selectEditionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectEditionList.IntegralHeight = false;
            this.selectEditionList.ItemHeight = 15;
            this.selectEditionList.Items.AddRange(new object[] {
            "Home",
            "Pro",
            "Enterprise",
            "Education"});
            this.selectEditionList.Location = new System.Drawing.Point(6, 40);
            this.selectEditionList.Name = "selectEditionList";
            this.selectEditionList.ScrollAlwaysVisible = true;
            this.selectEditionList.Size = new System.Drawing.Size(587, 175);
            this.selectEditionList.TabIndex = 3;
            this.selectEditionList.SelectedIndexChanged += new System.EventHandler(this.selectEditionList_SelectedIndexChanged);
            this.selectEditionList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.selectEditionList_MouseDoubleClick);
            //
            // wizardPageOptions
            //
            this.wizardPageOptions.Controls.Add(this.optionsEFI);
            this.wizardPageOptions.Controls.Add(label5);
            this.wizardPageOptions.Controls.Add(label4);
            this.wizardPageOptions.Controls.Add(this.optionsSummary);
            this.wizardPageOptions.Controls.Add(this.optionsFileName);
            this.wizardPageOptions.Controls.Add(this.optionsBootMenuEntry);
            this.wizardPageOptions.Controls.Add(label3);
            this.wizardPageOptions.IsFinishPage = true;
            this.wizardPageOptions.Name = "wizardPageOptions";
            this.wizardPageOptions.Size = new System.Drawing.Size(596, 255);
            this.wizardPageOptions.TabIndex = 3;
            this.wizardPageOptions.Text = "Boot loader options";
            this.wizardPageOptions.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPageOptions_Commit);
            this.wizardPageOptions.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageOptions_Initialize);
            //
            // optionsEFI
            //
            this.optionsEFI.AutoSize = true;
            this.optionsEFI.Location = new System.Drawing.Point(82, 233);
            this.optionsEFI.Name = "optionsEFI";
            this.optionsEFI.Size = new System.Drawing.Size(180, 19);
            this.optionsEFI.TabIndex = 6;
            this.optionsEFI.Text = "Add to EFI boot menu as well";
            this.optionsEFI.UseVisualStyleBackColor = true;
            //
            // optionsSummary
            //
            this.optionsSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsSummary.Location = new System.Drawing.Point(82, 61);
            this.optionsSummary.Multiline = true;
            this.optionsSummary.Name = "optionsSummary";
            this.optionsSummary.ReadOnly = true;
            this.optionsSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.optionsSummary.Size = new System.Drawing.Size(511, 166);
            this.optionsSummary.TabIndex = 5;
            //
            // optionsFileName
            //
            this.optionsFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsFileName.Location = new System.Drawing.Point(82, 32);
            this.optionsFileName.Name = "optionsFileName";
            this.optionsFileName.Size = new System.Drawing.Size(511, 23);
            this.optionsFileName.TabIndex = 3;
            //
            // optionsBootMenuEntry
            //
            this.optionsBootMenuEntry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsBootMenuEntry.Location = new System.Drawing.Point(82, 3);
            this.optionsBootMenuEntry.Name = "optionsBootMenuEntry";
            this.optionsBootMenuEntry.Size = new System.Drawing.Size(511, 23);
            this.optionsBootMenuEntry.TabIndex = 1;
            //
            // selectWIMDialog
            //
            this.selectWIMDialog.DefaultExt = "wim";
            this.selectWIMDialog.Filter = "Install Images (install.wim; install.esd)|install.wim;install.esd|All files (*.*)" +
    "|*.*";
            this.selectWIMDialog.RestoreDirectory = true;
            //
            // selectISODialog
            //
            this.selectISODialog.DefaultExt = "iso";
            this.selectISODialog.Filter = "ISO Images (*.iso)|*.iso|All files (*.*)|*.*";
            this.selectISODialog.RestoreDirectory = true;
            //
            // MainWizard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 409);
            this.Controls.Add(this.wizardControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWizard_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl)).EndInit();
            this.wizardPageDriveSelect.ResumeLayout(false);
            this.wizardPageBootStyle.ResumeLayout(false);
            this.wizardPageSelectSource.ResumeLayout(false);
            this.wizardPageSelectEdition.ResumeLayout(false);
            this.wizardPageOptions.ResumeLayout(false);
            this.wizardPageOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl wizardControl;
        private AeroWizard.WizardPage wizardPageDriveSelect;
        private System.Windows.Forms.ComboBox driveSelect;
        private AeroWizard.WizardPage wizardPageBootStyle;
        private System.Windows.Forms.CheckBox bootFilesDeeperLocationCheckbox;
        private System.Windows.Forms.Label bootStatusLabel;
        private AeroWizard.WizardPage wizardPageSelectSource;
        private CommandLink actionKeepAll;
        private CommandLink actionNewMenu;
        private System.Windows.Forms.OpenFileDialog selectWIMDialog;
        private System.Windows.Forms.OpenFileDialog selectISODialog;
        private AeroWizard.WizardPage wizardPageOptions;
        private System.Windows.Forms.TextBox optionsSummary;
        private System.Windows.Forms.TextBox optionsFileName;
        private System.Windows.Forms.TextBox optionsBootMenuEntry;
        private System.Windows.Forms.CheckBox optionsEFI;
        private AeroWizard.WizardPage wizardPageSelectEdition;
        private System.Windows.Forms.ListBox selectEditionList;
    }
}