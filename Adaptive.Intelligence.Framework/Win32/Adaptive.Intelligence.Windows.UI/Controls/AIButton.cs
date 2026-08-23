using Adaptive.Intelligence.Events.Arguments;
using Adaptive.Intelligence.Events.Delegates;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Adaptive.Intelligence.Win32.UI.Controls;

/// <summary>
/// Provides a customizable button implementation.
/// </summary>
public class AIButton : Button
{
    #region Public Events
    /// <summary>
    /// Occurs when an <see cref="Exception"/> is caught during execution of the control.
    /// </summary>
    public event ExceptionEventHandler? ExecutionError;
    #endregion

    #region Private Member Declarations

    #region Private Constants

    private const int DefaultBorderWidth = 1;
    private const int DefaultImageHeight16 = 16;
    private const int DefaultImageHeight32 = 32;

    private const int MinPadding = 5;

    private static readonly Color DefaultBorderColor = Color.Gray;
    private static readonly Color DefaultNormalStartColor = Color.FromArgb(248, 248, 248);
    private static readonly Color DefaultNormalEndColor = Color.Silver;
    private static readonly Color DefaultNormalTextColor = Color.Black;

    private static readonly Color DefaultHoverStartColor = Color.FromArgb(218, 194, 204);
    private static readonly Color DefaultHoverEndColor = Color.FromArgb(224, 224, 224);
    private static readonly Color DefaultHoverTextColor = Color.Black;

    private static readonly Color DefaultPressedStartColor = Color.Gray;
    private static readonly Color DefaultPressedEndColor = Color.FromArgb(174, 45, 61);
    private static readonly Color DefaultPressedTextColor = Color.White;

    #endregion

    // Normal Mode
    private Font? _normalFont;
    private Color _normalBorderColor;
    private Color _normalStartColor;
    private Color _normalEndColor;
    private Color _normalForeColor;
    private LinearGradientMode _normalDirection;

    // Hover or Checked Mode
    private Font? _hoverFont;
    private Color _hoverBorderColor;
    private Color _hoverStartColor;
    private Color _hoverEndColor;
    private Color _hoverForeColor;
    private LinearGradientMode _hoverDirection;

    // Pressed Mode
    private Font? _pressedFont;
    private Color _pressedBorderColor;
    private Color _pressedStartColor;
    private Color _pressedEndColor;
    private Color _pressedForeColor;
    private LinearGradientMode _pressedDirection;

    // Border
    private int _borderWidth;

    // Internal flags.
    private bool _checked;
    private bool _pressed;
    private bool _hover;

    #endregion

    #region Constructor / Dispose Methods

