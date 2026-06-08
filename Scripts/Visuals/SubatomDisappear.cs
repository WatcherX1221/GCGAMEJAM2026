using UnityEngine;

public class SubatomDisappear : MonoBehaviour
{
    float effectTimer;
    bool wasTouched;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wasTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(wasTouched)
        {
            if (effectTimer > 0)
            {
                transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, effectTimer / 2);
                effectTimer -= Time.deltaTime;
            }
            else
            {
                 gameObject.SetActive(false);
            }
        }
    }

    public void DisappearEffect()
    {
        wasTouched = true;
        effectTimer = 1f;
        GetComponent<Collider2D>().enabled = false;
    }
}
