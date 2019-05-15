// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminWindowViewModel.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the AdminWindowViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.ViewModel.Admin
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using BusinessLogicWPF.DesignAndValidation;
    using BusinessLogicWPF.Properties;
    using BusinessLogicWPF.View.Admin.UserControls;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// The admin window view model.
    /// </summary>
    public class AdminWindowViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWindowViewModel"/> class.
        /// </summary>
        /// <param name="snackBarMessageQueue">
        /// The snack bar message queue.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown if Argument is null
        /// </exception>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed. Suppression is OK here.")]
        public AdminWindowViewModel([NotNull] ISnackbarMessageQueue snackBarMessageQueue)
        {
            if (snackBarMessageQueue == null)
            {
                throw new ArgumentNullException(nameof(snackBarMessageQueue));
            }

            this.DemoItems = new[]
                                 {
                                     new DemoItem("Home", new AdminHome()),
                                     new DemoItem("Add Train", new AddTrain { DataContext = new AddTrainViewModel() }),
                                     new DemoItem(
                                         "Add Station",
                                         new AddStation { DataContext = new AddStationViewModel() }),
                                     new DemoItem("Add TTE", new AddTte { DataContext = new AddTteViewModel() }),
                                     new DemoItem(
                                         "Add Station Master",
                                         new AddStationMaster { DataContext = new AddStationMasterViewModel() })
                                 };
        }

        /// <summary>
        /// Gets or sets the demo items.
        /// </summary>
        [NotNull]
        public DemoItem[] DemoItems { get; set; }
    }
}
