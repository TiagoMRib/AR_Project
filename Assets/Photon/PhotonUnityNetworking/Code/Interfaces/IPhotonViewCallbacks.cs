<<<<<<< HEAD
﻿namespace Photon.Pun
{
    using Photon.Realtime;

    /// <summary>
    /// Empty Base class for all PhotonView callbacks.
    /// </summary>
    public interface IPhotonViewCallback
    {

    }

    /// <summary>
    /// This interface defines a callback which fires prior to the PhotonNetwork destroying the PhotonView and Gameobject.
    /// </summary>
    public interface IOnPhotonViewPreNetDestroy : IPhotonViewCallback
    {
        /// <summary>
        /// This method is called before Destroy() is initiated for a networked object. 
        /// </summary>
        /// <param name="rootView"></param>
        void OnPreNetDestroy(PhotonView rootView);
    }

    /// <summary>
    /// This interface defines a callback for changes to the PhotonView's owner.
    /// </summary>
    public interface IOnPhotonViewOwnerChange : IPhotonViewCallback
    {
        /// <summary>
        /// This method will be called when the PhotonView's owner changes.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <param name="previousOwner"></param>
        void OnOwnerChange(Player newOwner, Player previousOwner);
    }

    /// <summary>
    /// This interface defines a callback for changes to the PhotonView's controller.
    /// </summary>
    public interface IOnPhotonViewControllerChange : IPhotonViewCallback
    {
        /// <summary>
        /// This method will be called when the PhotonView's controller changes.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <param name="previousOwner"></param>
        void OnControllerChange(Player newController, Player previousController);
    }
}
=======
﻿namespace Photon.Pun
{
    using Photon.Realtime;

    /// <summary>
    /// Empty Base class for all PhotonView callbacks.
    /// </summary>
    public interface IPhotonViewCallback
    {

    }

    /// <summary>
    /// This interface defines a callback which fires prior to the PhotonNetwork destroying the PhotonView and Gameobject.
    /// </summary>
    public interface IOnPhotonViewPreNetDestroy : IPhotonViewCallback
    {
        /// <summary>
        /// This method is called before Destroy() is initiated for a networked object. 
        /// </summary>
        /// <param name="rootView"></param>
        void OnPreNetDestroy(PhotonView rootView);
    }

    /// <summary>
    /// This interface defines a callback for changes to the PhotonView's owner.
    /// </summary>
    public interface IOnPhotonViewOwnerChange : IPhotonViewCallback
    {
        /// <summary>
        /// This method will be called when the PhotonView's owner changes.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <param name="previousOwner"></param>
        void OnOwnerChange(Player newOwner, Player previousOwner);
    }

    /// <summary>
    /// This interface defines a callback for changes to the PhotonView's controller.
    /// </summary>
    public interface IOnPhotonViewControllerChange : IPhotonViewCallback
    {
        /// <summary>
        /// This method will be called when the PhotonView's controller changes.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <param name="previousOwner"></param>
        void OnControllerChange(Player newController, Player previousController);
    }
}
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
