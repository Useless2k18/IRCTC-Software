// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddRouteDialog.xaml.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Interaction logic for AddRouteDialog.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.View.Admin.UserControls.ForHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using BusinessLogicWPF.Core.Domain;
    using BusinessLogicWPF.Helper;
    using BusinessLogicWPF.Properties;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for AddRouteDialog.XAML
    /// </summary>
    public partial class AddRouteDialog : UserControl
    {
        /// <summary>
        /// The list of stations.
        /// </summary>
        private readonly List<Station> listOfStations;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRouteDialog"/> class.
        /// </summary>
        public AddRouteDialog()
        {
            this.InitializeComponent();

            if (DataHelper.StationsList != null)
            {
                this.listOfStations = DataHelper.StationsList.Stations.Values.ToList();
            }

            if (DataHelper.Train != null)
            {
                var span = DataHelper.Train.Duration;
                var time = new TimeSpan(
                    int.Parse(span.Split(':')[0]), // hours
                    int.Parse(span.Split(':')[1]), // minutes
                    0); // seconds

                var listOfDays = new ObservableCollection<int>();

                for (var i = 1; i <= Math.Floor(time.TotalDays) + 1; i++)
                {
                    listOfDays.Add(i);
                }

                this.ComboBoxDayCount.ItemsSource = listOfDays;
            }
            
            // Just for deleting already selected station
            /*else if (DataHelper.PreviousSelectedStation != null)
            {
                this.listOfStations.Remove(DataHelper.PreviousSelectedStation);
            }*/

            this.ComboBoxStationCode.ItemsSource = this.listOfStations;
        }

        /// <summary>
        /// The framework element on request bring into view.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FrameworkElementOnRequestBringIntoView(
            [CanBeNull] object sender,
            [NotNull] RequestBringIntoViewEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Allows the keyboard to bring the items into view as expected:
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up))
            {
                return;
            }            

            e.Handled = true;  
        }

        /// <summary>
        /// The text box station code on lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ComboBoxStationCodeOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (this.ComboBoxStationCode.SelectedValue as string == DataHelper.Train.SourceStation)
            {
                this.TextBoxArrivalTime.Text = "SRC0";
            }
            else if (this.ComboBoxStationCode.SelectedValue as string == DataHelper.Train.DestinationStation)
            {
                this.TextBoxDepartureTime.Text = "DEST0";
            }
            else
            {
                this.TextBoxDepartureTime.Text = string.Empty;
                this.TextBoxArrivalTime.Text = string.Empty;
            }
        }

        /// <summary>
        /// The button accept on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonAcceptOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.ComboBoxStationCode.Text)
                || string.IsNullOrWhiteSpace(this.ComboBoxDayCount.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxDepartureTime.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxArrivalTime.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }
            
            if (!DateTime.TryParse(this.TextBoxDepartureTime.Text, out var _))
            {
                if (this.TextBoxDepartureTime.Text == "DEST0"
                    && this.ComboBoxStationCode.SelectedValue as string != DataHelper.Train.DestinationStation)
                {
                    MessageBox.Show("Only Destination Station can have Departure Time as DEST0");
                    return;
                }

                if (this.TextBoxDepartureTime.Text != "DEST0")
                {
                    MessageBox.Show("Please type proper Departure Time!");
                    return;
                }
            }
            
            if (!DateTime.TryParse(this.TextBoxArrivalTime.Text, out var _))
            {
                if (this.TextBoxArrivalTime.Text == "SRC0"
                    && this.ComboBoxStationCode.SelectedValue as string != DataHelper.Train.SourceStation)
                {
                    MessageBox.Show("Only Source Station can have Arrival Time as SRC0");
                    return;
                }

                if (this.TextBoxArrivalTime.Text != "SRC0")
                {
                    MessageBox.Show("Please type proper Arrival Time!");
                    return;
                }
            }

            var route = new Route
                            {
                                StationCode = this.ComboBoxStationCode.SelectedValue as string,
                                DayCount = this.ComboBoxDayCount.SelectedValue is int value ? value : 0,
                                DepartureTime = this.TextBoxDepartureTime.Text,
                                ArrivalTime = this.TextBoxArrivalTime.Text
                            };

            route.StationZone = DataHelper.StationsList
                .Stations[route.StationCode ?? throw new InvalidOperationException()].Zone;

            route.StationDivision = DataHelper.StationsList.Stations[route.StationCode].RailwayDivision;

            /*// Just for deleting already selected station
            DataHelper.PreviousSelectedStation = DataHelper.StationsList.Stations[route.StationCode ?? throw new InvalidOperationException()];*/

            DialogHost.CloseDialogCommand.Execute(route, null);
        }
    }
}
