using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private float pitchRangeLow;
    [SerializeField] private float pitchRangeHigh;

    [SerializeField] private AudioClip attack1;
    [SerializeField] private AudioClip attack2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //play music when this object loads
        musicPlayer.Play();
    }

    public void PlayAttackSFX()
    {
        int randomSFX = Random.Range(1, 3);

        if(randomSFX == 1)
        {
            sfxPlayer.clip = attack1;
        }
        else
        {
            sfxPlayer.clip = attack2;
        }

            //randomizing the pitch of the sound each time it's played
            float randomPitch = Random.Range(pitchRangeLow, pitchRangeHigh);
        sfxPlayer.pitch = randomPitch;

        //play this sound effect when it is called
        sfxPlayer.Play();
    }
    
}
