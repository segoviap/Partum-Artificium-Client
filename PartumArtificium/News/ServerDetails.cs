#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

namespace PartumArtificium.News
{
    /// <summary> </summary>
    public class ServerDetails
    {
        private int _uniqueId;
        private string _serverName;
        private string _serverStatus;

        /// <summary> Unique ID for Server</summary>
        public int UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }
        /// <summary> </summary>
        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }
        /// <summary> </summary>
        public string ServerStatus
        {
            get { return _serverStatus; }
            set { _serverStatus = value; }
        }
    }
}
