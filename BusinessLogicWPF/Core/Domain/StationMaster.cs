// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StationMaster.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the StationMaster type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.Core.Domain
{
    using Google.Cloud.Firestore;

    /// <summary>
    /// The station master.
    /// </summary>
    [FirestoreData]
    public class StationMaster
    {
        /// <summary>
        /// Gets or sets the TTE id.
        /// </summary>
        [FirestoreProperty("empId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [FirestoreProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the zone.
        /// </summary>
        [FirestoreProperty("zone")]
        public string Zone { get; set; }

        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        [FirestoreProperty("division")]
        public string Division { get; set; }
    }
}
