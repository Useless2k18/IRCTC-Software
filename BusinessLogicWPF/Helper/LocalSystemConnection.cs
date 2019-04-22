// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalSystemConnection.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the LocalSystemConnection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.Helper
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The local system connection.
    /// </summary>
    public static class LocalSystemConnection
    {
        /// <summary>
        /// The internet get connected state.
        /// </summary>
        /// <param name="lpdwFlags">
        /// The LPDW flags.
        /// </param>
        /// <param name="dwReserved">
        /// The DW reserved.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        [DllImport("wininet.dll", SetLastError = true, CallingConvention = CallingConvention.ThisCall)]
        public static extern bool InternetGetConnectedState(out ConnectionStates lpdwFlags, long dwReserved);

        /// <summary>
        /// Retrieves the connected state of the local system.
        /// </summary>
        /// <param name="connectionStates">A <see cref="ConnectionStates"/> value that receives the connection description.</param>
        /// <returns>
        /// A return value of true indicates that either the modem connection is active,
        /// or a LAN connection is active and a proxy is properly configured for the LAN.
        /// A return value of false indicates that neither the modem nor the LAN is connected.
        /// If false is returned, the <see cref="ConnectionStates.Configured"/> flag may be set
        /// to indicate that auto-dial is configured to "always dial" but is not currently active.
        /// If auto-dial is not configured, the function returns false.
        /// </returns>
        public static bool IsConnectedToInternet(out ConnectionStates connectionStates)
        {
            connectionStates = ConnectionStates.Unknown;
            return InternetGetConnectedState(out connectionStates, 0);
        }

        /// <summary>
        /// Retrieves the connected state of the local system.
        /// </summary>
        /// <returns>
        /// A return value of true indicates that either the modem connection is active,
        /// or a LAN connection is active and a proxy is properly configured for the LAN.
        /// A return value of false indicates that neither the modem nor the LAN is connected.
        /// If false is returned, the <see cref="ConnectionStates.Configured"/> flag may be set
        /// to indicate that auto-dial is configured to "always dial" but is not currently active.
        /// If auto-dial is not configured, the function returns false.
        /// </returns>
        public static bool IsConnectedToInternet()
        {
            return IsConnectedToInternet(out _);
        }
    }
}