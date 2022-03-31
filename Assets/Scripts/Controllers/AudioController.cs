using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    /// <summary>
    /// Sound effects, music, does not matter as you call the clip via its name.
    /// </summary>
    public AudioClip[] audioClips; //The audio clips to be added via UnityEditor
    private Dictionary<string, AudioClip> allClips; //A dictionary of all the clips that can be called during runtime.

    private AudioSource musicPlayer; //An audio source that plays only music.
    private List<AudioSource> soundPlayers; //A list of references of audio sources for sound effects.

    private void Awake()
    {
        allClips = new Dictionary<string, AudioClip>();
        
        foreach (AudioClip clip in audioClips)
        {
            if (!clip)
            {
                continue;
            }
            if (allClips.ContainsKey(clip.name))
            {
                throw new System.Exception("There are two clips with the name: '" + clip.name + "'");
            }
            allClips.Add(clip.name, clip);
        }
        soundPlayers = new List<AudioSource>();
        musicPlayer = gameObject.AddComponent<AudioSource>();
        musicPlayer.loop = true;
    }

    private void PlayAudio(AudioSource source, string clipName)
    {
        if (!allClips.ContainsKey(clipName))
        {
            Debug.Log("The audioclip '" + clipName + "' does not exist." );
            return;
        }

        if (source)
        {
            source.Stop();
            source.clip = allClips[clipName];
            source.Play();
        }
    }

    /// <summary>
    /// Plays an audio file. It automatically loops. Only one can be played at a time.
    /// </summary>
    /// <param name="clipName">The name of the music file.</param>
    public void PlayMusic(string clipName)
    {
        if (!musicPlayer.clip || !musicPlayer.clip.name.Equals(clipName) || !musicPlayer.isPlaying)
        {
            PlayAudio(musicPlayer, clipName);
        }
    }

    /// <summary>
    /// Plays a audio file. It does not loop. Multiple can be played at a time.
    /// </summary>
    /// <param name="clipName">The name of the music file.</param>
    public void PlaySound(string clipName)
    {
        AudioSource player = null;
        foreach (AudioSource source in soundPlayers)
        {
            if (source && !source.isPlaying)
            {
                player = source;
            }
        }

        if (player == null)
        {
            player = gameObject.AddComponent<AudioSource>();
            player.loop = false;
            soundPlayers.Add(player);
        }

        PlayAudio(player, clipName);
    }

    /// <summary>
    /// Stops the current music playing.
    /// </summary>
    public void StopMusic () {
        musicPlayer.Stop();
    }
}
