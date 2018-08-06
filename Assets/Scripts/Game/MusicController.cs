using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : AResettable
{
    public AudioSource m_AudioSource;

    public override void Reset()
    {
        m_AudioSource.Play();
    }
}