    /// <summary>
    /// Initializes a new instance of the <see cref="AIButton"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public AIButton()
    {
        // Ensure the default objects are created.
        SetDefaults();

        // Set for owner drawing.
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.DoubleBuffer, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.Selectable, true);
        SetStyle(ControlStyles.StandardClick, true);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
    }
    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="System.Windows.Forms.ButtonBase" /> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        _normalFont = null;
        _hoverFont = null;
        _pressedFont = null;

        base.Dispose(disposing);
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating the size of the border of the button.
    /// </summary>
    /// <value>
    /// An integer indicating the width of the border, in pixels.
    /// </value>
    [Browsable(true),
     Category("Appearance"),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Gets or sets the width of the border.")]
    public int BorderWidth
    {
        get => _borderWidth;
        set
        {
            _borderWidth = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button is in a checked state.
    /// </summary>
    /// <value>
    ///   <b>true</b> if the button is in a checked state; otherwise, <b>false</b>.
    /// </value>
    [Browsable(true),
     Category("Behavior"),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Gets or sets the checked state of the button.")]
    public bool Checked
    {
        get => _checked;
        set
        {
            _checked = value;
            if (!value)
            {
                _pressed = false;
                _hover = false;
            }

            Invalidate();
        }
    }

    #region Hover States

    /// <summary>
    /// Gets or sets the color of the button when hovered over.
    /// </summary>
    /// <remarks>
    /// This only applies when the <see cref="BorderWidth"/> is greater than zero (0).
    /// </remarks>
    /// <value>
    /// A <see cref="Color"/> value.
    /// </value>
    [Browsable(true),
     Category("Hover State"),
     Description("Gets or sets the color of the border when hovered over."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverBorderColor
    {
        get => _hoverBorderColor;
        set
        {
            _hoverBorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the direction of the background gradient.
    /// </summary>
    /// <value>
    /// A <see cref="LinearGradientMode"/> enumerated value indicating the direction of the background
    /// gradient when hovered over.
    /// </value>
    [Browsable(true),
     Category("Hover State"),
     Description("Gets or sets the direction of the background gradient when hovered over."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public LinearGradientMode HoverDirection
    {
        get => _hoverDirection;
        set
        {
            _hoverDirection = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the ending color of the background gradient when hovered over.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value indicating the ending color of the background gradient when hovered over.
    /// </value>
    [Browsable(true),
     Category("Hover State"),
     Description("Gets or sets the ending color of the background gradient when hovered over."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverEndColor
    {
        get => _hoverEndColor;
        set
        {
            _hoverEndColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the font used by the text when hovered over.
    /// </summary>
    /// <value>
    /// The <see cref="Font"/> instance to use.
    /// </value>
    [Browsable(true),
     Category("Appearance"),
     Description("Get or sets the font to use when the button is hovered over."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Font? HoverFont
    {
        get => _hoverFont;
        set
        {
            _hoverFont = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the color of the button text when hovered over.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value for the text.
    /// </value>
    [Browsable(true),
     Category("Hover State"),
     Description("Gets or sets the color of the button text when hovered over."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverForeColor
    {
        get => _hoverForeColor;
        set
        {
            _hoverForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the starting color of the background gradient when hovered over.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value indicating the starting color of the background gradient when hovered over.
    /// </value>
    [Browsable(true),
     Category("Hover State"),
     Description("Gets or sets the starting color of the background gradient when hovered over."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverStartColor
    {
        get => _hoverStartColor;
        set
        {
            _hoverStartColor = value;
            Invalidate();
        }
    }

    #endregion

    /// <summary>
    /// Gets or sets the image that is displayed on a button control.
    /// </summary>
    /// <value>
    /// An <see cref="Image"/> instance, or <b>null</b>.
    /// </value>
    [Browsable(true),
     Category("Appearance"),
     Description("Get or sets the image to display in the button."),
     DefaultValue(null)]
    public new Image? Image
    {
        get => base.Image;
        set
        {
            base.Image = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the alignment of the image on the button control.
    /// </summary>
    /// <value>
    /// A <see cref="ContentAlignment"/> enumerated value.
    /// </value>
    [DefaultValue(ContentAlignment.MiddleCenter)]
    [Localizable(true)]
    [Description("Determines how the image is aligned on the button.")]
    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new ContentAlignment ImageAlign
    {
        get => base.ImageAlign;
        set
        {
            base.ImageAlign = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the alignment of the image on the button control.
    /// </summary>
    /// <value>
    /// A <see cref="ContentAlignment"/> enumerated value.
    /// </value>
    [DefaultValue(ContentAlignment.MiddleCenter)]
    [Description("Determines how the text is aligned on the button.")]
    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [RefreshProperties(RefreshProperties.Repaint)]
    public new ContentAlignment TextAlign
    {
        get => base.TextAlign;
        set
        {
            base.TextAlign = value;
            Invalidate();
        }
    }

    #region Normal States

    /// <summary>
    /// Gets or sets the color of the button in a normal state.
    /// </summary>
    /// <remarks>
    /// This only applies when the <see cref="BorderWidth"/> is greater than zero (0).
    /// </remarks>
    /// <value>
    /// A <see cref="Color"/> value.
    /// </value>
    [Browsable(true),
     Category("Normal State"),
     Description("Gets or sets the color of the border in a normal state."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color NormalBorderColor
    {
        get => _normalBorderColor;
        set
        {
            _normalBorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the direction of the background gradient in a normal state.
    /// </summary>
    /// <value>
    /// A <see cref="LinearGradientMode"/> enumerated value indicating the direction of the background
    /// gradient in a normal state.
    /// </value>
    [Browsable(true),
     Category("Normal State"),
     Description("Gets or sets the direction of the background gradient in a normal state."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public LinearGradientMode NormalDirection
    {
        get => _normalDirection;
        set
        {
            _normalDirection = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the ending color of the background gradient in a normal state.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value indicating the ending color of the background gradient in a normal state.
    /// </value>
    [Browsable(true),
     Category("Normal State"),
     Description("Gets or sets the ending color of the background gradient in a normal state."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color NormalEndColor
    {
        get => _normalEndColor;
        set
        {
            _normalEndColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the font used by the text in a normal state.
    /// </summary>
    /// <value>
    /// The <see cref="Font"/> instance to use.
    /// </value>
    [Browsable(true),
     Category("Appearance"),
     Description("Get or sets the font to use in a normal state."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Font? NormalFont
    {
        get => _normalFont;
        set
        {
            _normalFont = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the color of the button text in a normal state
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value for the text.
    /// </value>
    [Browsable(true),
     Category("Normal State"),
     Description("Gets or sets the color of the text in a normal state."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color NormalForeColor
    {
        get => _normalForeColor;
        set
        {
            _normalForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the starting color of the background gradient in a normal state.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value indicating the starting color of the background gradient in a normal state.
    /// </value>
    [Browsable(true),
     Category("Normal State"),
     Description("Gets or sets the starting color of the background gradient in a normal state."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color NormalStartColor
    {
        get => _normalStartColor;
        set
        {
            _normalStartColor = value;
            Invalidate();
        }
    }

    #endregion

    #region Pressed States

    /// <summary>
    /// Gets or sets the color of the button when pressed.
    /// </summary>
    /// <remarks>
    /// This only applies when the <see cref="BorderWidth"/> is greater than zero (0).
    /// </remarks>
    /// <value>
    /// A <see cref="Color"/> value.
    /// </value>
    [Browsable(true),
     Category("Pressed State"),
     Description("Gets or sets the color of the border when pressed."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color PressedBorderColor
    {
        get => _pressedBorderColor;
        set
        {
            _pressedBorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the direction of the background gradient when pressed.
    /// </summary>
    /// <value>
    /// A <see cref="LinearGradientMode"/> enumerated value indicating the direction of the background
    /// gradient when pressed.
    /// </value>
    [Browsable(true),
     Category("Pressed State"),
     Description("Gets or sets the direction of the background gradient when pressed."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public LinearGradientMode PressedDirection
    {
        get => _pressedDirection;
        set
        {
            _pressedDirection = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the ending color of the background gradient when pressed.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value indicating the ending color of the background gradient when pressed.
    /// </value>
    [Browsable(true),
     Category("Pressed State"),
     Description("Gets or sets the ending color of the background gradient when pressed."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color PressedEndColor
    {
        get => _pressedEndColor;
        set
        {
            _pressedEndColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the font used by the text when pressed.
    /// </summary>
    /// <value>
    /// The <see cref="Font"/> instance to use.
    /// </value>
    [Browsable(true),
     Category("Appearance"),
     Description("Get or sets the font to use when the button is pressed."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Font? PressedFont
    {
        get => _pressedFont;
        set
        {
            _pressedFont = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the color of the button text when pressed.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value for the text.
    /// </value>
    [Browsable(true),
     Category("Pressed State"),
     Description("Gets or sets the color of the text when pressed."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color PressedForeColor
    {
        get => _pressedForeColor;
        set
        {
            _pressedForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the starting color of the background gradient when pressed.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> value indicating the starting color of the background gradient when pressed.
    /// </value>
    [Browsable(true),
     Category("Pressed State"),
     Description("Gets or sets the starting color of the background gradient when pressed."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color PressedStartColor
    {
        get => _pressedStartColor;
        set
        {
            _pressedStartColor = value;
            Invalidate();
        }
    }

    #endregion

    #endregion

    #region Protected Method Overrides
    /// <summary>
    /// Raises the <see cref="ExecutionError"/> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="ExceptionEventArgs"/> instance containing the related Exception.
    /// </param>
    protected virtual void OnExecutionError(ExceptionEventArgs e)
    {
        ExecutionError?.Invoke(this, e);
    }

    /// <summary>
    /// Raises the <see cref="Control.OnMouseEnter(System.EventArgs)" /> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
    protected override void OnMouseEnter(EventArgs e)
    {
        _hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.OnMouseLeave(System.EventArgs)" /> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.OnMouseDown(MouseEventArgs)" /> event.
    /// </summary>
    /// <param name="mevent">A <see cref="MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        _hover = true;
        _pressed = true;
        Invalidate();
        base.OnMouseDown(mevent);
    }

    /// <summary>
    /// Raises the <see cref="Control.OnMouseUp(MouseEventArgs)" /> event.
    /// </summary>
    /// <param name="mevent">A <see cref="MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        _pressed = false;
        Invalidate();
        base.OnMouseUp(mevent);
    }

    /// <summary>
    /// Paints the background of the control.
    /// </summary>
    /// <param name="pevent">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        base.OnPaintBackground(pevent);
        //SetDrawingQuality(pevent.Graphics);
        //DrawBackground(pevent.Graphics);
    }

    /// <summary>
    /// Raises the <see cref="Control.Paint" /> event.
    /// </summary>
    /// <param name="pevent">A <see cref="PaintEventArgs" /> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs pevent)
    {
        Graphics g = pevent.Graphics;
        SetDrawingQuality(g);
        DrawBackground(g);

        // Use the correct font.
        Font? textFont = GetFontReference();

        // Draw the image.
        DrawButtonImage(g);

        // Draw the text.
        DrawText(g, textFont);

        // Draw the border.
        DrawBorder(g);
    }

    /// <summary>Raises the <see cref="Control.Resize" /> event.</summary>
    /// <param name="e">An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnResize(EventArgs e)
    {
        Invalidate();
        base.OnResize(e);
        Invalidate();
    }

    #endregion

    #region Private Methods / Functions

    /// <summary>
    /// Calculates the image position.
    /// </summary>
    /// <returns>
    /// A <see cref="Rectangle"/> containing the X and Y co-ordinates and the width and height of the image.
    /// </returns>
    private Rectangle CalculateImagePosition()
    {
        int imageX = 0;
        int imageY = 0;

        int height = DefaultImageHeight16;
        int width = DefaultImageHeight16;

        if (Image != null)
        {
            int imageHeight;
            try
            {
                imageHeight = Image.Height;
            }
            catch
            {
                imageHeight = DefaultImageHeight16;
            }

            if (imageHeight >= DefaultImageHeight32)
            {
                height = DefaultImageHeight32;
                width = DefaultImageHeight32;
            }

            // Calculate the Y location.
            switch (ImageAlign)
            {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                    imageY = Height - MinPadding - height;
                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                    imageY = MinPadding;
                    break;

                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    imageY = (Height - height) / 2;
                    break;
            }

            // Calculate the X location.
            switch (ImageAlign)
            {
                case ContentAlignment.TopCenter:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                    imageX = (Width - width) / 2;
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    imageX = Width - (width + MinPadding);
                    break;

                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    imageX = MinPadding;
                    break;
            }
        }

        return new Rectangle(imageX, imageY, width, height);
    }
    /// <summary>
    /// Draws the button border.
    /// </summary>
    /// <param name="g">
    /// The <see cref="Graphics"/> object used to draw the button.
    /// </param>
    private void DrawBorder(Graphics g)
    {
        if (_borderWidth > 0)
        {
            using Pen borderPen = GetBorderPen();
            g.DrawRectangle(borderPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
        }
    }
    /// <summary>
    /// Draws the text
    /// </summary>
    /// <param name="g">
    /// The <see cref="Graphics"/> object used to draw the button.
    /// </param>
    /// <param name="textFont">
    /// The <see cref="Font"/> to use with the text.
    /// </param>
    private void DrawText(Graphics? g, Font? textFont)
    {
        if (g != null && textFont != null)
        {
            TextFormatFlags formatFlags = GetTextFormatFlags();
            Rectangle adjustmentRectangle = CalculateTextRectangle();

            TextRenderer.DrawText(g, Text, textFont, adjustmentRectangle, GetTextColor(), formatFlags);
        }
    }
    /// <summary>
    /// Gets the text format flags.
    /// </summary>
    /// <returns>
    /// A <see cref="TextFormatFlags"/> enumerated value indicating how to draw the text.
    /// </returns>
    private TextFormatFlags GetTextFormatFlags()
    {
        TextFormatFlags formatFlags = TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis;

        switch (TextAlign)
        {
            case ContentAlignment.BottomCenter:
                formatFlags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                break;

            case ContentAlignment.BottomLeft:
                formatFlags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                break;

            case ContentAlignment.BottomRight:
                formatFlags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                break;

            case ContentAlignment.MiddleCenter:
                formatFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                break;

            case ContentAlignment.MiddleLeft:
                formatFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                break;

            case ContentAlignment.MiddleRight:
                formatFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                break;

            case ContentAlignment.TopCenter:
                formatFlags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                break;

            case ContentAlignment.TopLeft:
                formatFlags |= TextFormatFlags.Top | TextFormatFlags.Left;
                break;

            case ContentAlignment.TopRight:
                formatFlags |= TextFormatFlags.Top | TextFormatFlags.Right;
                break;
        }

        return formatFlags;
    }
    /// <summary>
    /// Calculates the actual drawing area for the text, based on the image settings.
    /// </summary>
    /// <returns>
    /// The <see cref="Rectangle"/> to draw the text in.
    /// </returns>
    private Rectangle CalculateTextRectangle()
    {
        int x = ClientRectangle.X;
        int y = ClientRectangle.Y;
        int width = ClientRectangle.Width;
        int height = ClientRectangle.Height;

        if (Image == null || TextImageRelation == TextImageRelation.Overlay)
        {
            // Standard Positioning
            y -= 2;
        }
        else
        {
            switch (TextImageRelation)
            {
                case TextImageRelation.ImageBeforeText:
                    y = -2;
                    x += Image.Width;
                    width -= Image.Width;
                    break;

                case TextImageRelation.ImageAboveText:
                    y += Image.Height + 2;
                    height -= y + 2;
                    break;

                case TextImageRelation.TextAboveImage:
                    y = -2;
                    height -= Image.Height;
                    break;

                case TextImageRelation.TextBeforeImage:
                    y -= 2;
                    width -= Image.Width;
                    break;
            }
        }

        return new Rectangle(x, y, width, height);
    }

    /// <summary>
    /// Draws the background of the control based on its state.
    /// </summary>
    /// <param name="g">The <see cref="Graphics"/> instance to use.
    /// </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "<Pending>")]
    private void DrawBackground(Graphics g)
    {
        LinearGradientBrush backBrush;

        Rectangle actualRectangle = new(ClientRectangle.X, ClientRectangle.Y - 1, ClientRectangle.Width,
            ClientRectangle.Height + 2);

        if (_pressed)
        {
            backBrush = new LinearGradientBrush(actualRectangle, _pressedStartColor, _pressedEndColor,
                _pressedDirection);

        }
        else if (_hover || _checked)
        {
            backBrush = new LinearGradientBrush(actualRectangle, _hoverStartColor, _hoverEndColor,
                _hoverDirection);

        }
        else
        {
            backBrush = new LinearGradientBrush(actualRectangle, _normalStartColor, _normalEndColor,
                _normalDirection);
        }

        g.FillRectangle(backBrush, actualRectangle);
        backBrush.Dispose();
    }

    /// <summary>
    /// Draws the button image.
    /// </summary>
    /// <param name="g">The <see cref="Graphics"/> instance to use.
    /// </param>
    private void DrawButtonImage(Graphics? g)
    {
        if (base.Image != null)
        {
            Rectangle location = CalculateImagePosition();
            try
            {
                g?.DrawImage(base.Image, location);
            }
            catch (Exception ex)
            {
                OnExecutionError(new ExceptionEventArgs(ex));
            }
        }
    }

    /// <summary>
    /// Gets the reference to the font to use based on the button's state.
    /// </summary>
    /// <returns>
    /// A <see cref="Font"/> instance.
    /// </returns>
    private Font? GetFontReference()
    {
        Font? textFont;
        if (_pressed)
        {
            textFont = _pressedFont;
        }
        else if (_hover || _checked)
        {
            textFont = _hoverFont;
        }
        else
        {
            textFont = _normalFont;
        }

        return textFont;
    }

    /// <summary>
    /// Creates the solid brush object used to draw the text.
    /// </summary>
    /// <returns>
    /// A <see cref="SolidBrush"/> instance.
    /// </returns>
    private Color GetTextColor()
    {
        Color color;
        if (_pressed)
        {
            color = _pressedForeColor;
        }
        else if (_hover || _checked)
        {
            color = _hoverForeColor;
        }
        else if (!Enabled)
        {
            color = SystemColors.GrayText;
        }
        else
        {
            color = _normalForeColor;
        }

        return color;
    }

    /// <summary>
    /// Creates the pen object used to draw the text.
    /// </summary>
    /// <returns>
    /// A <see cref="Pen"/> instance.
    /// </returns>
    private Pen GetBorderPen()
    {
        Pen borderPen;
        if (_pressed)
        {
            borderPen = new Pen(_pressedBorderColor, _borderWidth);
        }
        else if (_hover || _checked)
        {
            borderPen = new Pen(_hoverBorderColor, _borderWidth);
        }
        else
        {
            borderPen = new Pen(_normalBorderColor, _borderWidth);
        }

        return borderPen;
    }

    /// <summary>
    /// Sets the defaults property and object instances.
    /// </summary>
    private void SetDefaults()
    {
        _normalFont = new Font(UiConstants.StandardFontName, UiConstants.StandardFontSize, FontStyle.Regular);
        _hoverFont = new Font(UiConstants.StandardFontName, UiConstants.StandardFontSize, FontStyle.Regular);
        _pressedFont = new Font(UiConstants.StandardFontName, UiConstants.StandardFontSize, FontStyle.Regular);

        _normalStartColor = DefaultNormalStartColor;
        _normalEndColor = DefaultNormalEndColor;
        _normalForeColor = DefaultNormalTextColor;
        _normalDirection = LinearGradientMode.ForwardDiagonal;

        _hoverStartColor = DefaultHoverStartColor;
        _hoverEndColor = DefaultHoverEndColor;
        _hoverForeColor = DefaultHoverTextColor;
        _hoverDirection = LinearGradientMode.BackwardDiagonal;

        _pressedStartColor = DefaultPressedStartColor;
        _pressedEndColor = DefaultPressedEndColor;
        _pressedForeColor = DefaultPressedTextColor;
        _pressedDirection = LinearGradientMode.BackwardDiagonal;

        _borderWidth = DefaultBorderWidth;
        _normalBorderColor = DefaultBorderColor;
        _pressedBorderColor = DefaultBorderColor;
        _hoverBorderColor = DefaultBorderColor;
    }
    /// <summary>
    /// Sets the drawing quality.
    /// </summary>
    /// <param name="g">
    /// The <see cref="Graphics"/> instance to be set.
    /// </param>
    private static void SetDrawingQuality(Graphics? g)
    {
        if (g != null)
        {
            // Set the drawing quality.
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }
    }
    #endregion
}
