<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoomListView.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    /// <summary>
    /// PhotonNetwork.GetPing() UI property.
    /// </summary>
    public class PingProperty : PropertyListenerBase
    {
        public Text Text;

        int _cache = -1;

        void Update()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                if (PhotonNetwork.GetPing() != _cache)
                {
                    _cache = PhotonNetwork.GetPing();
                    Text.text = _cache.ToString() + " ms";
                    this.OnValueChanged();
                }
            }
            else
            {
                if (_cache != -1)
                {
                    _cache = -1;
                    Text.text = "n/a";
                }
            }
        }
    }
=======
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoomListView.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    /// <summary>
    /// PhotonNetwork.GetPing() UI property.
    /// </summary>
    public class PingProperty : PropertyListenerBase
    {
        public Text Text;

        int _cache = -1;

        void Update()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                if (PhotonNetwork.GetPing() != _cache)
                {
                    _cache = PhotonNetwork.GetPing();
                    Text.text = _cache.ToString() + " ms";
                    this.OnValueChanged();
                }
            }
            else
            {
                if (_cache != -1)
                {
                    _cache = -1;
                    Text.text = "n/a";
                }
            }
        }
    }
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}