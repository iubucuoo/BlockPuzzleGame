using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Inst = null;

    public bool isPlaying_Sound = true;
    public bool isPlaying_Music = true;

    void Awake()
    {
        Inst = this;
    }


    public void PlayMusic(string music)
    {
        if (isPlaying_Music == false)
        {
            return;
        }
        if (AudioController.IsMusicPaused())
        {
            UnpauseMusic();
            return;
        }
        AudioController.PlayMusic(music, 1, .7f);
    }
    public void UnpauseMusic()
    {
        if (AudioController.IsMusicPaused())
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
        if (isPlaying_Sound == false)
        {
            return;
        }

        AudioController.Play(sound);
    }

    public void ButtonClick()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("click");
    }
    public void PlayGameOver()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("gameover");
    }
    public void PlayNewPrep()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("new");
    }
    public void PlayBoom()
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("sfx_combo_2");
    }
    public void PlayBoom(int lv)
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        if (lv<2)
        {
            Play("sfx_combo_2");
        }
        else if (lv<3)
        {
            Play("sfx_combo_3");
        }
        else if (lv < 4)
        {
            Play("sfx_combo_4");
        }
        else if (lv < 5)
        {
            Play("sfx_combo_5");
        }
        else
        {
            Play("sfx_combo_6");
        }
    }
    public void PlayEffectLevel(int level)
    {
        if (isPlaying_Sound == false)
        {
            return;
        }
        //一般 great,excellent
        //强度50% awesome,amazing
        //强度75% wonderful,fantastic,fabulous,marvelous
        //强度100% terrific,incredible
        if (level <= 1)
        {

        }
        else if (level ==2)
        {
            Play("effectGood");
        }
        else if (level == 3)
        {
            Play("effectGreat");
        }
        else if (level == 4)
        {
            Play("effectPerfect");
        }
        else if (level >= 5)
        {
            Play("effectAmazing");
        }
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
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("pick");
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
        if (isPlaying_Sound == false)
        {
            return;
        }
        Play("place");
    }
}
