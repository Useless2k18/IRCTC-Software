// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tte.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the Tte type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.Core.Domain
{
    using Google.Cloud.Firestore;

    /// <summary>
    /// The TTE.
    /// </summary>
    [FirestoreData]
    public class Tte
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
    }
}
