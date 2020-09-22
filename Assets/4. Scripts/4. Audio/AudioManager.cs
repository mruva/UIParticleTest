using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    #region Attributes
    [SerializeField]
    private AudioMixerSnapshot _gamePlayMusic;
    [SerializeField]
    private AudioMixerSnapshot _enemySnap;
    [SerializeField]
    private AudioMixerSnapshot _ambIdle;
    [SerializeField]
    private AudioMixerSnapshot _ambIN;
    [SerializeField]
    private AudioMixerSnapshot _redTrap;
    [SerializeField]
    private AudioMixerSnapshot _magneticTrap;
    [SerializeField]
    private AudioMixerSnapshot _idleTrap;
    [SerializeField]
    private AudioMixerSnapshot _menuMusic;
    [SerializeField]
    private AudioMixerSnapshot _gameIsOverMusic;
    /*
    [SerializeField]
    private AudioMixerSnapshot _eventSnap;
    */

    public Sound[] sounds;

    //Per passare una sola istanza di AudioManager tra le scene
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log(typeof(AudioManager).ToString() + "is NULL");
            return _instance;
        }

        private set
        {
            _instance = value;
        }

    }

    private bool isPlaying;

    #endregion

    // Awake is called before Start() that is the first frame update
    void Awake()
    {

        //se non esiste un-istanza di AudioManager la creo altrimenti no
        if (Instance == null)
        {
            _instance = this;
        } else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mix;
            s.source.spatialBlend = s.spatialBlend;
        }

        #region Gestione eventi

        //pensare a come gestire anche gli audio del gameplay nell'audio manager...forse fare un audiomanager specifico per gameplay?
        //creare degli eventi per l'audio!
        //WildUfoEvents.boolHit.AddListener(BoolHit);
        WildUfoEvents.audioEvent.AddListener(PlaySound); 
        WildUfoEvents.boolHit.AddListener(TransitionSound); 

        #endregion

    }


    #region Gestione metodi Eventi Listener

    private void PlaySound(AudioEventData soundData)
    {
        if (soundData.loop == false)
        {
            if (soundData.playAtTheSameTime == false)
            {
                Play(soundData.name);
            }
            else if (soundData.playAtTheSameTime == true)
            {
                PlayAtTheSameTime(soundData.name);
            }
        } else if (soundData.loop == true)
        {
            PlayLoop(soundData.name);
        }
        
    }

    private void TransitionSound(BoolEventData boolData)
    {
        switch (boolData.currentEvent)
        {
            case BoolEventData.BoolEvent.ambienteMusic:
                if (boolData.boolEvent == false)
                {
                    _ambIdle.TransitionTo(0.5f);
                } 
                else if (boolData.boolEvent == true)
                {
                    _ambIN.TransitionTo(0.3f);
                }
                break;
            case BoolEventData.BoolEvent.enemyZone:
                if (boolData.boolEvent == false)
                {
                    _gamePlayMusic.TransitionTo(0.75f);
                }
                else if (boolData.boolEvent == true)
                {
                    if (GameManager.GameIsOver)
                    {
                        _gameIsOverMusic.TransitionTo(0.3f);
                    } else
                    {
                        _enemySnap.TransitionTo(0.1f);
                    }
                    
                }
                break;
            case BoolEventData.BoolEvent.menuMusic:
                if (boolData.boolEvent == false)
                {
                    _gamePlayMusic.TransitionTo(0.75f);
                }
                else if (boolData.boolEvent == true)
                {
                    _menuMusic.TransitionTo(1f);
                }
                break;
            case BoolEventData.BoolEvent.gameIsOver:
                if (boolData.boolEvent == true)
                {
                    Debug.Log(GameManager.GameIsOver.ToString());
                    _gameIsOverMusic.TransitionTo(0.3f);
                }
                break;
            case BoolEventData.BoolEvent.endLevel:
                if (boolData.boolEvent == true)
                {
                    _gameIsOverMusic.TransitionTo(1f);
                    Play("WinJingle");
                }
                break;
            default:
                //Debug.LogError("Errore nessun caso di SUONO evento bool gestito");
                break;
        }
    }


    #endregion

   

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        s.source.Stop();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
        
    }

    public void PlayAtTheSameTime(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
            s.source.Play();
    }

    public void PlayLoop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        s.source.loop = true;
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void PlayOneShot(string name)
    {
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.PlayOneShot(s.source.clip);
        }else
        {
            return;
        }
            
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        s.source.volume = volume;
        s.source.Play();
    }

    public void PlayWithFadeOut(string name, float fadeoutTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        FadeOutMusic(name, fadeoutTime);

        s.source.Play();
    }

    
    public void FadeOutMusic(string song, float fadeOutTime)
    {
        StartCoroutine(FadeOut(song, fadeOutTime));
    }

    IEnumerator FadeOut(string _song, float _fadeOutTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _song);

        float t = 0f;
        float _vol = s.source.volume;
        for (t = 0; t < _fadeOutTime; t += Time.deltaTime)
        {
            s.source.volume = (_vol - (t / _fadeOutTime));
            yield return null;
        }
    }


    public void PlayWithFadeIn(string name, float fadeinTime, float setVolume)
    {
        if (!isPlaying)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + "not found");
                return;
            }
            FadeInMusic(s, fadeinTime, setVolume);
            isPlaying = true;
        }

    }


    public void FadeInMusic(Sound sound, float fadeInTime, float volumeToSet)
    {
        StartCoroutine(FadeIn(sound, fadeInTime, volumeToSet));
        sound.source.Play();
    }

    IEnumerator FadeIn(Sound _sound, float _fadeInTime, float _volume)
    {
        
        float t;
        for (t = 0; t < _fadeInTime; t += Time.deltaTime)
        {
            _sound.source.volume = 0;
            _sound.source.volume += (t /(_volume * 10 * _fadeInTime));
           yield return null;
         }
        //yield return new WaitForSeconds(_fadeInTime);
    }
}
