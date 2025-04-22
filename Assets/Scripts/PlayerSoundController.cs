using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spellect.HealthController;
using static Spellect.SpellcastingController;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] AudioSource movementAudioSource;
    [SerializeField] AudioSource spellAudioSource;
    bool playingFootsteps = false;
    bool playingLeftFoot = false;

    public AudioClip spellSuccess;
    public AudioClip spellFail;

    public void StartFootsteps()
    {
        playingFootsteps = true;
        movementAudioSource.Play();
    }

    public void StopFootsteps()
    {
        playingFootsteps = false;
    }

    public void Update()
    {
        if (!movementAudioSource.isPlaying && playingFootsteps)
        {
            movementAudioSource.pitch = Random.RandomRange(0.9f, 1.1f);
            if (playingLeftFoot)
            {
                movementAudioSource.panStereo = 0.1f;
                playingLeftFoot = false;
            }
            else
            {
                movementAudioSource.panStereo = -0.1f;
                playingLeftFoot = true;
            }
            movementAudioSource.Play();
            
                
        }

        
    }
    public void PlaySpellSound()
    {
        spellAudioSource.clip = spellSuccess;
    }

    public void OnSpellCast(object o, SpellCastEventArgs e)
    {
        spellAudioSource.PlayOneShot(spellSuccess);
    }


}
