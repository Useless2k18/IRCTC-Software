// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StationsList.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the StationsList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.Model.Json.Creation
{
    using System.Collections.Generic;

    using BusinessLogicWPF.Core.Domain;

    /// <summary>
    /// The stations list.
    /// </summary>
    public class StationsList
    {
        /// <summary>
        /// Gets or sets the stations.
        /// </summary>
        public IDictionary<string, Station> Stations { get; set; }
    }
}
