using System.ComponentModel;

namespace Adaptive.Intelligence.Common;

/// <summary>
/// Provides the base implementation for classes that can notify subscribers when a property value changes.
/// </summary>
/// <seealso cref="INotifyPropertyChanged" />
public abstract class PropertyAwareBase : DisposableObjectBase, INotifyPropertyChanged
{
    #region Public Events		
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region Dispose Method
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyAwareBase"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    protected PropertyAwareBase() : base()
    {

    }
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
    /// <b>false</b> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        PropertyChanged = null;
        base.Dispose(disposing);
    }
    #endregion

    #region Event Methods		
    /// <summary>
    /// Called when a property value is changed.
    /// </summary>
    /// <param name="propertyName">
    /// A string containing the name of the property.
    /// </param>
    protected virtual void OnPropertyChanged(string? propertyName)
    {
        if (!string.IsNullOrEmpty(propertyName))
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }
    #endregion
}
