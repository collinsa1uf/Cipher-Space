using UnityEngine;

public class Rotate: MonoBehaviour
{
    public GameObject square;
    public GameObject triangle;
    void Start()
    {
        
    }

    void Update()
    {
        square.transform.Rotate(0, 0, 30 * Time.deltaTime);
        triangle.transform.Rotate(0, 0, -30 * Time.deltaTime);
    }
}
