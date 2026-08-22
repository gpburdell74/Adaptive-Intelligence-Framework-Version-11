using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Utility
{
    /// <summary>
    /// Provides a simple undo buffer stack implementation.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public class UndoBuffer<T> : DisposableObjectBase
    {
        #region Public Events        
        /// <summary>
        /// Occurs when the undo buffer changes.
        /// </summary>
        public event EventHandler? UndoBufferChanged;
        #endregion

        #region Private Member Declarations        
        /// <summary>
        /// The buffer/stack.
        /// </summary>
        private Stack<T>? _buffer;
        #endregion

        #region Constructor / Dispose Methods        
        /// <summary>
        /// Initializes a new instance of the <see cref="UndoBuffer{T}"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public UndoBuffer()
        {
            _buffer = new Stack<T>(1024);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _buffer?.Clear();
                _buffer?.TrimExcess();
            }

            _buffer = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets a value indicating whether there is anything present in the undo buffer.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the undo buffer has data; otherwise, <c>false</c>.
        /// </value>
        public bool HasData => _buffer!.Count > 0;
        #endregion

        #region Public Methods / Functions        
        /// <summary>
        /// Adds the specified item to the undo stack.
        /// </summary>
        /// <param name="item">
        /// The <typeparamref name="T"/> instance to add.
        /// </param>
        public void Add(T item)
        {
            _buffer!.Push(item);
            OnUndoBufferChanged();
        }
        /// <summary>
        /// Determines whether the specified item is the same as the last item on the stack.
        /// </summary>
        /// <param name="itemToCheck">The item to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified item is the same as the last item on the stack; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSame(T itemToCheck)
        {
            if ((itemToCheck == null) && (_buffer == null))
            {
                return true;
            }

            if (itemToCheck == null || _buffer == null || _buffer.Count == 0)
            {
                return false;
            }

            return itemToCheck.Equals(_buffer.Peek());
        }
        /// <summary>
        /// Gets the last item on the stack and removes it.
        /// </summary>
        /// <returns>
        /// The last <typeparamref name="T"/> item on the stack, or <b>null</b> if the stack is empty.
        /// </returns>
        public T? GetLast()
        {
            T? value = default;
            if (_buffer!.Count > 0)
            {
                value = _buffer.Pop();
                OnUndoBufferChanged();
            }
            return value;
        }
        #endregion

        #region Protected Event Methods        
        /// <summary>
        /// Raises the <see cref="UndoBufferChanged"/> event.
        /// </summary>
        protected virtual void OnUndoBufferChanged()
        {
            UndoBufferChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }

}
