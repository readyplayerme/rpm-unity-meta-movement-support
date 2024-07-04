using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.MetaMovement.Samples
{
    public class LoadUrlOnStart : MonoBehaviour
    {
        [SerializeField]
        private string avatarUrl = "https://models.readyplayer.me/6686400a821532d96e544831.glb";
        public UnityEvent<string> OnStartEvent;

        private void Start()
        {
            OnStartEvent.Invoke(avatarUrl);
        }
    }
}