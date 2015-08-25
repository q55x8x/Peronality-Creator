<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutScreen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutScreen))
        Me.btnClose = New System.Windows.Forms.Button
        Me.bpLogo = New i00BindingList.BufferedPanel
        Me.pnlTabHolders = New System.Windows.Forms.Panel
        Me.pnlAbout = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.SiteLinkProduct = New System.Windows.Forms.LinkLabel
        Me.Label3 = New System.Windows.Forms.Label
        Me.CtlLogo1 = New i00BindingList.ctlLogo
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblEvaluation = New System.Windows.Forms.Label
        Me.pnlLegal = New System.Windows.Forms.Panel
        Me.bpLicence = New i00BindingList.BufferedPanel
        Me.lblLicence = New i00BindingList.AutoGrowLabel
        Me.lblReferenced = New System.Windows.Forms.Label
        Me.pnlReferences = New i00BindingList.BufferedPanel
        Me.lblReferences = New i00BindingList.AutoGrowLabel
        Me.bpTabs = New i00BindingList.BufferedPanel
        Me.SiteLink = New System.Windows.Forms.LinkLabel
        Me.Label2 = New System.Windows.Forms.Label
        Me.bpLogo.SuspendLayout()
        Me.pnlTabHolders.SuspendLayout()
        Me.pnlAbout.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.CtlLogo1.SuspendLayout()
        Me.pnlLegal.SuspendLayout()
        Me.bpLicence.SuspendLayout()
        Me.pnlReferences.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(697, 531)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'bpLogo
        '
        Me.bpLogo.Controls.Add(Me.pnlTabHolders)
        Me.bpLogo.Controls.Add(Me.bpTabs)
        Me.bpLogo.Controls.Add(Me.SiteLink)
        Me.bpLogo.Controls.Add(Me.Label2)
        Me.bpLogo.Controls.Add(Me.btnClose)
        Me.bpLogo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpLogo.Location = New System.Drawing.Point(0, 0)
        Me.bpLogo.Name = "bpLogo"
        Me.bpLogo.Size = New System.Drawing.Size(784, 566)
        Me.bpLogo.TabIndex = 0
        '
        'pnlTabHolders
        '
        Me.pnlTabHolders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTabHolders.BackColor = System.Drawing.Color.Transparent
        Me.pnlTabHolders.Controls.Add(Me.pnlAbout)
        Me.pnlTabHolders.Controls.Add(Me.pnlLegal)
        Me.pnlTabHolders.Location = New System.Drawing.Point(12, 28)
        Me.pnlTabHolders.Name = "pnlTabHolders"
        Me.pnlTabHolders.Size = New System.Drawing.Size(760, 484)
        Me.pnlTabHolders.TabIndex = 12
        '
        'pnlAbout
        '
        Me.pnlAbout.BackColor = System.Drawing.Color.Transparent
        Me.pnlAbout.Controls.Add(Me.Panel1)
        Me.pnlAbout.Controls.Add(Me.CtlLogo1)
        Me.pnlAbout.Location = New System.Drawing.Point(339, 273)
        Me.pnlAbout.Name = "pnlAbout"
        Me.pnlAbout.Size = New System.Drawing.Size(288, 150)
        Me.pnlAbout.TabIndex = 12
        Me.pnlAbout.Tag = "About"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.SiteLinkProduct)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(3, 121)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(282, 26)
        Me.Panel1.TabIndex = 10
        '
        'SiteLinkProduct
        '
        Me.SiteLinkProduct.AutoSize = True
        Me.SiteLinkProduct.BackColor = System.Drawing.Color.Transparent
        Me.SiteLinkProduct.LinkColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SiteLinkProduct.Location = New System.Drawing.Point(3, 13)
        Me.SiteLinkProduct.Name = "SiteLinkProduct"
        Me.SiteLinkProduct.Size = New System.Drawing.Size(19, 13)
        Me.SiteLinkProduct.TabIndex = 9
        Me.SiteLinkProduct.TabStop = True
        Me.SiteLinkProduct.Tag = "false"
        Me.SiteLinkProduct.Text = "...."
        Me.SiteLinkProduct.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(232, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Tag = "false"
        Me.Label3.Text = "For product updates and examples on use goto:"
        '
        'CtlLogo1
        '
        Me.CtlLogo1.Controls.Add(Me.Label1)
        Me.CtlLogo1.Controls.Add(Me.lblVersion)
        Me.CtlLogo1.Controls.Add(Me.lblEvaluation)
        Me.CtlLogo1.Dock = System.Windows.Forms.DockStyle.Top
        Me.CtlLogo1.DrawBackground = False
        Me.CtlLogo1.Location = New System.Drawing.Point(0, 0)
        Me.CtlLogo1.Name = "CtlLogo1"
        Me.CtlLogo1.Size = New System.Drawing.Size(288, 115)
        Me.CtlLogo1.TabIndex = 0
        Me.CtlLogo1.Tag = "false"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(82, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(134, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Tag = ""
        Me.Label1.Text = "Created by i00 Productions"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.Location = New System.Drawing.Point(79, 70)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblVersion.TabIndex = 6
        Me.lblVersion.Tag = ""
        Me.lblVersion.Text = "Version:"
        '
        'lblEvaluation
        '
        Me.lblEvaluation.AutoSize = True
        Me.lblEvaluation.BackColor = System.Drawing.Color.Transparent
        Me.lblEvaluation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEvaluation.Location = New System.Drawing.Point(79, 57)
        Me.lblEvaluation.Name = "lblEvaluation"
        Me.lblEvaluation.Size = New System.Drawing.Size(215, 13)
        Me.lblEvaluation.TabIndex = 5
        Me.lblEvaluation.Tag = ""
        Me.lblEvaluation.Text = "Above and beyond standard binding!"
        '
        'pnlLegal
        '
        Me.pnlLegal.BackColor = System.Drawing.Color.Transparent
        Me.pnlLegal.Controls.Add(Me.bpLicence)
        Me.pnlLegal.Controls.Add(Me.lblReferenced)
        Me.pnlLegal.Controls.Add(Me.pnlReferences)
        Me.pnlLegal.Location = New System.Drawing.Point(20, 11)
        Me.pnlLegal.Name = "pnlLegal"
        Me.pnlLegal.Size = New System.Drawing.Size(288, 215)
        Me.pnlLegal.TabIndex = 11
        Me.pnlLegal.Tag = "Legal"
        '
        'bpLicence
        '
        Me.bpLicence.AutoScroll = True
        Me.bpLicence.Controls.Add(Me.lblLicence)
        Me.bpLicence.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpLicence.Location = New System.Drawing.Point(0, 0)
        Me.bpLicence.Name = "bpLicence"
        Me.bpLicence.Size = New System.Drawing.Size(288, 115)
        Me.bpLicence.TabIndex = 16
        '
        'lblLicence
        '
        Me.lblLicence.BackColor = System.Drawing.Color.Transparent
        Me.lblLicence.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblLicence.Location = New System.Drawing.Point(0, 0)
        Me.lblLicence.Name = "lblLicence"
        Me.lblLicence.Size = New System.Drawing.Size(271, 182)
        Me.lblLicence.TabIndex = 10
        Me.lblLicence.Tag = ""
        Me.lblLicence.Text = resources.GetString("lblLicence.Text")
        '
        'lblReferenced
        '
        Me.lblReferenced.AutoSize = True
        Me.lblReferenced.BackColor = System.Drawing.Color.Transparent
        Me.lblReferenced.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblReferenced.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReferenced.Location = New System.Drawing.Point(0, 115)
        Me.lblReferenced.Name = "lblReferenced"
        Me.lblReferenced.Size = New System.Drawing.Size(143, 13)
        Me.lblReferenced.TabIndex = 2
        Me.lblReferenced.Tag = "false"
        Me.lblReferenced.Text = "Referenced Assemblies:"
        '
        'pnlReferences
        '
        Me.pnlReferences.AutoScroll = True
        Me.pnlReferences.Controls.Add(Me.lblReferences)
        Me.pnlReferences.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlReferences.Location = New System.Drawing.Point(0, 128)
        Me.pnlReferences.Name = "pnlReferences"
        Me.pnlReferences.Size = New System.Drawing.Size(288, 87)
        Me.pnlReferences.TabIndex = 1
        '
        'lblReferences
        '
        Me.lblReferences.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblReferences.Location = New System.Drawing.Point(0, 0)
        Me.lblReferences.Name = "lblReferences"
        Me.lblReferences.Size = New System.Drawing.Size(288, 13)
        Me.lblReferences.TabIndex = 0
        Me.lblReferences.Tag = ""
        Me.lblReferences.Text = "...."
        '
        'bpTabs
        '
        Me.bpTabs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bpTabs.BackColor = System.Drawing.Color.Transparent
        Me.bpTabs.Location = New System.Drawing.Point(12, 12)
        Me.bpTabs.Name = "bpTabs"
        Me.bpTabs.Size = New System.Drawing.Size(760, 16)
        Me.bpTabs.TabIndex = 10
        '
        'SiteLink
        '
        Me.SiteLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SiteLink.AutoSize = True
        Me.SiteLink.BackColor = System.Drawing.Color.Transparent
        Me.SiteLink.LinkColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SiteLink.Location = New System.Drawing.Point(12, 536)
        Me.SiteLink.Name = "SiteLink"
        Me.SiteLink.Size = New System.Drawing.Size(19, 13)
        Me.SiteLink.TabIndex = 4
        Me.SiteLink.TabStop = True
        Me.SiteLink.Text = "...."
        Me.SiteLink.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(12, 515)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(211, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "©2010 i00 Productions.  All rights reserved."
        '
        'AboutScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(784, 566)
        Me.Controls.Add(Me.bpLogo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(364, 305)
        Me.Name = "AboutScreen"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About i00 Binding List"
        Me.bpLogo.ResumeLayout(False)
        Me.bpLogo.PerformLayout()
        Me.pnlTabHolders.ResumeLayout(False)
        Me.pnlAbout.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.CtlLogo1.ResumeLayout(False)
        Me.CtlLogo1.PerformLayout()
        Me.pnlLegal.ResumeLayout(False)
        Me.pnlLegal.PerformLayout()
        Me.bpLicence.ResumeLayout(False)
        Me.pnlReferences.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bpLogo As BufferedPanel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SiteLink As System.Windows.Forms.LinkLabel
    Friend WithEvents bpTabs As BufferedPanel
    Friend WithEvents pnlTabHolders As System.Windows.Forms.Panel
    Friend WithEvents pnlAbout As System.Windows.Forms.Panel
    Friend WithEvents CtlLogo1 As ctlLogo
    Friend WithEvents lblEvaluation As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SiteLinkProduct As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlLegal As System.Windows.Forms.Panel
    Friend WithEvents bpLicence As BufferedPanel
    Friend WithEvents lblLicence As AutoGrowLabel
    Friend WithEvents lblReferenced As System.Windows.Forms.Label
    Friend WithEvents pnlReferences As BufferedPanel
    Friend WithEvents lblReferences As AutoGrowLabel
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
