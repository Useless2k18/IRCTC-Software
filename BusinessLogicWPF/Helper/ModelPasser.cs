// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelPasser.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the ModelPasser type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.Helper
{
    using BusinessLogicWPF.Core.Domain;

    /// <summary>
    /// The model passer.
    /// </summary>
    public static class ModelPasser
    {
        /// <summary>
        /// The station master.
        /// </summary>
        private static StationMaster stationMaster;

        /// <summary>
        /// The TTE.
        /// </summary>
        private static Tte tte;

        /// <summary>
        /// Gets or sets the station master.
        /// </summary>
        public static StationMaster StationMaster
        {
            get => stationMaster;
            set
            {
                if (value != null && stationMaster == value)
                {
                    return;
                }

                stationMaster = value;
            }
        }

        /// <summary>
        /// Gets or sets the TTE.
        /// </summary>
        public static Tte Tte
        {
            get => tte;
            set
            {
                if (value != null && tte == value)
                {
                    return;
                }

                tte = value;
            }
        }
    }
}
