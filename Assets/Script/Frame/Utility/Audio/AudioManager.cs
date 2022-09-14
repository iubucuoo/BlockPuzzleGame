using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WUtils.Utils
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public void Init()
        {
            method_0();
        }

        private void OnApplicationPause(bool pause)
        {
            bool_0 = pause;
        }

        private void LateUpdate()
        {
            if (!bool_0)
            {
                for (int i = 0; i < list_0.Count; i++)
                {
                    list_0[i].Update();
                }
            }
        }

        public AudioGroup CreateGroup(int id, string name = null)
        {
            AudioGroup audioGroup = new AudioGroup(id, name);
            list_0.Add(audioGroup);
            return audioGroup;
        }

        public AudioGroup GetGroup(int groupID)
        {
            for (int i = 0; i < list_0.Count; i++)
            {
                if (list_0[i].id == groupID)
                {
                    return list_0[i];
                }
            }
            return CreateGroup(groupID, null);
        }
        public AudioObject Play(int groupID, string audioClip, bool loop = false, float volumn = 1f, GameObject target = null, float fadeIn = 0f, float delay = 0f)
        {
            AudioGroup group = GetGroup(groupID);
            AudioObject result;
            if (group != null)
            {
                result = group.Play(audioClip, loop, volumn, target, fadeIn, delay);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public AudioObject Play(int groupID, AudioClip audioClip, bool loop = false, float volumn = 1f, GameObject target = null, float fadeIn = 0f, float delay = 0f)
        {
            AudioGroup group = GetGroup(groupID);
            AudioObject result;
            if (group != null)
            {
                result = group.Play(audioClip, loop, volumn, target, fadeIn, delay);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public AudioObject PlaySource(int groupID, string audioSource, GameObject target = null, float fadeIn = 0f, float delay = 0f)
        {
            AudioGroup group = GetGroup(groupID);
            AudioObject result;
            if (group != null)
            {
                result = group.PlaySource(audioSource, target, fadeIn, delay);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public void StopAll(int groupID)
        {
            AudioGroup group = GetGroup(groupID);
            if (group != null)
            {
                group.StopAll();
            }
        }

        public float GetVolume(int groupID)
        {
            AudioGroup group = GetGroup(groupID);
            return (group != null) ? group.volumn : 0f;
        }

        public void SetVolume(int groupID, float volume)
        {
            AudioGroup group = GetGroup(groupID);
            if (group != null)
            {
                group.volumn = volume;
            }
        }

        private void method_0()
        {
            GameObject gameObject = new GameObject("AudioListener");
            gameObject.AddComponent<AudioListener>();
            transform_0 = gameObject.transform;
            AttachAudioListener(null, Vector3.zero);
        }

        public void AttachAudioListener(Transform target, Vector3 pos)
        {
            transform_0.SetParent(target ? target : base.transform, false);
            transform_0.localPosition = pos;
        }
        private List<AudioGroup> list_0 = new List<AudioGroup>();
        private bool bool_0 = false;
        private Transform transform_0;
    }
}