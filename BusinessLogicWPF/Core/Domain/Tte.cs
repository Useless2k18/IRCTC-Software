﻿// --------------------------------------------------------------------------------------------------------------------
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
        /// Gets or sets the email id.
        /// </summary>
        [FirestoreProperty("emailId")]
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [FirestoreProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [FirestoreProperty("password")]
        public string Password { get; set; }

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
