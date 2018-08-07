using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Audio/Track", order = 1)]
public class AudioTrack : ScriptableObject
{
    public string objectName = "New AudioTrack";
    public string trackName = "";
    public Vector3 bounceForce = new Vector3(0, 250, 0);
    public AudioClip audioClip;
}
