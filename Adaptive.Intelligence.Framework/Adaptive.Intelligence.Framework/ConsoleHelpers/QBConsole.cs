using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;

namespace Adaptive.Intelligence.ConsoleHelpers
{
    /// <summary>
    /// Provides extensions to the standard System.Console operations.
    /// </summary>
    /// <remarks>
    /// This class is inspired by Microsoft QuickBasic 4.5 and the Commodore 64.
    /// </remarks>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class QBConsole : DisposableObjectBase
    {
        #region Private Member Declarations

        /// <summary>
        /// The fore color.
        /// </summary>
        private ConsoleColor _foreColor = ConsoleColor.Blue;
        /// <summary>
        /// The back color.
        /// </summary>
        private ConsoleColor _backColor = ConsoleColor.White;
        /// <summary>
        /// The character set for old-school ASCII.
        /// </summary>
        private char[]? _characterSet;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="QBConsole"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public QBConsole()
        {
            InitializeCharacterSet(false);
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QBConsole"/> class.
        /// </summary>
        /// <param name="adjustCharacterSet"></param>
        public QBConsole(bool adjustCharacterSet)
        {
            InitializeCharacterSet(adjustCharacterSet);
            InitializeComponent();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (_characterSet != null)
            {
                Array.Clear(_characterSet, 0, 256);
            }

            _characterSet = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Centers the text within the specified number of spaces.
        /// </summary>
        /// <param name="width">
        /// An integer specifying the total width in which to center the text.
        /// </param>
        /// <param name="content">
        /// A string containing the content to be centered.
        /// </param>
        /// <returns>
        /// A string of the specified width, with the specified text centered within.
        /// </returns>
        public static string CenterText(int width, string content)
        {
            string returnContent;

            if (content.Length >= width)
            {
                returnContent = content[..width];
            }
            else
            {
                int spaces = width - content.Length;
                int left = spaces / 2;

                returnContent = new string(CharacterConstants.SpaceChar, left) + content;
                returnContent += new string(CharacterConstants.SpaceChar, width - returnContent.Length);
            }

            return returnContent;
        }
        /// <summary>
        /// Clears the specified portion of the console screen.
        /// </summary>
        /// <param name="x">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="y">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="width">
        /// An integer specifying the number of columns to clear.
        /// </param>
        /// <param name="height">
        /// An integer specifying the number of rows to clear.
        /// </param>
        public static void Clear(int x, int y, int width, int height)
        {
            if (width > 0 && height > 0)
            {
                for (int c = 0; c < height; c++)
                {
                    System.Console.SetCursorPosition(x, y);
                    System.Console.Write(new string(' ', width));
                    y++;
                }
            }
        }
        /// <summary>
        /// Clears the console screen.
        /// </summary>
        public static void Cls()
        {
            System.Console.Clear();
        }
        /// <summary>
        /// Clears the console screen.
        /// </summary>
        /// <param name="foreground">
        /// A <see cref="ConsoleColor"/> specifying the new fore ground color for the screen.
        /// </param>
        /// <param name="background">
        /// A <see cref="ConsoleColor"/> specifying the new fore ground color for the screen.
        /// </param>
        public void Cls(ConsoleColor foreground, ConsoleColor background)
        {
            Color(foreground, background);
            Cls();
        }
        /// <summary>
        /// Sets the colors for the console to use.
        /// </summary>
        /// <param name="foreground">
        /// A <see cref="ConsoleColor"/> specifying the new fore ground color for the screen.
        /// </param>
        /// <param name="background">
        /// A <see cref="ConsoleColor"/> specifying the new fore ground color for the screen.
        /// </param>
        public void Color(ConsoleColor foreground, ConsoleColor background)
        {
            _foreColor = foreground;
            _backColor = background;
            System.Console.BackgroundColor = _backColor;
            System.Console.ForegroundColor = _foreColor;
        }
        /// <summary>
        /// Draws the progress bar with the specified percentage.
        /// </summary>
        /// <param name="x">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="y">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="width">
        /// An integer specifying the number of columns to clear.
        /// </param>
        /// <param name="percent">
        /// An integer specifying the percentage value in the range of 0 - 100.
        /// </param>
        public void DrawProgressBar(int x, int y, int width, int percent)
        {
            if (percent < 0)
            {
                percent = 0;
            }
            else if (percent > 100)
            {
                percent = 100;
            }

            var leftChars = (int)(width * (float)percent / 100);
            var rightChars = width - leftChars;

            string left = new(GetAsciiCharacter(177), leftChars);
            string right = new(GetAsciiCharacter(178), rightChars);

            System.Console.SetCursorPosition(x, y);
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write(left);
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.Write(right);
        }
        /// <summary>
        /// Draws the title bar.
        /// </summary>
        public static void DrawTitleBar()
        {
            System.Console.SetCursorPosition(0, 0);
            System.Console.Write(new string(' ', 80));
        }
        /// <summary>
        /// Sets the cursor position to the specified co-ordinates.
        /// </summary>
        /// <param name="x">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="y">
        /// An integer specifying the X co-ordinate.
        /// </param>
        public static void Locate(int x, int y)
        {
            System.Console.SetCursorPosition(x, y);
        }
        /// <summary>
        /// Prints the specified content.
        /// </summary>
        /// <param name="content">
        /// A string containing the content to display.
        /// </param>
        public static void Print(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                System.Console.Write(content);
            }
        }
        /// <summary>
        /// Prints the specified content at the specified location.
        /// </summary>
        /// <param name="x">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="y">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="content">
        /// A string containing the content to display.
        /// </param>
        public static void Print(int x, int y, string content)
        {
            System.Console.SetCursorPosition(x, y);
            System.Console.Write(content);
        }
        /// <summary>
        /// Prints the specified content at the specified location, centered in the specified width.
        /// </summary>
        /// <param name="x">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="y">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="width">
        /// An integer specifying the total width of the display area, in character columns.
        /// </param>
        /// <param name="content">
        /// A string containing the content to display.
        /// </param>
        public static void PrintCenter(int x, int y, int width, string content)
        {
            System.Console.SetCursorPosition(x, y);
            string line = CenterText(width, content);
            System.Console.Write(line);
        }
        /// <summary>
        /// Draws a box (with double-lined characters) in the specified area.
        /// </summary>
        /// <param name="x">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="y">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="width">
        /// An integer specifying the total width of the display area, in character columns.
        /// </param>
        /// <param name="height">
        /// An integer specifying the total number of rows to drawm including borders.
        /// </param>
        public void PrintDoubleBox(int x, int y, int width, int height)
        {
            System.Console.SetCursorPosition(x, y);
            int rows = height - 2;

            System.Console.SetCursorPosition(x, y);
            PrintDoubleBoxTopLeft();
            if (width > 0)
            {
                System.Console.Write(new string(GetAsciiCharacter(205), width - 2));
            }

            PrintDoubleBoxTopRight();

            for (int row = 1; row <= rows; row++)
            {
                System.Console.SetCursorPosition(x, y + row);
                PrintDoubleBoxWindowRow(width);
            }
            System.Console.SetCursorPosition(x, y + height - 1);
            PrintDoubleBoxBottomLeft();
            if (width > 0)
            {
                PrintDoubleBoxHorizontal(width - 2);
            }

            PrintDoubleBoxBottomRight();
        }
        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="x">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="y">
        /// An integer specifying the X co-ordinate.
        /// </param>
        /// <param name="width">
        /// An integer specifying the total width of the display area, in character columns.
        /// </param>
        /// <param name="height">
        /// An integer specifying the total number of rows to draw including borders.
        /// </param>
        /// <param name="text">
        /// A string containing the text to display.
        /// </param>
        public void ShowMessage(int x, int y, int width, int height, string text)
        {
            PrintDoubleBox(x, y, width, height);
            Color(ConsoleColor.Yellow, ConsoleColor.Blue);
            PrintCenter(x + 1, y + 1, width - 2, text);
        }
        #endregion

        #region Private Methods / Functions		
        /// <summary>
        /// Prints the bottom left character for a double-bordered box. 
        /// </summary>
        private void PrintDoubleBoxBottomLeft()
        {
            System.Console.Write(GetAsciiCharacter(200));
        }
        /// <summary>
        /// Prints the bottom right character for a double-bordered box. 
        /// </summary>
        private void PrintDoubleBoxBottomRight()
        {
            System.Console.Write(GetAsciiCharacter(188));
        }
        /// <summary>
        /// Prints the horizontal border character for a double-bordered box. 
        /// </summary>
        private void PrintDoubleBoxHorizontal(int width)
        {
            System.Console.Write(new string(GetAsciiCharacter(205), width));
        }
        /// <summary>
        /// Prints the top left character for a double-bordered box. 
        /// </summary>
        private void PrintDoubleBoxTopLeft()
        {
            System.Console.Write(GetAsciiCharacter(201));
        }
        /// <summary>
        /// Prints the top right character for a double-bordered box. 
        /// </summary>
        private void PrintDoubleBoxTopRight()
        {
            System.Console.Write(GetAsciiCharacter(187));
        }
        /// <summary>
        /// Prints the vertical border character for a double-bordered box. 
        /// </summary>
        private void PrintDoubleBoxVertical()
        {
            System.Console.Write(GetAsciiCharacter(186));
        }
        /// <summary>
        /// Gets the definition for PrintDoubleBoxWindowRow.
        /// </summary>
        private void PrintDoubleBoxWindowRow(int width)
        {
            if (width > 2)
            {
                PrintDoubleBoxVertical();
                System.Console.Write(new string(' ', width - 2));
                PrintDoubleBoxVertical();
            }
        }
        /// <summary>
        /// Initializes the character set.
        /// </summary>
        private void InitializeCharacterSet(bool adjustCharacterSet)
        {
            byte[] data = new byte[256];
            for (int count = 0; count < 256; count++)
            {
                data[count] = (byte)count;
            }

            try
            {
                if (adjustCharacterSet)
                {
                    System.Console.OutputEncoding = System.Text.Encoding.GetEncoding(437);
                    _characterSet = System.Text.Encoding.GetEncoding(437).GetChars(data);
                }
                else
                {
                    _characterSet = System.Text.Encoding.ASCII.GetChars(data);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message, ex);
            }
        }
        /// <summary>
        /// Initializes the console for use.
        /// </summary>
        private void InitializeComponent()
        {
#if WIN32
                System.Console.SetWindowSize(WindowsSizeWidth, WindowsSizeHeight);
#endif
            System.Console.CursorVisible = false;
            System.Console.ForegroundColor = _foreColor;
            System.Console.BackgroundColor = _backColor;
        }

        /// <summary>
        /// Gets the definition for GetAsciiCharacter.
        /// </summary>
        private char GetAsciiCharacter(int characterNumber)
        {
            char returnChar = CharacterConstants.NullChar;

            if (_characterSet != null)
            {
                returnChar = _characterSet[characterNumber];
            }

            return returnChar;
        }
        #endregion
    }
}