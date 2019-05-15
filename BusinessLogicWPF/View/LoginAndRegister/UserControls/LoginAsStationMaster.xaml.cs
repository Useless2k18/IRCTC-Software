// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginAsStationMaster.xaml.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Interaction logic for LoginAsStationMaster.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
namespace BusinessLogicWPF.View.LoginAndRegister.UserControls
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using BusinessLogicWPF.Core.Domain;
    using BusinessLogicWPF.Helper;
    using BusinessLogicWPF.Properties;
    using BusinessLogicWPF.View.StationMaster.Window;

    using Window = System.Windows.Window;

    /// <summary>
    /// Interaction logic for Login As Station Master XAML
    /// </summary>
    public partial class LoginAsStationMaster : UserControl
    {
        /// <summary>
        /// The window.
        /// </summary>
        [NotNull]
        private readonly Window window;

        /// <summary>
        /// The station master.
        /// </summary>
        private StationMaster stationMaster;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginAsStationMaster"/> class.
        /// </summary>
        public LoginAsStationMaster()
        {
            this.InitializeComponent();
            this.window = Application.Current.MainWindow ?? throw new InvalidOperationException();

            var buttonBack = (Button)this.window?.FindName("ButtonBack");
            if (buttonBack != null)
            {
                buttonBack.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// The button login click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonLoginClick([NotNull] object sender, [NotNull] RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (this.stationMaster != null)
            {
                if (this.stationMaster.Password == this.TextPassword.Password)
                {
                    ModelPasser.StationMaster = this.stationMaster;
                    var stationMasterWindow = new StationMasterWindow();
                    this.window.Hide();
                    stationMasterWindow.ShowDialog();
                    this.window.Show();
                }
                else
                {
                    MessageBox.Show("Wrong Password");
                }
            }
            else
            {
                MessageBox.Show("Wrong details entered!");
            }
        }

        #region Input Fields

        /// <summary>
        /// The text user name on password changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextUserNameOnPasswordChanged([CanBeNull] object sender, [CanBeNull] TextChangedEventArgs e)
        {
            this.UsernameAlert.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// The text user name lost keyboard focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private async void TextUserNameLostKeyboardFocus(
            [NotNull] object sender,
            [NotNull] KeyboardFocusChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (this.TextUserName.Text.Length == 0)
            {
                return;
            }

            await Task.Factory.StartNew(
                () =>
                    {
                        string text = null;

                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal,
                            (ThreadStart)(() => text = this.TextUserName.Text));

                        if (Regex.IsMatch(text, "^0205"))
                        {
                            var zoneIndex = Convert.ToInt32(text.Substring(4, 2));
                            var divisionIndex = Convert.ToInt32(text.Substring(6, 2));

                            var zone = DataHelper.ZoneAndDivisionModel.ZoneList[zoneIndex];
                            var division = DataHelper.ZoneAndDivisionModel.DivisionList[zone][divisionIndex];

                            if (StaticDbContext.ConnectFireStore.FindDocument(
                                text,
                                "Root",
                                "Employee",
                                zone,
                                division,
                                "StationMaster"))
                            {
                                this.stationMaster =
                                    StaticDbContext.ConnectFireStore.GetCollectionFields<StationMaster>(
                                        "Root",
                                        "Employee",
                                        zone,
                                        division,
                                        "StationMaster",
                                        text);
                            }
                        }

                        Application.Current.Dispatcher.Invoke(() => { this.ButtonLogin.IsEnabled = true; });
                    }).ConfigureAwait(true);
        }

        /// <summary>
        /// The text password on lost keyboard focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextPasswordOnLostKeyboardFocus([CanBeNull] object sender, [CanBeNull] KeyboardFocusChangedEventArgs e)
        {
            if (this.TextPassword.Password.Length == 0)
            {
                this.LabelPasswordEmptyError.Foreground = new SolidColorBrush(Colors.OrangeRed);
                this.LabelPasswordEmptyError.Visibility = Visibility.Visible;
                this.TextPassword.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                this.ButtonLogin.IsEnabled = false;
            }
            else
            {
                this.LabelPasswordEmptyError.Visibility = Visibility.Hidden;
                this.TextPassword.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#89000000");
            }
        }

        /// <summary>
        /// The text password on password changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextPasswordOnPasswordChanged([NotNull] object sender, [NotNull] RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (this.TextPassword.Password.Length != 0)
            {
                this.LabelPasswordEmptyError.Visibility = Visibility.Hidden;
            }
            else
            {
                this.ButtonLogin.IsEnabled = false;
            }
        }

        /// <summary>
        /// The text password on preview mouse move.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown if Argument Null Exception
        /// </exception>
        private void TextPasswordOnPreviewMouseMove([NotNull] object sender, [NotNull] MouseEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (this.LabelPasswordEmptyError.Visibility == Visibility.Hidden)
            {
                this.TextPassword.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF673AB7");
            }
        }

        /// <summary>
        /// The text password on mouse leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown if Argument Null Exception
        /// </exception>
        private void TextPasswordOnMouseLeave([NotNull] object sender, [NotNull] MouseEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (this.LabelPasswordEmptyError.Visibility == Visibility.Hidden)
            {
                this.TextPassword.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#89000000");
            }
        }

        #endregion
    }
}
