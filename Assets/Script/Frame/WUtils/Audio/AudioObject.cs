using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
namespace WUtils.Utils
{
    public class AudioObject  
    {
        public event Action<AudioObject> onComplete
        {
            [CompilerGenerated]
            add
            {
                Action<AudioObject> action = action_0;
                Action<AudioObject> action2;
                do
                {
                    action2 = action;
                    Action<AudioObject> value2 = (Action<AudioObject>)Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange<Action<AudioObject>>(ref action_0, value2, action2);
                }
                while (action != action2);
            }
            [CompilerGenerated]
            remove
            {
                Action<AudioObject> action = action_0;
                Action<AudioObject> action2;
                do
                {
                    action2 = action;
                    Action<AudioObject> value2 = (Action<AudioObject>)Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange<Action<AudioObject>>(ref action_0, value2, action2);
                }
                while (action != action2);
            }
        }

        public float time
        {
            get
            {
                return audioSource ? audioSource.time : 0f;
            }
        }
        public float normalizedTime
        {
            get
            {
                float result;
                if (!audioSource)
                {
                    result = 0f;
                }
                else
                {
                    AudioClip clip = audioSource.clip;
                    if (!clip)
                    {
                        result = 0f;
                    }
                    else
                    {
                        result = audioSource.time / clip.length;
                    }
                }
                return result;
            }
        }

        public void Stop(float fadeOut = 0f)
        {
            group.Stop(this, fadeOut);
        }

        public void Pause()
        {
            group.Pause(this);
        }

        public void UnPause()
        {
            group.UnPause(this);
        }

        internal void OnComplete()
        {
            action_0?.Invoke(this);
        }

        public AudioGroup group;
        public GameObject audioClipRequest;
        public GameObject audioSourceRequest;
        public AudioSource audioSource;
        public Coroutine coPlay;
        public float volumn;
        public float startTime;
        public float fadeIn;
        public float stopTime;
        public float fadeOut;
        public SoundObjectState state;
        public bool paused;
        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<AudioObject> action_0;
    }
}