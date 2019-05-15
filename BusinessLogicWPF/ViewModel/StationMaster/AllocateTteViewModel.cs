// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllocateTteViewModel.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the AllocateTteViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.ViewModel.StationMaster
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    using BusinessLogicWPF.Core.Domain;
    using BusinessLogicWPF.Helper;
    using BusinessLogicWPF.Properties;

    /// <summary>
    /// The allocate TTE View Model.
    /// </summary>
    public class AllocateTteViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllocateTteViewModel"/> class.
        /// </summary>
        public AllocateTteViewModel()
        {
            this.Items1 = CreateData();
            /*this.Items2 = CreateData();
            this.Items3 = CreateData();*/
        }

        /// <summary>
        /// The property changed of Allocate TTE View Model.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets The items 1.
        /// </summary>
        [CanBeNull]
        public ObservableCollection<Train> Items1 { get; }

        /// <summary>
        /// Gets the items 2.
        /// </summary>
        [CanBeNull]
        public ObservableCollection<Train> Items2 { get; }

        /// <summary>
        /// Gets the items 3.
        /// </summary>
        [CanBeNull]
        public ObservableCollection<Train> Items3 { get; }

        /// <summary>
        /// The create data.
        /// </summary>
        /// <returns>
        /// The <see cref="ObservableCollection{T}"/>.
        /// </returns>
        [CanBeNull]
        private static ObservableCollection<Train> CreateData()
        {
            var trains = new List<Train>();
            //SomeBackgroundTasks().ConfigureAwait(false);
            Task.Run(
                async () =>
                    {
                        var db = StaticDbContext.ConnectFireStore?.GetFirestoreDb();

                        var collections = db?.Collection("DynamicRoot/TrainDetails/RunningTrains");

                        var query = collections?.WhereEqualTo("tteAllocatedStatus", false);

                        var listener = query?.Listen(
                            snapshot2 => trains.AddRange(
                                snapshot2.Documents.Where(snapshotDocument => snapshotDocument.Exists)
                                    .Select(snapshotDocument => snapshotDocument.ConvertTo<Train>())));

                        var snapshot = await query?.GetSnapshotAsync();

                        trains.AddRange(
                            snapshot.Documents.Where(snapshotDocument => snapshotDocument.Exists)
                                .Select(snapshotDocument => snapshotDocument.ConvertTo<Train>()));
                    }).Wait();

            var observableCollectionOfTrain = new ObservableCollection<Train>(trains);

            return observableCollectionOfTrain;
        }

        /*private static async Task<List<Train>> SomeBackgroundTasks()
        {
            var db = StaticDbContext.ConnectFireStore?.GetFirestoreDb();

            var collections = db?.Collection("DynamicRoot/TrainDetails/RunningTrains");

            var query = collections?.WhereEqualTo("tteAllocatedStatus", false);
            var snapshot = await query?.GetSnapshotAsync();

            return snapshot?.Documents.Where(documentSnapshot => documentSnapshot.Exists)
                .Select(documentSnapshot => documentSnapshot.ConvertTo<Train>()).ToList();
        }*/

        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        [NotNull]
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() =>
            args => this.PropertyChanged?.Invoke(this, args);
    }
}
