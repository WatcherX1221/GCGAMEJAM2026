using UnityEngine;

public class AtomVisualController : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite(0);
    }

    // Update is called once per frame
    public void UpdateSprite(int chargeValue)
    {
        switch(chargeValue)
        {
            case 0:
                sr.sprite = sprites[3];
                break;
            case 1:
                sr.sprite = sprites[4];
                break;
            case 2:
                sr.sprite = sprites[5];
                break;
            case 3:
                sr.sprite = sprites[6];
                break;
            case -1:
                sr.sprite = sprites[2];
                break;
            case -2:
                sr.sprite = sprites[1];
                break;
            case -3:
                sr.sprite = sprites[0];
                break;
        }
    }
}
