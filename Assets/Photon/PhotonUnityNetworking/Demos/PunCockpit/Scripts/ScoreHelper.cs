<<<<<<< HEAD
﻿// <copyright file="ScoreHelper.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using Photon.Pun.UtilityScripts;


namespace Photon.Pun.Demo.Cockpit
{

    public class ScoreHelper : MonoBehaviour
    {
        public int Score;

        int _currentScore;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


            if (PhotonNetwork.LocalPlayer != null && Score != _currentScore)
            {
                _currentScore = Score;
                PhotonNetwork.LocalPlayer.SetScore(Score);
            }

        }
    }

=======
﻿// <copyright file="ScoreHelper.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using Photon.Pun.UtilityScripts;


namespace Photon.Pun.Demo.Cockpit
{

    public class ScoreHelper : MonoBehaviour
    {
        public int Score;

        int _currentScore;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


            if (PhotonNetwork.LocalPlayer != null && Score != _currentScore)
            {
                _currentScore = Score;
                PhotonNetwork.LocalPlayer.SetScore(Score);
            }

        }
    }

>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}