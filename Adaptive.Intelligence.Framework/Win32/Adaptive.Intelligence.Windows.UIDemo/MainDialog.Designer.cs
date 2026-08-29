namespace Adaptive.Intelligence.Windows.UIDemo;

partial class MainDialog
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components;

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDialog));
        aiButton1 = new Adaptive.Intelligence.Win32.UI.Controls.AIButton();
        aiButton2 = new Adaptive.Intelligence.Win32.UI.Controls.AIButton();
        advancedLabel1 = new Adaptive.Intelligence.Win32.UI.Controls.AdvancedLabel();
        SuspendLayout();
        // 
        // aiButton1
        // 
        aiButton1.BorderWidth = 1;
        aiButton1.Checked = false;
        aiButton1.HoverBorderColor = Color.Gray;
        aiButton1.HoverDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        aiButton1.HoverEndColor = Color.FromArgb(224, 224, 224);
        aiButton1.HoverFont = new Font("Segoe UI", 9.75F);
        aiButton1.HoverForeColor = Color.Black;
        aiButton1.HoverStartColor = Color.FromArgb(218, 194, 204);
        aiButton1.Location = new Point(247, 149);
        aiButton1.Name = "aiButton1";
        aiButton1.NormalBorderColor = Color.Gray;
        aiButton1.NormalDirection = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
        aiButton1.NormalEndColor = Color.Silver;
        aiButton1.NormalFont = new Font("Segoe UI", 9.75F);
        aiButton1.NormalForeColor = Color.Black;
        aiButton1.NormalStartColor = Color.FromArgb(248, 248, 248);
        aiButton1.PressedBorderColor = Color.Gray;
        aiButton1.PressedDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        aiButton1.PressedEndColor = Color.FromArgb(174, 45, 61);
        aiButton1.PressedFont = new Font("Segoe UI", 9.75F);
        aiButton1.PressedForeColor = Color.White;
        aiButton1.PressedStartColor = Color.Gray;
        aiButton1.Size = new Size(75, 23);
        aiButton1.TabIndex = 0;
        aiButton1.Text = "aiButton1";
        aiButton1.UseVisualStyleBackColor = true;
        // 
        // aiButton2
        // 
        aiButton2.BorderWidth = 1;
        aiButton2.Checked = false;
        aiButton2.HoverBorderColor = Color.Gray;
        aiButton2.HoverDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        aiButton2.HoverEndColor = Color.FromArgb(224, 224, 224);
        aiButton2.HoverFont = new Font("Segoe UI", 9.75F);
        aiButton2.HoverForeColor = Color.Black;
        aiButton2.HoverStartColor = Color.FromArgb(218, 194, 204);
        aiButton2.Location = new Point(543, 130);
        aiButton2.Name = "aiButton2";
        aiButton2.NormalBorderColor = Color.Gray;
        aiButton2.NormalDirection = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
        aiButton2.NormalEndColor = Color.Silver;
        aiButton2.NormalFont = new Font("Segoe UI", 9.75F);
        aiButton2.NormalForeColor = Color.Black;
        aiButton2.NormalStartColor = Color.FromArgb(248, 248, 248);
        aiButton2.PressedBorderColor = Color.Gray;
        aiButton2.PressedDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        aiButton2.PressedEndColor = Color.FromArgb(174, 45, 61);
        aiButton2.PressedFont = new Font("Segoe UI", 9.75F);
        aiButton2.PressedForeColor = Color.White;
        aiButton2.PressedStartColor = Color.Gray;
        aiButton2.Size = new Size(75, 23);
        aiButton2.TabIndex = 1;
        aiButton2.Text = "aiButton2";
        aiButton2.UseVisualStyleBackColor = true;
        // 
        // advancedLabel1
        // 
        advancedLabel1.AutoSize = true;
        advancedLabel1.Font = new Font("Segoe UI", 9.75F);
        advancedLabel1.Location = new Point(576, 44);
        advancedLabel1.Name = "advancedLabel1";
        advancedLabel1.Size = new Size(102, 17);
        advancedLabel1.TabIndex = 2;
        advancedLabel1.Text = "advancedLabel1";
        // 
        // MainDialog
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1110, 631);
        Controls.Add(advancedLabel1);
        Controls.Add(aiButton2);
        Controls.Add(aiButton1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        Name = "MainDialog";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "MainDialog";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Win32.UI.Controls.AIButton aiButton1;
    private Win32.UI.Controls.AIButton aiButton2;
    private Win32.UI.Controls.AdvancedLabel advancedLabel1;
}