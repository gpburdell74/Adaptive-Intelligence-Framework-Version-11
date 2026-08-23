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
        FirstButton = new Adaptive.Intelligence.Win32.UI.Controls.AIButton();
        SecondButton = new Adaptive.Intelligence.Win32.UI.Controls.AIButton();
        ButtonLabel = new Adaptive.Intelligence.Win32.UI.Controls.AdvancedLabel();
        SuspendLayout();
        // 
        // FirstButton
        // 
        FirstButton.BorderWidth = 1;
        FirstButton.Checked = false;
        FirstButton.HoverBorderColor = Color.Gray;
        FirstButton.HoverDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        FirstButton.HoverEndColor = Color.FromArgb(224, 224, 224);
        FirstButton.HoverFont = new Font("Segoe UI", 9.75F);
        FirstButton.HoverForeColor = Color.Black;
        FirstButton.HoverStartColor = Color.FromArgb(218, 194, 204);
        FirstButton.Location = new Point(12, 50);
        FirstButton.Name = "FirstButton";
        FirstButton.NormalBorderColor = Color.Gray;
        FirstButton.NormalDirection = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
        FirstButton.NormalEndColor = Color.Silver;
        FirstButton.NormalFont = new Font("Segoe UI", 9.75F);
        FirstButton.NormalForeColor = Color.Black;
        FirstButton.NormalStartColor = Color.FromArgb(248, 248, 248);
        FirstButton.PressedBorderColor = Color.Gray;
        FirstButton.PressedDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        FirstButton.PressedEndColor = Color.FromArgb(174, 45, 61);
        FirstButton.PressedFont = new Font("Segoe UI", 9.75F);
        FirstButton.PressedForeColor = Color.White;
        FirstButton.PressedStartColor = Color.Gray;
        FirstButton.Size = new Size(100, 36);
        FirstButton.TabIndex = 0;
        FirstButton.Text = "Button &1";
        FirstButton.UseVisualStyleBackColor = true;
        // 
        // SecondButton
        // 
        SecondButton.BorderWidth = 1;
        SecondButton.Checked = false;
        SecondButton.HoverBorderColor = Color.Blue;
        SecondButton.HoverDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        SecondButton.HoverEndColor = Color.FromArgb(128, 128, 255);
        SecondButton.HoverFont = new Font("Segoe UI", 9.75F);
        SecondButton.HoverForeColor = Color.Black;
        SecondButton.HoverStartColor = Color.FromArgb(192, 255, 192);
        SecondButton.Location = new Point(118, 50);
        SecondButton.Name = "SecondButton";
        SecondButton.NormalBorderColor = Color.Navy;
        SecondButton.NormalDirection = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
        SecondButton.NormalEndColor = Color.Blue;
        SecondButton.NormalFont = new Font("Segoe UI", 9.75F);
        SecondButton.NormalForeColor = Color.White;
        SecondButton.NormalStartColor = Color.FromArgb(0, 192, 0);
        SecondButton.PressedBorderColor = Color.FromArgb(0, 64, 0);
        SecondButton.PressedDirection = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
        SecondButton.PressedEndColor = Color.Lime;
        SecondButton.PressedFont = new Font("Segoe UI", 9.75F);
        SecondButton.PressedForeColor = Color.White;
        SecondButton.PressedStartColor = Color.Blue;
        SecondButton.Size = new Size(100, 36);
        SecondButton.TabIndex = 1;
        SecondButton.Text = "Button &2";
        SecondButton.UseVisualStyleBackColor = true;
        // 
        // ButtonLabel
        // 
        ButtonLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        ButtonLabel.Location = new Point(12, 18);
        ButtonLabel.Name = "ButtonLabel";
        ButtonLabel.Shadow = true;
        ButtonLabel.Size = new Size(206, 26);
        ButtonLabel.TabIndex = 2;
        ButtonLabel.TabStop = false;
        ButtonLabel.Text = "Buttons:";
        ButtonLabel.TextAlign = ContentAlignment.TopLeft;
        // 
        // MainDialog
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1120, 577);
        Controls.Add(ButtonLabel);
        Controls.Add(SecondButton);
        Controls.Add(FirstButton);
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        Name = "MainDialog";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Adaptive Intelligence Win32 UI Demo Application";
        ResumeLayout(false);
    }

    #endregion

    private Win32.UI.Controls.AIButton FirstButton;
    private Win32.UI.Controls.AIButton SecondButton;
    private Win32.UI.Controls.AdvancedLabel ButtonLabel;
}