using UdonSharp;
using UnityEngine;

namespace KamigakureShrine
{
    /// <summary>
    /// Generic shrine object (suzu bell, omikuji box, purification altar).
    /// Set eventName to _RingBell, _DrawOmikuji, or _Purify.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ShrineInteractable : UdonSharpBehaviour
    {
        [SerializeField] private KamiShrineManager manager;
        [SerializeField] private string eventName = "_RingBell";
        [SerializeField] private AudioSource sfx;

        public override void Interact()
        {
            if (sfx != null) sfx.Play();
            manager.SendCustomEvent(eventName);
        }
    }
}
