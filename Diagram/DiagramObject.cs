using System.ComponentModel;
using System.Globalization;

namespace WpfApplication1.Diagram
{
    /// <summary>
    /// The diagram object base class.
    /// </summary>
    public abstract class DiagramObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets ot sets the X coordinate of the object.
        /// </summary>
        public abstract double X { get; set; }

        /// <summary>
        /// Gets ot sets the Y coordinate of the object.
        /// </summary>
        public abstract double Y { get; set; }

        /// <summary>
        /// Called when property of this object has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">The changed property name.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return this.X.ToString(CultureInfo.InvariantCulture) + "," + this.Y.ToString(CultureInfo.InvariantCulture);
        }
    }
}
