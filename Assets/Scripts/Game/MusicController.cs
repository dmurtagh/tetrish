using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : AResettable
{
    public AudioSource m_AudioSource;

    public List<AudioTrack> m_AudioTracks;
    private int m_TrackIndex = 0;

    public void Start()
    {
        SetAudioTrack(0);
    }

    public override void Reset()
    {
        m_AudioSource.Play();
    }

    public AudioTrack GetCurrentAudioTrack()
    {
        return m_AudioTracks[m_TrackIndex];
    }

    private void SetAudioTrack(int index)
    {
        m_AudioSource.clip = m_AudioTracks[index].audioClip;
        m_AudioSource.Play();
    }

    void Update()
    {
        CheckForNextTrack();
    }

    private void CheckForNextTrack()
    {
        if (InputManager.Instance.GetNextTrackKeyDown())
        {
            m_TrackIndex = (m_TrackIndex + 1) % m_AudioTracks.Count;
            SetAudioTrack(m_TrackIndex);
            Debug.Log("m_TrackIndex = " + m_TrackIndex);
        }
    }
}
