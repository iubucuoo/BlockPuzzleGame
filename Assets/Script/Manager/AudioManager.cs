using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public static AudioManager Inst=null;

	public bool isPlaying_Sound= true;
	public bool isPlaying_Music= true;

	void Awake()
	{
		Inst = this;
	}
    

	public void PlayMusic(string music)
	{
		if (isPlaying_Music == false) {
			return;
		}

		AudioController.PlayMusic (music,.7f,.7f);
	}
    public void UnpauseMusic()
    {
        AudioController.UnpauseMusic();
    }
    public void PauseMusic()
    {
        AudioController.PauseMusic();
    }
    public void StopMusic()
    {
        AudioController.StopMusic(.4f);
    }
    public void StopBGMusic()
    {
        StopMusic();
    }
    public void PlayBGMusic()
    {
        PlayMusic("UiMusic");
    }

    public void Play(string sound)
	{
		if (isPlaying_Sound == false) {
			return;
		}

		AudioController.Play (sound);
	}

	public void ButtonClick()
	{
		if (isPlaying_Sound == false) {
			return;
		}
		Play ("click");
	}
    public void PlayGameOver()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("gameover");
    }
    public void PlayBoom()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("sfx_combo_2");
    }
    public void PlayGameOpen()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("gameopen");
    }
    public void PlayNewRecord()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("newrecord");
    }
    public void PlayPick()
	{
		if (isPlaying_Sound == false) {
			return;
		}
		Play ("pick");
	}
    public void PlayReturn()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("return");
    }
    public void PlayPlace()
	{
		if (isPlaying_Sound == false) {
			return;
		}
		Play ("place");
	}
}
