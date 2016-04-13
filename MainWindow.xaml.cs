using Microsoft.Maps.MapControl.WPF;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication1.Diagram;
using WpfApplication1.Map;
using WpfApplication1.Providers;
using WpfApplication1.ViewModels;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string googleKey = "AIzaSyCZuYOphggEHKWTR69z5tDMrErKENtX2p0";

        /// <summary>
        /// Instantiates new <see cref="MainWindow"/> control.
        /// </summary>
        public MainWindow()
        {
            // Initialize data context
            this.DataContext = ViewModelProvider.MainViewModel;
            this.ViewModel.Nodes.CollectionChanged += Nodes_CollectionChanged;
            this.ViewModel.Connectors.CollectionChanged += Connectors_CollectionChanged;

            // Initialize UI
            this.InitializeComponent();

            // Populate UI from data context
            this.AddConnectors(this.ViewModel.Connectors);
            this.AddNodes(this.ViewModel.Nodes);

            // Attach to events
            this.Map.PreviewMouseDown += Map_PreviewMouseDown;
        }

        /// <summary>
        /// Handle the right click mouse down event on the map.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The mouse event arguments.</param>
        private void Map_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // On right click, calculates the position based on 
            // mouse location on the map
            if (e.RightButton == MouseButtonState.Pressed)
                this.ViewModel.HandleClick(this.Map.ViewportPointToLocation(e.GetPosition(this.Map)));
        }

        /// <summary>
        /// Handles the connectors collection change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Connectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.AddConnectors(e.NewItems.OfType<ConnectorObject>());
        }

        /// <summary>
        /// Adds the specified connectors to the map.
        /// </summary>
        /// <param name="nodes">The connectors collection.</param>
        private void AddConnectors(IEnumerable<ConnectorObject> connectors)
        {
            foreach (var connector in connectors)
            {
                // Insert so that connectors render before nodes 
                this.Map.Children.Insert(0, connector.Line);

                // Creates the label for the connector one the map
                // Label will be located in the mid-point of the line
                var label = new TextBlock();
                label.Text = connector.Text;
                label.Foreground = Brushes.Black;
                label.FontSize = 16;
                MapLayer.SetPosition(label, MapFunctions.CalculateUsingHaversine(new Location(connector.TextX, connector.TextY), 0, 0));
                this.Map.Children.Add(label);
            }
        }

        /// <summary>
        /// Handles the nodes collection change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.AddNodes(e.NewItems.OfType<NodeObject>());
        }

        /// <summary>
        /// Adds the specified nodes to the map.
        /// </summary>
        /// <param name="nodes">The nodes collection.</param>
        private void AddNodes(IEnumerable<NodeObject> nodes)
        {
            foreach (var node in nodes)
                this.Map.Children.Add(node.Pin);
        }

        /// <summary>
        /// Gets the main view model.
        /// </summary>
        public MainViewModel ViewModel => this.DataContext as MainViewModel;
    }
}
