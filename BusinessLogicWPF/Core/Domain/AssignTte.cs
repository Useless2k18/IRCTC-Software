// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssignTte.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the AssignTte type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.Core.Domain
{
    using System;

    using BusinessLogicWPF.Properties;

    using Google.Cloud.Firestore;

    /// <summary>
    /// The assign TTE
    /// </summary>
    [FirestoreData]
    public class AssignTte
    {
        /// <summary>
        /// Gets or sets the TTE.
        /// </summary>
        [CanBeNull]
        [FirestoreProperty("tteDetails")]
        public Tte TteDetails { get; set; }

        /// <summary>
        /// Gets or sets the source station.
        /// </summary>
        [FirestoreProperty("sourceStation")]
        public string SourceStation { get; set; }

        /// <summary>
        /// Gets or sets the source date time.
        /// </summary>
        [CanBeNull]
        [FirestoreProperty("sourceDateTime")]
        public string SourceDateTime { get; set; }

        /// <summary>
        /// Gets or sets the destination station.
        /// </summary>
        [FirestoreProperty("destinationStation")]
        public string DestinationStation { get; set; }

        /// <summary>
        /// Gets or sets the destination date time.
        /// </summary>
        [CanBeNull]
        [FirestoreProperty("destinationDateTime")]
        public string DestinationDateTime { get; set; }
    }
}
