// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddStationMaster.xaml.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Interaction logic for AddStationMaster.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.View.Admin.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using BusinessLogicWPF.Core.Domain;
    using BusinessLogicWPF.Helper;
    using BusinessLogicWPF.Model;
    using BusinessLogicWPF.Properties;

    using MahApps.Metro.Controls;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for AddStationMaster.XAML
    /// </summary>
    public partial class AddStationMaster : UserControl
    {
        /// <summary>
        /// The station master Id.
        /// </summary>
        private string stationMasterId;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AddStationMaster"/> class.
        /// </summary>
        public AddStationMaster()
        {
            this.InitializeComponent();
            
            if (DataHelper.ZoneAndDivisionModel != null && DataHelper.ZoneAndDivisionModel.ZoneList != null)
            {
                this.ComboBoxZoneName.ItemsSource = DataHelper.ZoneAndDivisionModel.ZoneList;
            }
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
        /// The button refresh on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonRefreshOnClick(object sender, RoutedEventArgs e)
        {
            this.Refresh();
            this.ComboBoxDivisionName.IsEnabled = false;
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
            if (string.IsNullOrWhiteSpace(this.TextBoxStationMasterName.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxStationMasterEmailId.Text)
                || string.IsNullOrWhiteSpace(this.PasswordBoxStationMasterPassword.Password)
                || string.IsNullOrWhiteSpace(this.ComboBoxZoneName.Text)
                || string.IsNullOrWhiteSpace(this.ComboBoxDivisionName.Text))
            {
                MessageBox.Show("Please fill up all the fields!");
                return;
            }

            if (!this.TextBoxStationMasterName.Text.Contains(" "))
            {
                if (MessageBox.Show(
                        $"Is \"{this.TextBoxStationMasterName.Text}\" Full Name of the Station Master?",
                        "Confirmation of Full Name",
                        MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            ButtonProgressAssist.SetIsIndicatorVisible(this.ButtonAccept, true);

            var stationMaster = new StationMaster
                                    {
                                        Name = this.TextBoxStationMasterName.Text,
                                        EmailId = this.TextBoxStationMasterEmailId.Text,
                                        Password = this.PasswordBoxStationMasterPassword.Password,
                                        Zone = this.ComboBoxZoneName.Text,
                                        Division = this.ComboBoxDivisionName.Text
                                    };

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (o, args) =>
                {
                    var task = this.AddStationMasterAsync(stationMaster);

                    Task.WaitAll(task);
                };
            backgroundWorker.RunWorkerCompleted += (o, args) =>
                {
                    MessageBox.Show("You have successfully added Station Master" + $"\nName: {this.TextBoxStationMasterName.Text}\nId: {this.stationMasterId}");
                    ButtonProgressAssist.SetIsIndicatorVisible(this.ButtonAccept, false);
                };

            try
            {
                backgroundWorker.RunWorkerAsync();
                this.Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// The add station master async.
        /// </summary>
        /// <param name="stationMaster">
        /// The station master.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task AddStationMasterAsync(StationMaster stationMaster)
        {
            var id = StaticDbContext.ConnectFireStore.GetAllDocumentId(
                "Root",
                "Employee",
                stationMaster.Zone,
                stationMaster.Division,
                "StationMaster");

            var max = 0;

            if (id.Count != 0)
            {
                max = Convert.ToInt32(id.OrderByDescending(i => int.Parse(i.Substring(8))).First().Substring(8));
            }

            this.stationMasterId = $"{(int)EnumEmployeeGroups.GroupB:D2}" + 
                                   $"{(int)EnumEmployeeType.StationMaster:D2}" + 
                                   $"{DataHelper.ZoneAndDivisionModel.ZoneList.IndexOf(stationMaster.Zone):D2}" + 
                                   $"{DataHelper.ZoneAndDivisionModel.DivisionList[stationMaster.Zone].IndexOf(stationMaster.Division):D2}" + 
                                   $"{(max + 1):D7}";

            stationMaster.Id = this.stationMasterId;

            var noOfStationMaster = 0;

            var divisionField = StaticDbContext.ConnectFireStore.GetCollectionFields(
                "Root",
                "Employee",
                stationMaster.Zone,
                stationMaster.Division);

            if (divisionField != null)
            {
                noOfStationMaster = Convert.ToInt32(divisionField["noOfStationMaster"]);
            }

            if (StaticDbContext.ConnectFireStore?.FindDocument(
                    stationMaster.Id,
                    "Root",
                    "Employee",
                    stationMaster.Zone,
                    stationMaster.Division,
                    "StationMaster") == false)
            {
                noOfStationMaster++;
            }

            await StaticDbContext.ConnectFireStore.AddOrUpdateCollectionDataAsync(
                new Dictionary<string, int> { { "noOfStationMaster", noOfStationMaster } },
                "Root",
                "Employee",
                stationMaster.Zone,
                stationMaster.Division);

            await StaticDbContext.ConnectFireStore.AddOrUpdateCollectionDataAsync(
                stationMaster,
                "Root",
                "Employee",
                stationMaster.Zone,
                stationMaster.Division,
                "StationMaster",
                stationMaster.Id);
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

            foreach (var passwordBox in this.FindChildren<PasswordBox>())
            {
                passwordBox.Clear();
            }

            foreach (var comboBox in this.FindChildren<ComboBox>())
            {
                comboBox.SelectedIndex = -1;
            }

            ErrorLabelHelper.Reset();
        }
    }
}
