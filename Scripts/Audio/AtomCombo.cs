using UnityEngine;

public class AtomCombo : MonoBehaviour
{
    public int subatomCombo;
    public AudioClip atomHitAudio;

    public void PlayComboAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.pitch = (subatomCombo / 10f) + 0.5f;
        if(audio.pitch > 2)
        {
            audio.pitch = 2;
        }
        audio.PlayOneShot(atomHitAudio);
        subatomCombo++;
    }
}
