using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, Vector2.zero) > 0.1f)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, 0f, Time.deltaTime * 2), Mathf.Lerp(transform.position.y, 0f, Time.deltaTime * 2), -10);
        }
        else
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }
    public void ShakeCamera()
    {
        Camera cam = GetComponent<Camera>();
        transform.position = new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f));
    }
}
