using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{

    private AudioSource audioSource;
    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();    
    }

    public void PlayAudio(string path, float volume = 1)
    {
        this.audioSource.clip = ResourceManager.Instance.GetAudio(path);
        this.audioSource.volume = volume;
        this.audioSource.Play();
        StartCoroutine(WaitToReturnToPool());
    }

    private IEnumerator WaitToReturnToPool()
    {
        if (this.audioSource.clip != null) {
            yield return new WaitForSeconds(this.audioSource.clip.length);
        } else {
            yield return null;
        }
        PoolManager.Instance.ReleaseObject(Env.AUDIO_SOURCE, this.gameObject);
    }


}
