using UdonSharp;
using UnityEngine;

namespace KamigakureShrine
{
    /// <summary>
    /// Place on each lesser kami statue (needs a collider).
    /// Interact is a rare event, so a single cross-behaviour call to the manager is fine.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class KamiStatue : UdonSharpBehaviour
    {
        [SerializeField] private KamiShrineManager manager;
        [SerializeField] private int kamiIndex;

        public override void Interact()
        {
            manager._OnStatueInteract(kamiIndex);
        }
    }
}
