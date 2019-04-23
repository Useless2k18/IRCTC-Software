// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnterCoaches.xaml.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Interaction logic for EnterCoaches.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.View.Admin.UserControls.ForHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;

    using BusinessLogicWPF.Helper;
    using BusinessLogicWPF.Properties;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for EnterCoaches.XAML
    /// </summary>
    public partial class EnterCoaches : UserControl
    {
        /// <summary>
        /// The counter.
        /// </summary>
        private int counter = 1;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BusinessLogicWPF.View.Admin.UserControls.ForHelpers.EnterCoaches" /> class.
        /// </summary>
        public EnterCoaches()
        {
            this.InitializeComponent();

            DataHelper.StatusForEnable = false;
        }

        /// <summary>
        /// The button delete on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonDeleteOnClick(object sender, RoutedEventArgs e)
        {
            var s = "TextBox" + this.counter;

            var findTextBox = ExtendedVisualTreeHelper.FindChild<TextBox>(this.StackPanel, s);
            this.StackPanel.Children.Remove(findTextBox);
            this.counter--;
        }

        /// <summary>
        /// The button add on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonAddOnClick([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
        {
            if (this.counter != 0)
            {
                var s = "TextBox" + this.counter;

                var findTextBox = ExtendedVisualTreeHelper.FindChild<TextBox>(this.StackPanel, s);

                if (string.IsNullOrWhiteSpace(findTextBox?.Text))
                {
                    MessageBox.Show("Please update previous text field(s)");
                    return;
                }
            }

            this.AddNewTextBox(true);

            // Update Static class DataHelper
            DataHelper.StatusForEnable = false;
        }

        private void AddNewTextBox(bool newFlag = false)
        {
            var textBox = new TextBox
                              {
                                  Name = "TextBox" + ++this.counter, 
                                  Margin = new Thickness(10, 10, 10, 10),
                                  CharacterCasing = CharacterCasing.Upper
                              };

            // Material Design Properties
            HintAssist.SetHint(textBox, "Enter Coach Number");
            HintAssist.SetIsFloating(textBox, true);

            if (newFlag)
            {
                if (this.counter != 0)
                {
                    var s = "TextBox" + (this.counter - 1);

                    var findOldTextBox = ExtendedVisualTreeHelper.FindChild<TextBox>(this.StackPanel, s);

                    if (string.IsNullOrWhiteSpace(findOldTextBox?.Text))
                    {
                        return;
                    }

                    var newTextBoxText = $"{new string(findOldTextBox.Text.TakeWhile(char.IsLetter).ToArray())}{this.counter}";
                    textBox.Text = newTextBoxText;

                    // var text = $"{new string(findOldTextBox.Text.Where(char.IsDigit).ToArray())}";
                    // var resultString = Regex.Match(findOldTextBox.Text, @"\d+").Value;
                    var resultString = string.Join(
                        " ",
                        Regex.Matches(findOldTextBox.Text, @"\d+").OfType<Match>().Select(m => m.Value));
                    var resultStringArray = resultString.Split(' ');
                }
            }

            // Update ScrollViewer
            this.ScrollViewer.ScrollToEnd();

            this.StackPanel.Children.Add(textBox);
        }

        /// <summary>
        /// The button success on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonSuccessOnClick([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
        {
            for (var i = 1; i <= this.counter; i++)
            {
                var textBox = ExtendedVisualTreeHelper.FindChild<TextBox>(this.StackPanel, "TextBox" + i);
                
                if (string.IsNullOrWhiteSpace(textBox?.Text))
                {
                    MessageBox.Show("Please fill up all the fields!");
                    DataHelper.StatusForEnable = false;
                    return;
                }
            }

            var s = MessageBox.Show(
                "Are you sure you want to continue? You cannot undo this operation",
                "Question",
                MessageBoxButton.YesNo);
            
            if (s == MessageBoxResult.No)
            {
                return;
            }

            var button = (Button)this.FindName("ButtonAdd");
            var button2 = (Button)this.FindName("ButtonSuccess");
            var button3 = (Button)this.FindName("ButtonDelete");

            DataHelper.CoachesList = new List<string>();

            for (var i = 1; i <= this.counter; i++)
            {
                var textBox = ExtendedVisualTreeHelper.FindChild<TextBox>(this.StackPanel, "TextBox" + i);

                if (textBox != null)
                {
                    DataHelper.CoachesList?.Add(textBox.Text);
                    textBox.IsEnabled = false;
                }
            }

            if (button != null && button2 != null && button3 != null)
            {
                button.Visibility = Visibility.Collapsed;
                button2.Visibility = Visibility.Collapsed;
                button3.Visibility = Visibility.Collapsed;
            }

            DataHelper.StatusForEnable = true;
        }
    }
}
