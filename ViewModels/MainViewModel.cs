using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApplication1.Diagram;
using WpfApplication1.Google;
using System.Collections.Generic;
using Microsoft.Maps.MapControl.WPF;

namespace WpfApplication1.ViewModels
{
    /// <summary>
    /// The view model for <see cref="MainWIndow"/>.
    /// </summary>
    public sealed class MainViewModel : ViewModelBase
    {
        private const string googleMatrixApiKey = "<YOUR KEY HERE>";
        private double scale = 1;

        /// <summary>
        /// Gets the collection of diagram nodes.
        /// </summary>
        public ObservableCollection<NodeObject> Nodes { get; } = new ObservableCollection<NodeObject>();

        /// <summary>
        /// Gets the collection of diagram connectors.
        /// </summary>
        public ObservableCollection<ConnectorObject> Connectors { get; } = new ObservableCollection<ConnectorObject>();

        /// <summary>
        /// Gets or sets the diagram scale.
        /// </summary>
        public double Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Handles the mouse click on the map.
        /// This will add new node on given location and connect it to other nodes.
        /// </summary>
        /// <param name="location">The location that the click cooresponds to.</param>
        public void HandleClick(Location location)
        {
            var node = new NodeObject()
            {
                X = location.Latitude,
                Y = location.Longitude
            };

            this.Nodes.Add(node);
            this.ConnectPoint(node);
        }

        /// <summary>
        /// Connects the specified node with other nodes in the collection.
        /// </summary>
        /// <param name="node">The node.</param>
        private async void ConnectPoint(NodeObject node)
        {
            // Send the request with origin as specified node
            // and destinations all other nodes except specified origin node
            var origin = node;
            var originCollection = new List<NodeObject>() { origin };
            var weights = await new GoogleMatrixApiClient(googleMatrixApiKey).RequestMatrix(
                originCollection.Select(n => n.ToString()),
                this.Nodes.Except(originCollection).Select(n => n.ToString()));

            // Ignore if nothing returned
            if (weights.Count() == 0) return;

            // There should only one row in response so we'll use that
            var weight = weights.First();
            for (var connectionIndex = 0; connectionIndex < weight.Count(); connectionIndex++)
            {
                // Ignore 0 weight values
                if (weight.ElementAt(connectionIndex) == 0) continue;

                // Create connector between origin node and
                // destination node where text is travel duration in minutes
                this.Connectors.Add(new ConnectorObject
                {
                    StartNode = origin,
                    EndNode = this.Nodes[connectionIndex],
                    Text = (weight.ElementAt(connectionIndex) / 60.0).ToString("0.00")
                });
            }
        }
    }
}
