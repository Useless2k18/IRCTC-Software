// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddTte.xaml.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Interaction logic for AddTte.xaml
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

    using Google.Cloud.Firestore;

    using MahApps.Metro.Controls;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for AddTTE.XAML
    /// </summary>
    public partial class AddTte : UserControl
    {
        /// <summary>
        /// The TTE id.
        /// </summary>
        private string tteId = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTte"/> class.
        /// </summary>
        public AddTte()
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
            if (string.IsNullOrWhiteSpace(this.TextBoxTteName.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxTteEmailId.Text)
                || string.IsNullOrWhiteSpace(this.PasswordBoxTtePassword.Password)
                || string.IsNullOrWhiteSpace(this.ComboBoxZoneName.Text)
                || string.IsNullOrWhiteSpace(this.ComboBoxDivisionName.Text))
            {
                MessageBox.Show("Please fill up all the fields!");
                return;
            }

            if (!this.TextBoxTteName.Text.Contains(" "))
            {
                if (MessageBox.Show(
                        $"Is \"{this.TextBoxTteName.Text}\" Full Name of the TTE?",
                        "Confirmation of Full Name",
                        MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            ButtonProgressAssist.SetIsIndicatorVisible(this.ButtonAccept, true);

            var tte = new Tte
                          {
                              Name = this.TextBoxTteName.Text,
                              EmailId = this.TextBoxTteEmailId.Text,
                              Password = this.PasswordBoxTtePassword.Password,
                              Zone = this.ComboBoxZoneName.Text,
                              Division = this.ComboBoxDivisionName.Text
                          };
            
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (o, args) =>
                {
                    var task = this.AddTteAsync(tte);

                    Task.WaitAll(task);
                };
            backgroundWorker.RunWorkerCompleted += (o, args) =>
                {
                    MessageBox.Show("You have successfully added TTE" + $"\nName: {this.TextBoxTteName.Text}\nId: {this.tteId}");
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
        /// The Add TTE async.
        /// </summary>
        /// <param name="tte">
        /// The TTE
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task AddTteAsync(Tte tte)
        {
            var id = StaticDbContext.ConnectFireStore.GetAllDocumentId(
                "Root",
                "Employee",
                tte.Zone,
                tte.Division,
                "Tte");

            var max = 0;

            if (id.Count != 0)
            {
                max = Convert.ToInt32(id.OrderByDescending(i => int.Parse(i.Substring(8))).First().Substring(8));
            }

            this.tteId = $"{(int)EnumEmployeeGroups.GroupB:D2}" + 
                         $"{(int)EnumEmployeeType.Tte:D2}" + 
                         $"{DataHelper.ZoneAndDivisionModel.ZoneList.IndexOf(tte.Zone):D2}" + 
                         $"{DataHelper.ZoneAndDivisionModel.DivisionList[tte.Zone].IndexOf(tte.Division):D2}" + 
                         $"{max + 1:D7}";

            tte.Id = this.tteId;

            var noOfTte = 0;

            var divisionField =
                StaticDbContext.ConnectFireStore.GetCollectionFields("Root", "Employee", tte.Zone, tte.Division);

            if (divisionField != null)
            {
                noOfTte = Convert.ToInt32(divisionField["noOfTte"]);
            }

            if (StaticDbContext.ConnectFireStore?.FindDocument(
                    tte.Id,
                    "Root",
                    "Employee",
                    tte.Zone,
                    tte.Division,
                    "Tte") == false)
            {
                noOfTte++;
            }

            await StaticDbContext.ConnectFireStore.AddOrUpdateCollectionDataAsync(
                new Dictionary<string, int> { { "noOfTte", noOfTte } },
                "Root",
                "Employee",
                tte.Zone,
                tte.Division);

            await StaticDbContext.ConnectFireStore.AddOrUpdateCollectionDataAsync(
                tte,
                "Root",
                "Employee",
                tte.Zone,
                tte.Division,
                "Tte",
                tte.Id);
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

    [FirestoreData]
    public class City
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string State { get; set; }

        [FirestoreProperty]
        public string Country { get; set; }

        [FirestoreProperty("Capital")]
        public bool IsCapital { get; set; }

        [FirestoreProperty]
        public long Population { get; set; }
    }
}
