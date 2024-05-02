using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReaction : MonoBehaviour
{

    public AudioSource _audioSource;
    public static float[] _spectrum = new float[512];


    // Start is called before the first frame update
    void Start()
    {

        if (_audioSource != null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetMusicAudioSource();

    }

    void GetMusicAudioSource()
    {
        _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);

    }


}

