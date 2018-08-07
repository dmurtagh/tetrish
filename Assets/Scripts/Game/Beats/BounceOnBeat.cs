using UnityEngine;

public class BounceOnBeat : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2d;
    MusicController m_MusicController;
    public Vector2 m_JumpForceAddition = new Vector2(0, 0);
    public float m_JumpForceMultiplier = 1f;

    // Use this for initialization
    void Start ()
    {
        //Select the instance of AudioProcessor and pass a reference
        //to this object
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);

        m_Rigidbody2d = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        m_MusicController = FindObjectOfType<MusicController>();
    }

    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    void onOnbeatDetected()
    {
        m_Rigidbody2d.velocity = new Vector2(m_Rigidbody2d.velocity.x, 0f); // Zero out the y velocity so the jump has equal effect, whether we're stationary, jumping or falling

        Vector2 force = m_MusicController.GetCurrentAudioTrack().bounceForce;
        force += m_JumpForceAddition;
        force *= m_JumpForceMultiplier;

        m_Rigidbody2d.AddForce(force);
    }

    //This event will be called every frame while music is playing
    void onSpectrum(float[] spectrum)
    {
    }
}
