// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStates.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the ConnectionStates type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.Helper
{
    using System;

    /// <summary>
    /// The connection states.
    /// </summary>
    [Flags]
    public enum ConnectionStates
    {
        /// <summary>
        /// Unknown state.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Local system uses a modem to connect to the Internet.
        /// </summary>
        Modem = 0x1,

        /// <summary>
        /// Local system uses a local area network to connect to the Internet.
        /// </summary>
        Lan = 0x2,

        /// <summary>
        /// Local system uses a proxy server to connect to the Internet.
        /// </summary>
        Proxy = 0x4,

        /// <summary>
        /// Local system has RAS (Remote Access Services) installed.
        /// </summary>
        RasInstalled = 0x10,

        /// <summary>
        /// Local system is in offline mode.
        /// </summary>
        Offline = 0x20,

        /// <summary>
        /// Local system has a valid connection to the Internet, but it might or might not be currently connected.
        /// </summary>
        Configured = 0x40,
    }
}