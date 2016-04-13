using Microsoft.Maps.MapControl.WPF;

namespace WpfApplication1.Diagram
{
    /// <summary>
    /// The node object.
    /// </summary>
    public class NodeObject : DiagramObject
    {
        private Pushpin pin;
        private double x;
        private double y;


        /// <summary>
        /// Gets or sets the X coordinate of the node.
        /// </summary>
        public override double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
                this.OnPropertyChanged(nameof(this.X));
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the node.
        /// </summary>
        public override double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
                this.OnPropertyChanged(nameof(this.Y));
            }
        }

        /// <summary>
        /// Gets the pin for this instance.
        /// </summary>
        public Pushpin Pin
        {
            get
            {
                if (this.pin == null)
                {
                    this.pin = new Pushpin()
                    {
                        Location = this.Location
                    };
                }

                return this.pin;
            }
        }

        /// <summary>
        /// Gets new instance of location based on current instance coordinates.
        /// </summary>
        protected Location Location => new Location(this.X, this.Y);

        /// <summary>
        /// Compares specified object with this instance 
        /// and determines whether they are equal.
        /// </summary>
        /// <param name="obj">The object to compare this instance to.</param>
        /// <returns>Returns <c>True</c> if specified object is equal to this instance; <c>False</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            // Not equal if other object is null
            if (obj == null) return false;

            // Not equal if object can't be implicitly casted to this type
            var node = obj as NodeObject;
            if (node == null) return false;

            // Equal only if coordinates are equal
            return node.x == this.x && node.y == this.y;
        }

        /// <summary>
        /// Generates the hash code of this instance.
        /// </summary>
        /// <returns>Returns hash code of this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ this.X.GetHashCode();
                hash = (hash * 16777619) ^ this.Y.GetHashCode();
                return hash;
            }
        }
    }
}
