﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddStation.xaml.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Interaction logic for AddStations.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.View.Admin.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Threading;

    using BusinessLogicWPF.Core.Domain;
    using BusinessLogicWPF.Helper;
    using BusinessLogicWPF.Model;
    using BusinessLogicWPF.Properties;
    using BusinessLogicWPF.ViewModel.Admin;

    using Firebase.Auth;

    using Google.Cloud.Firestore;

    using MahApps.Metro.Controls;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for AddStations.XAML
    /// </summary>
    public partial class AddStation : UserControl
    {
        /// <summary>
        /// The regex.
        /// </summary>
        [NotNull]
        private static readonly Regex Regex = new Regex("[^0-9]+"); // regex that matches disallowed text

        /// <summary>
        /// The background worker.
        /// </summary>
        private readonly BackgroundWorker backgroundWorker;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BusinessLogicWPF.View.Admin.UserControls.AddStation" /> class.
        /// </summary>
        public AddStation()
        {
            this.InitializeComponent();

            this.DataContext = new AddStationViewModel();

            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += this.BackgroundWorkerDoWork;
            this.backgroundWorker.RunWorkerCompleted += this.BackgroundWorkerRunWorkerCompleted;

            try
            {
                this.backgroundWorker.RunWorkerAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// The is text allowed.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsTextAllowed([NotNull] string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return !Regex.IsMatch(text);
        }

        /// <summary>
        /// The background worker do work.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(
                () =>
                    {
                        if (DataHelper.ZoneAndDivisionModel != null && DataHelper.ZoneAndDivisionModel.ZoneList != null)
                        {
                            this.ComboBoxZoneName.ItemsSource = DataHelper.ZoneAndDivisionModel.ZoneList;
                        }
                    },
                DispatcherPriority.Normal);
        }

        /// <summary>
        /// The background worker run worker completed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ComboBoxZoneName.IsEnabled = true;
        }

        /// <summary>
        /// The combo box zone name on selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ComboBoxZoneNameOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ComboBoxZoneName.SelectedItem == null)
            {
                return;
            }
            
            var b = DataHelper.ZoneAndDivisionModel.DivisionList.TryGetValue(
                this.ComboBoxZoneName.SelectedItem.ToString(),
                out var divisionsList);

            if (b)
            {
                this.ComboBoxDivisionName.ItemsSource = divisionsList;
                this.ComboBoxDivisionName.IsEnabled = true;
            }
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
        /// The text OTP preview text input.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxPinCodePreviewTextInput([NotNull] object sender, [NotNull] TextCompositionEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            e.Handled = !IsTextAllowed(e.Text);
        }

        /// <summary>
        /// The text OTP on pasting.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxPinCodeOnPasting([NotNull] object sender, [NotNull] DataObjectPastingEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text ?? throw new InvalidOperationException()))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
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
        private void ButtonAcceptOnClick([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.ComboBoxZoneName.Text)
                || string.IsNullOrWhiteSpace(this.ComboBoxDivisionName.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxStationCode.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxStationName.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxPinCode.Text))
            {
                MessageBox.Show("Please fill up all the fields!");
                return;
            }

            if (this.TextBoxPinCode.Text.Length < 6)
            {
                MessageBox.Show("Pin Code must have SIX digits!");
                return;
            }
            
            var s = MessageBox.Show(
                "Are you sure you want to continue? You cannot undo this operation",
                "Question",
                MessageBoxButton.YesNo);
            
            if (s == MessageBoxResult.No)
            {
                return;
            }

            ButtonProgressAssist.SetIsIndicatorVisible(this.ButtonAccept, true);

            var station = new Station
                              {
                                  Zone = this.ComboBoxZoneName.Text,
                                  RailwayDivision = this.ComboBoxDivisionName.Text,
                                  StationCode = this.TextBoxStationCode.Text,
                                  StationName = this.TextBoxStationName.Text,
                                  StationPinCode = Convert.ToInt32(this.TextBoxPinCode.Text)
                              };

            var backgroundWorker2 = new BackgroundWorker();
            backgroundWorker2.DoWork += (o, args) =>
                {
                    var task = this.AddStationAsync(station);

                    Task.WaitAll(task);
                };

            backgroundWorker2.RunWorkerCompleted += (o, args) =>
                {
                    ButtonProgressAssist.SetIsIndicatorVisible(this.ButtonAccept, false);
                    MessageBox.Show("Data Successfully added");
                };

            try
            {
                backgroundWorker2.RunWorkerAsync();
                this.Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// The add station async.
        /// </summary>
        /// <param name="station">
        /// The station.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task AddStationAsync(Station station)
        {
            if (StaticDbContext.ConnectFireStore != null)
            {
                await StaticDbContext.ConnectFireStore?.AddOrUpdateCollectionDataAsync(
                    station,
                    "Root",
                    "Stations",
                    "StationDetails",
                    station.Zone,
                    station.RailwayDivision,
                    station.StationCode);

                var divisionDetails = StaticDbContext.ConnectFireStore?.GetCollectionFields(
                    "Root",
                    "Stations",
                    "StationDetails",
                    station.Zone);

                var listOfDivisions = (List<object>)divisionDetails?["divisions"];

                if (listOfDivisions?.Contains(station.RailwayDivision) == false)
                {
                    listOfDivisions.Add(station.RailwayDivision);
                    var noOfDivisions = Convert.ToInt32(divisionDetails["noOfDivision"]);
                    noOfDivisions++;
                    await StaticDbContext.ConnectFireStore?.AddOrUpdateCollectionDataAsync(
                        divisionDetails,
                        "Root",
                        "Stations",
                        "StationDetails",
                        station.Zone);

                    await StaticDbContext.ConnectFireStore?.AddOrUpdateCollectionDataAsync(
                        new Dictionary<string, int> { { "noOfDivision", noOfDivisions } },
                        "Root",
                        "Stations",
                        "StationDetails",
                        station.Zone);
                }
            }
        }

        /// <summary>
        /// The button reset on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonResetOnClick(object sender, RoutedEventArgs e)
        {
            this.Refresh();

            this.ComboBoxDivisionName.IsEnabled = false;
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        private void Refresh()
        {
            foreach (var textBox in this.FindChildren<TextBox>())
            {
                textBox.Clear();
            }

            foreach (var comboBox in this.FindChildren<ComboBox>())
            {
                comboBox.SelectedIndex = -1;
            }

            ErrorLabelHelper.Reset();
        }
    }
}
