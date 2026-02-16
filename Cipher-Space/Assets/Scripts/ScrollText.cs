using System.Collections;
using UnityEngine;

public class ScrollText : MonoBehaviour
{
    public float scrollTextSpeed = 0.1f;

    private GameObject text;
    private Transform textTransform;
    private bool startScrolling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject;
        textTransform = text.GetComponent<Transform>();

        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        if (startScrolling)
        {
            textTransform.position = new Vector3(textTransform.position.x, textTransform.position.y + scrollTextSpeed, textTransform.position.z);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        startScrolling = true;
    }
}
