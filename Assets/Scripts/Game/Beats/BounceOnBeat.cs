using UnityEngine;

public class BounceOnBeat : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2d;
    public float m_JumpForce = 100f;

    // Use this for initialization
    void Start ()
    {
        //Select the instance of AudioProcessor and pass a reference
        //to this object
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);

        m_Rigidbody2d = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
    }

    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    void onOnbeatDetected()
    {
        m_Rigidbody2d.velocity = new Vector2(m_Rigidbody2d.velocity.x, 0f); // Zero out the y velocity so the jump has equal effect, whether we're stationary, jumping or falling
        m_Rigidbody2d.AddForce(new Vector2(0, m_JumpForce));
    }

    //This event will be called every frame while music is playing
    void onSpectrum(float[] spectrum)
    {
    }
}
