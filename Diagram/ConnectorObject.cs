using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows.Media;
using WpfApplication1.Map;

namespace WpfApplication1.Diagram
{
    /// <summary>
    /// The connector object.
    /// </summary>
    public class ConnectorObject : DiagramObject
    {
        private MapPolyline line;
        private NodeObject startNode;
        private NodeObject endNode;
        private string text;
        private double textX;
        private double textY;


        /// <summary>
        /// Recalculates the text coordinates based on start and end node coordinates.
        /// </summary>
        private void RecalculateTextCoordinates()
        {
            // Ignore if not both nodes are set
            if (this.StartNode == null || this.EndNode == null)
                return;

            this.TextX = (this.StartNode.X + this.EndNode.X) / 2.0;
            this.TextY = (this.StartNode.Y + this.EndNode.Y) / 2.0;
        }


        /// <summary>
        /// Gets or sets the start node object.
        /// </summary>
        public NodeObject StartNode
        {
            get
            {
                return this.startNode;
            }
            set
            {
                this.startNode = value;
                this.OnPropertyChanged(nameof(this.StartNode));

                // Calculate text position
                this.RecalculateTextCoordinates();

                // Rebuild line (only if it was build already)
                if (this.line != null)
                    this.RebuildLine();
            }
        }

        /// <summary>
        /// Gets or sets the end node object.
        /// </summary>
        public NodeObject EndNode
        {
            get
            {
                return this.endNode;
            }
            set
            {
                this.endNode = value;
                this.OnPropertyChanged(nameof(this.EndNode));

                // Calculate text position
                this.RecalculateTextCoordinates();

                // Rebuild line (only if it was build already)
                if (this.line != null)
                    this.RebuildLine();
            }
        }

        /// <summary>
        /// Gets or sets the connector text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));
            }
        }

        /// <summary>
        /// Gets or sets the text X coordinates.
        /// </summary>
        public double TextX
        {
            get
            {
                return this.textX;
            }
            set
            {
                this.textX = value;
                this.OnPropertyChanged(nameof(this.TextX));
            }
        }

        /// <summary>
        /// Gets or sets the text Y coordinates.
        /// </summary>
        public double TextY
        {
            get
            {
                return this.textY;
            }
            set
            {
                this.textY = value;
                this.OnPropertyChanged(nameof(this.TextY));
            }
        }

        /// <summary>
        /// The X coordinate of connector is always 0 because the line 
        /// coordinates are determined by start and end node positions.
        /// </summary>
        public override double X
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        /// <summary>
        /// The Y coordinate of connector is always 0 because the line 
        /// coordinates are determined by start and end node positions.
        /// </summary>
        public override double Y
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the line that represents this instance on the map.
        /// </summary>
        public MapPolyline Line
        {
            get
            {
                if (this.line == null)
                    this.RebuildLine();

                return this.line;
            }
        }

        /// <summary>
        /// Rebuilds the line that represents this instance on the map.
        /// </summary>
        protected void RebuildLine()
        {
            // Calculate angle of the line
            var deltaX = this.EndNode.X - this.StartNode.X;
            var deltaY = this.EndNode.Y - this.StartNode.Y;
            var angle = Math.Atan2(deltaY, deltaX);

            // Create line
            this.line = new MapPolyline()
            {
                Locations = new LocationCollection()
                        {
                            MapFunctions.CalculateUsingHaversine(new Location(this.StartNode.X, this.StartNode.Y), 0.0005, MapFunctions.RadianToDegree(angle + Math.PI / 2)),
                            MapFunctions.CalculateUsingHaversine(new Location(this.StartNode.X, this.StartNode.Y), 0.0005, MapFunctions.RadianToDegree(angle - Math.PI / 2)),
                            MapFunctions.CalculateUsingHaversine(new Location(this.EndNode.X, this.EndNode.Y), 0.0005, MapFunctions.RadianToDegree(angle - Math.PI / 2)),
                            MapFunctions.CalculateUsingHaversine(new Location(this.EndNode.X, this.EndNode.Y), 0.0005, MapFunctions.RadianToDegree(angle + Math.PI / 2)),
                        },
                Fill = new SolidColorBrush(Color.FromRgb(60, 60, 60))
            };
        }
    }
}
