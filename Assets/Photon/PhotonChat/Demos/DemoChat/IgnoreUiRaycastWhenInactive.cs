<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;


namespace Photon.Chat.Demo
{
    public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return this.gameObject.activeInHierarchy;
        }
    }
=======
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;


namespace Photon.Chat.Demo
{
    public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return this.gameObject.activeInHierarchy;
        }
    }
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}