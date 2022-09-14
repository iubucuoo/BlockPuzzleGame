using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WUtils.Utils
{
    public enum SoundObjectState
    {
        Loading,
        Loaded,
        Playing
    }
    public class AudioGroup
    {
        public AudioGroup(int id, string name)
        {
            this.id = id;
            gameObject = new GameObject(string.IsNullOrEmpty(name) ? ("Group" + id.ToString()) : name);
            gameObject.transform.SetParent(AudioManager.Inst.transform, false);
        }

        public AudioObject Play(string audioClip, bool loop = false, float volumn = 1f, GameObject target = null, float fadeIn = 0f, float delay = 0f)
        {
            AudioObject audioObject = new AudioObject();
            audioObject.volumn = volumn;
            audioObject.audioSource = method_2(target ? target : gameObject);
            audioObject.audioSource.loop = loop;
            audioObject.audioSource.volume = volumn * volumn;
            audioObject.audioSource.spatialBlend = (float)(target ? 1 : 0);
            audioObject.audioClipRequest = null;// ResourceManager.LoadAssetAsync(audioClip, typeof(AudioClip), -1f);
            audioObject.coPlay = AudioManager.Inst.StartCoroutine(method_0(audioObject, fadeIn, delay));
            return audioObject;
        }

        public AudioObject Play(AudioClip audioClip, bool loop = false, float volumn = 1f, GameObject target = null, float fadeIn = 0f, float delay = 0f)
        {
            AudioObject audioObject = new AudioObject();
            audioObject.volumn = volumn;
            audioObject.audioSource = method_2(target ? target : gameObject);
            audioObject.audioSource.loop = loop;
            audioObject.audioSource.volume = volumn * volumn;
            audioObject.audioSource.spatialBlend = (float)(target ? 1 : 0);
            audioObject.audioSource.clip = audioClip;
            audioObject.coPlay = AudioManager.Inst.StartCoroutine(method_0(audioObject, fadeIn, delay));
            return audioObject;
        }

        public AudioObject PlaySource(string audioSource, GameObject target = null, float fadeIn = 0f, float delay = 0f)
        {
            AudioObject audioObject = new AudioObject();
            audioObject.audioSourceRequest = null;
            audioObject.coPlay = AudioManager.Inst.StartCoroutine(method_0(audioObject, fadeIn, delay));
            return audioObject;
        }

        private IEnumerator method_0(AudioObject audioObject_0, float float_0, float float_1)
        {
            audioObject_0.group = this;
            audioObject_0.startTime = Time.time + float_1;
            audioObject_0.fadeIn = float_0;
            audioObject_0.stopTime = 0f;
            audioObject_0.fadeOut = 0f;
            audioObject_0.state = SoundObjectState.Loading;
            audioObject_0.paused = false;
            m_Objects.Add(audioObject_0);
            if (audioObject_0.audioSourceRequest != null)
            {
                yield return audioObject_0.audioSourceRequest;
                if (audioObject_0.audioSourceRequest)
                {
    
                    Stop(audioObject_0, 0f);
                    yield break;
                }
                audioObject_0.audioSource = audioObject_0.audioSourceRequest.gameObject.GetComponent<AudioSource>();
                audioObject_0.volumn = audioObject_0.audioSource.volume;
                audioObject_0.audioSource.volume = audioObject_0.volumn * volumn;
            }
            if (audioObject_0.audioClipRequest != null)
            {
                yield return audioObject_0.audioClipRequest;
                if (audioObject_0.audioClipRequest == null)
                {
                    Debug.Log("audioClipRequest == null");
                    yield break;
                }
                if (audioObject_0.audioSourceRequest)
                {
                    Stop(audioObject_0, 0f);
                    yield break;
                }
                audioObject_0.audioSource.clip = null;
            }
            if (audioObject_0.paused)
            {
                audioObject_0.audioSource.Pause();
            }
            audioObject_0.state = SoundObjectState.Loaded;
            yield break;
        }

        public void Stop(AudioObject obj, float fadeOut = 0f)
        {
            if (fadeOut <= 0f)
            {
                m_Objects.Remove(obj);
                method_1(obj);
            }
            else
            {
                obj.stopTime = Time.time;
                obj.fadeOut = fadeOut;
            }
        }

        public void StopAll()
        {
            for (int i = 0; i < m_Objects.Count; i++)
            {
                method_1(m_Objects[i]);
            }
            m_Objects.Clear();
        }

        public void Pause(AudioObject obj)
        {
            if (!obj.paused)
            {
                obj.paused = true;
                if (obj.state == SoundObjectState.Playing && obj.audioSource)
                {
                    obj.audioSource.Pause();
                }
            }
        }

        public void UnPause(AudioObject obj)
        {
            if (obj.paused)
            {
                obj.paused = false;
                if (obj.state == SoundObjectState.Playing && obj.audioSource)
                {
                    obj.audioSource.UnPause();
                }
            }
        }

        public void Update()
        {
            float time = Time.time;
            for (int i = m_Objects.Count - 1; i >= 0; i--)
            {
                AudioObject audioObject = m_Objects[i];
                if (audioObject.state != SoundObjectState.Loading)
                {
                    if (!audioObject.audioSource)
                    {
                        Stop(audioObject, 0f);
                    }
                    else
                    {
                        if (audioObject.state == SoundObjectState.Loaded && audioObject.startTime <= time)
                        {
                            audioObject.state = SoundObjectState.Playing;
                            audioObject.startTime = time;
                            audioObject.audioSource.Play();
                        }
                        if (!audioObject.paused && audioObject.state == SoundObjectState.Playing)
                        {
                            if (!audioObject.audioSource.isPlaying)
                            {
                                audioObject.OnComplete();
                                Stop(audioObject, 0f);
                            }
                            else
                            {
                                float num = audioObject.volumn * volumn;
                                if (audioObject.fadeIn > 0f && time - audioObject.startTime < audioObject.fadeIn)
                                {
                                    float num2 = (time - audioObject.startTime) / audioObject.fadeIn;
                                    num *= num2;
                                }
                                if (audioObject.stopTime > 0f)
                                {
                                    if (time - audioObject.stopTime >= audioObject.fadeOut)
                                    {
                                        Stop(audioObject, 0f);
                                        goto IL_16E;
                                    }
                                    float num3 = (time - audioObject.stopTime) / audioObject.fadeOut;
                                    num *= 1f - num3;
                                }
                                audioObject.audioSource.volume = num;
                            }
                        }
                    }
                }
            IL_16E:;
            }
        }

        private void method_1(AudioObject audioObject_0)
        {
            if (audioObject_0.coPlay != null)
            {
                AudioManager.Inst.StopCoroutine(audioObject_0.coPlay);
                audioObject_0.coPlay = null;
            }
            if (audioObject_0.audioSource)
            {
                audioObject_0.audioSource.Stop();
            }
            audioObject_0.audioSource = null;
            if (audioObject_0.audioSourceRequest != null)
            {
                audioObject_0.audioSourceRequest = null;
                audioObject_0.audioSource = null;
            }
            else
            {
                method_3(audioObject_0.audioSource);
                audioObject_0.audioSource = null;
            }
            if (audioObject_0.audioClipRequest != null)
            {
                //audioObject_0.audioClipRequest.Dispose();
                audioObject_0.audioClipRequest = null;
            }
        }

        private AudioSource method_2(GameObject gameObject_0)
        {
            for (int i = 0; i < list_0.Count; i++)
            {
                AudioSource audioSource = list_0[i];
                if (!audioSource)
                {
                    list_0.RemoveAt(i);
                }
                else if (audioSource.gameObject == gameObject_0)
                {
                    list_0.RemoveAt(i);
                    return audioSource;
                }
            }
            return gameObject_0.AddComponent<AudioSource>();
        }

        private void method_3(AudioSource audioSource_0)
        {
            if (audioSource_0)
            {
                list_0.Add(audioSource_0);
            }
        }
        public int id;
        public float volumn = 1f;
        public GameObject gameObject;
        public List<AudioObject> m_Objects = new List<AudioObject>();
        private List<AudioSource> list_0 = new List<AudioSource>();
    }
}