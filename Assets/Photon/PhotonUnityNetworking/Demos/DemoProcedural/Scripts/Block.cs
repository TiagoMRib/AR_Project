<<<<<<< HEAD
﻿using UnityEngine;

namespace Photon.Pun.Demo.Procedural
{
    /// <summary>
    /// The Block component is attach to each instantiated Block at runtime.
    /// It provides the Block's ID as well as the parent's Cluster ID in order to apply modifications.
    /// </summary>
    public class Block : MonoBehaviour
    {
        public int BlockId { get; set; }
        public int ClusterId { get; set; }
    }
=======
﻿using UnityEngine;

namespace Photon.Pun.Demo.Procedural
{
    /// <summary>
    /// The Block component is attach to each instantiated Block at runtime.
    /// It provides the Block's ID as well as the parent's Cluster ID in order to apply modifications.
    /// </summary>
    public class Block : MonoBehaviour
    {
        public int BlockId { get; set; }
        public int ClusterId { get; set; }
    }
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}