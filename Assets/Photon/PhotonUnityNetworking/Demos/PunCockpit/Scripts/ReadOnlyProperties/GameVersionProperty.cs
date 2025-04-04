<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoomListView.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
	/// <summary>
	/// PhotonNetwork.GameVersion UI property.
	/// </summary>
	public class GameVersionProperty : MonoBehaviour 
    {
		public Text Text;

		string _cache;

		void Update()
		{
			if (PhotonNetwork.GameVersion != _cache) {
				_cache = PhotonNetwork.GameVersion;
				Text.text = _cache;
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

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
	/// <summary>
	/// PhotonNetwork.GameVersion UI property.
	/// </summary>
	public class GameVersionProperty : MonoBehaviour 
    {
		public Text Text;

		string _cache;

		void Update()
		{
			if (PhotonNetwork.GameVersion != _cache) {
				_cache = PhotonNetwork.GameVersion;
				Text.text = _cache;
			}
		}
	}
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}