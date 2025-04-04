<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundTimeOutField.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit Demo
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    /// <summary>
    /// PhotonNetwork.BackgroundTimeout UI InputField.
    /// </summary>
    public class BackgroundTimeOutField : MonoBehaviour
    {
        public InputField PropertyValueInput;

        float _cache;

        bool registered;

        void OnEnable()
        {
            if (!registered)
            {
                registered = true;
                PropertyValueInput.onEndEdit.AddListener(OnEndEdit);
            }
        }

        void OnDisable()
        {
            registered = false;
            PropertyValueInput.onEndEdit.RemoveListener(OnEndEdit);
        }

        void Update()
        {
            if (PhotonNetwork.KeepAliveInBackground != _cache)
            {
                _cache = PhotonNetwork.KeepAliveInBackground;
                PropertyValueInput.text = _cache.ToString("F1");
            }
        }

        // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartChat.
        public void OnEndEdit(string value)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Tab))
            {
                this.SubmitForm(value.Trim());
            }
            else
            {
                this.SubmitForm(value);
            }
        }

        public void SubmitForm(string value)
        {
            _cache = float.Parse(value);
            PhotonNetwork.KeepAliveInBackground = _cache;
            //Debug.Log("PhotonNetwork.BackgroundTimeout = " + PhotonNetwork.BackgroundTimeout, this);
        }
    }
=======
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundTimeOutField.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit Demo
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    /// <summary>
    /// PhotonNetwork.BackgroundTimeout UI InputField.
    /// </summary>
    public class BackgroundTimeOutField : MonoBehaviour
    {
        public InputField PropertyValueInput;

        float _cache;

        bool registered;

        void OnEnable()
        {
            if (!registered)
            {
                registered = true;
                PropertyValueInput.onEndEdit.AddListener(OnEndEdit);
            }
        }

        void OnDisable()
        {
            registered = false;
            PropertyValueInput.onEndEdit.RemoveListener(OnEndEdit);
        }

        void Update()
        {
            if (PhotonNetwork.KeepAliveInBackground != _cache)
            {
                _cache = PhotonNetwork.KeepAliveInBackground;
                PropertyValueInput.text = _cache.ToString("F1");
            }
        }

        // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartChat.
        public void OnEndEdit(string value)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Tab))
            {
                this.SubmitForm(value.Trim());
            }
            else
            {
                this.SubmitForm(value);
            }
        }

        public void SubmitForm(string value)
        {
            _cache = float.Parse(value);
            PhotonNetwork.KeepAliveInBackground = _cache;
            //Debug.Log("PhotonNetwork.BackgroundTimeout = " + PhotonNetwork.BackgroundTimeout, this);
        }
    }
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}