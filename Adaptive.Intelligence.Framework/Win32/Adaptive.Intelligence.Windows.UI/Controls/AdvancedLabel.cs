using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Adaptive.Intelligence.Win32.UI.Controls;

/// <summary>
/// Provides an advanced drawing label control to provide advanced display options.
/// </summary>
/// <seealso cref="UserControl" />
[DefaultBindingProperty("Text"),
 DefaultProperty("Text"),
 Description("Provides an advanced label class for Desktop applications.")]
public sealed class AdvancedLabel : Label
{
    #region Private Member Declarations
    /// <summary>
    /// The alignment of the text.
    /// </summary>
    private ContentAlignment _alignment = ContentAlignment.TopCenter;
    /// <summary>
    /// Flag to use shadowing.
    /// </summary>
    private bool _shadow;
    /// <summary>
    /// The maximum width of the label when auto-sizing.
    /// </summary>
    private int _maxAutoSizeWidth = 800;
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="AdvancedLabel"/> class.
    /// </summary>
    public AdvancedLabel()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.FixedHeight, false);
        SetStyle(ControlStyles.FixedWidth, false);
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.CacheText, true);
        SetStyle(ControlStyles.ContainerControl, false);
        SetStyle(ControlStyles.DoubleBuffer, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.Selectable, false);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);

        AutoSize = false;
        TabStop = false;
        Font = UiConstants.CreateStandardFont();
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets a value indicating whether the control auto-sizes itself.
    /// </summary>
    /// <value>
    /// <b>true</b> if the control is auto-sized; otherwise, <b>false</b>.
    /// </value>
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override bool AutoSize
    {
        get => base.AutoSize;
        set
        {
            base.AutoSize = value;
            PerformAutoSize();
            Invalidate();
        }
    }
    /// <summary>
    /// Gets or sets the maximum width of the label when being automatically sized.
    /// </summary>
    /// <value>
    /// An integer specifying the maximum width of the label when being auto-sized.
    /// </value>
    [SettingsBindable(true),
     Bindable(false),
     Localizable(false),
     Category("Behavior"),
     DefaultValue(800),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("An integer specifying the maximum width of the label when being auto-sized."),
     Browsable(true)]
    public int AutoSizeMaxWidth
    {
        get => _maxAutoSizeWidth;
        set
        {
            if (value >= 1)
            {
                _maxAutoSizeWidth = value;
                PerformAutoSize();
                Invalidate();
            }
        }
    }
    /// <summary>
    /// Gets or sets a value indicating whether to use anti-aliasing when drawing the text.
    /// </summary>
    /// <value>
    ///   <b>true</b> to use anti-aliasing; otherwise, <b>false</b>.
    /// </value>
    [Category("Appearance"),
     DefaultValue(false),
     Description("Indicates whether to use anti-aliasing when drawing the text."),
     Browsable(true)]
    public bool Shadow
    {
        get => _shadow;
        set
        {
            _shadow = value;
            PerformAutoSize();
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the text displayed in the label.
    /// </summary>
    /// <value>
    /// A string containing the text to be displayed in the label.
    /// </value>
    [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), SettingsBindable(true),
     Bindable(true),
     Localizable(true),
     Category("Appearance"),
     DefaultValue(""),
     AllowNull(),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("The text to be displayed in the label."),
     Browsable(true)]
    public override string Text
    {
        get => base.Text;
        set
        {
            if (base.Text == value)
            {
                return;
            }
            base.Text = value;
            PerformAutoSize();
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the alignment of text in the label.
    /// </summary>
    /// <value>
    /// One of the <see cref="ContentAlignment" /> values. The default is
    /// <see cref="ContentAlignment.TopLeft" />.
    /// </value>
    [DefaultValue(ContentAlignment.TopCenter),
     Localizable(true),
     Category("Appearance"),
     Bindable(true),
     Browsable(true),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Determines how the text is aligned within the control.")]
    public new ContentAlignment TextAlign
    {
        get => _alignment;
        set
        {
            _alignment = value;
            PerformAutoSize();
            Invalidate();
        }
    }
    #endregion

    #region Protected Method Overrides
    /// <summary>
    /// Paints the background of the control.
    /// </summary>
    /// <param name="e">
    /// A <see cref="PaintEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        SetGraphicsState(g);
        base.OnPaintBackground(e);
    }
    /// <summary>
    /// Raises the <see cref="Control.Paint" /> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="PaintEventArgs"/> instance containing the event data.
    /// </param>
    protected override void OnPaint(PaintEventArgs e)
    {
        if (!string.IsNullOrEmpty(Text))
        {
            Graphics g = e.Graphics;
            TextFormatFlags formatFlags = CreateFormatFlags();
            Rectangle adjustmentRectangle = CreateAdjustmentRectangle();

            // Draw the shadow if specified.
            if (_shadow)
            {
                DrawShadow(g, adjustmentRectangle, formatFlags);
            }

            // Draw the text.
            TextRenderer.DrawText(g, Text, Font, adjustmentRectangle, ForeColor, formatFlags);
        }
    }
    /// <summary>
    /// Raises the <see cref="Control.Resize" /> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
        PerformAutoSize();
        base.OnResize(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.TextChanged"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        PerformAutoSize();
        Invalidate();
    }
    #endregion

    #region Public Methods / Functions		
    /// <summary>
    /// Gets the desired size of the control when auto-sized.
    /// </summary>
    /// <returns>
    /// A <see cref="Size"/> specifying the dimensions.
    /// </returns>
    public Size GetDesiredSize()
    {
        if (IsHandleCreated)
        {
            Graphics g = CreateGraphics();
            Size desiredSize = CalculateDesiredSize(g);
            g.Dispose();
            return desiredSize;
        }
        else
        {
            return new Size(100, 100);
        }
    }
    #endregion

    #region Private Methods / Functions
    /// <summary>
    /// Creates the adjustment rectangle for drawing.
    /// </summary>
    /// <returns>
    /// The <see cref="Rectangle"/> instance to draw in.
    /// </returns>
    private Rectangle CreateAdjustmentRectangle()
    {
        return new Rectangle(
            ClientRectangle.X - 5,
            ClientRectangle.Y - 4,
            ClientRectangle.Width + 10, 
            ClientRectangle.Height + 5);

    }
    /// <summary>
    /// Draws the shadow under the label text.
    /// </summary>
    /// <param name="g">
    /// The <see cref="Graphics"/> instance used to perform the drawing.
    /// </param>
    /// <param name="adjustmentRectangle">The adjustment rectangle.</param>
    /// <param name="formatFlags">The format flags.</param>
    private void DrawShadow(Graphics g, Rectangle adjustmentRectangle, TextFormatFlags formatFlags)
    {
        Color shadowColor = Color.FromArgb(8, 188, 188, 188);

        Rectangle rect = new(
            adjustmentRectangle.X + 2,
            adjustmentRectangle.Y + 2,
            adjustmentRectangle.Width,
            adjustmentRectangle.Height);

        TextRenderer.DrawText(g, Text, Font, rect, shadowColor, formatFlags);
    }
    /// <summary>
    /// Performs the auto-sizing of the label.
    /// </summary>
    private void PerformAutoSize(Graphics? g = null)
    {
        if (IsHandleCreated && !IsDisposed)
        {
            if (g is null)
            {
                g = CreateGraphics();
            }

            if (Dock == DockStyle.None && AutoSize && !string.IsNullOrEmpty(Text))
            {
                // Calculate the new size values.
                Size = CalculateDesiredSize(g);
            }
        }
    }
    /// <summary>
    /// Sets the state of the graphics object for drawing.
    /// </summary>
    /// <param name="g">
    /// The <see cref="Graphics"/> instance used to draw on the control.
    /// </param>
    private static void SetGraphicsState(Graphics g)
    {
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        g.TextContrast = 0;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.CompositingMode = CompositingMode.SourceCopy;
        g.CompositingQuality = CompositingQuality.HighQuality;
    }
    /// <summary>
    /// Creates the text format flags.
    /// </summary>
    /// <returns>
    /// A <see cref="TextFormatFlags"/> enumerated value indicating how to draw the text.
    /// </returns>
    private TextFormatFlags CreateFormatFlags()
    {
        TextFormatFlags formatFlags = TextFormatFlags.WordBreak;

        switch (_alignment)
        {
            case ContentAlignment.BottomCenter:
                formatFlags = formatFlags | TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                break;

            case ContentAlignment.BottomLeft:
                formatFlags = formatFlags | TextFormatFlags.Bottom | TextFormatFlags.Left;
                break;

            case ContentAlignment.BottomRight:
                formatFlags = formatFlags | TextFormatFlags.Bottom | TextFormatFlags.Right;
                break;

            case ContentAlignment.MiddleCenter:
                formatFlags = formatFlags | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                break;

            case ContentAlignment.MiddleLeft:
                formatFlags = formatFlags | TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                break;

            case ContentAlignment.MiddleRight:
                formatFlags = formatFlags | TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                break;

            case ContentAlignment.TopCenter:
                formatFlags = formatFlags | TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                break;

            case ContentAlignment.TopLeft:
                formatFlags = formatFlags | TextFormatFlags.Top | TextFormatFlags.Left;
                break;

            case ContentAlignment.TopRight:
                formatFlags = formatFlags | TextFormatFlags.Top | TextFormatFlags.Right;
                break;
        }

        return formatFlags;
    }

    /// <summary>
    /// Calculates the desired control size based on current settings.
    /// </summary>
    /// <param name="g">
    /// The <see cref="Graphics"/> instance used to measure and draw the text content.
    /// </param>
    /// <returns>
    /// A <see cref="Size"/> specifying the desired control size.
    /// </returns>
    private Size CalculateDesiredSize(Graphics g)
    {
        Size desiredSize = new();

        // Calculate the size based on text measurement.
        Size minSize = TextRenderer.MeasureText(g, Text, Font, new Size(AutoSizeMaxWidth, 2048), CreateFormatFlags());

        // Calculate the actual minimum size of the control.
        int height = minSize.Height + Padding.Top + Padding.Bottom;
        int width = minSize.Width + Padding.Left + Padding.Right;
        if (_shadow)
        {
            height += 2;
            width += 2;
        }

        // If the desired width < max auto size width, set these values accordingly.
        if (width <= AutoSizeMaxWidth)
        {
            desiredSize.Width = width;
            desiredSize.Height = height;
        }
        else
        {
            // The width will now be the max width.
            // Calculate the number of lines needed.
            int lineHeight = minSize.Height;
            int lineCount = (width / AutoSizeMaxWidth) + 1;

            desiredSize.Width = AutoSizeMaxWidth;
            desiredSize.Height = lineCount * lineHeight;
        }

        return desiredSize;
    }
    #endregion
}
