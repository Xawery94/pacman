using UnityEngine;

public class Points : MonoBehaviour
{
    public float speed;

    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        speed -= 0.01f;
    }
}
