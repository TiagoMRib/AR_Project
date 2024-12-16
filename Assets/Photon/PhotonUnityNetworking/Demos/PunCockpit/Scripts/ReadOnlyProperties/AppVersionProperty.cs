<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppVersionProperty.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    /// <summary>
    /// PhotonNetwork.AppVersion UI property.
    /// </summary>
    public class AppVersionProperty : MonoBehaviour
    {

        public Text Text;

        string _cache;

        void Update()
        {
            if (PhotonNetwork.AppVersion != _cache)
            {
                _cache = PhotonNetwork.AppVersion;
                Text.text = _cache;
            }
        }
    }
=======
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppVersionProperty.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    /// <summary>
    /// PhotonNetwork.AppVersion UI property.
    /// </summary>
    public class AppVersionProperty : MonoBehaviour
    {

        public Text Text;

        string _cache;

        void Update()
        {
            if (PhotonNetwork.AppVersion != _cache)
            {
                _cache = PhotonNetwork.AppVersion;
                Text.text = _cache;
            }
        }
    }
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}