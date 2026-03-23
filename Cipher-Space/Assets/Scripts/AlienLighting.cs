using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AlienLighting : MonoBehaviour
{
    public SpriteRenderer alienSpriteRender;
    public GameObject lighting;
    private Transform lightingTransform;
    private Light2D globalAlienLight;
    private GameObject prevLighting = null;

    private void Start()
    {
        lightingTransform = lighting.GetComponent<Transform>();
        globalAlienLight = lightingTransform.GetChild(0).gameObject.GetComponent<Light2D>();
    }

    void Update()
    {
        if (prevLighting != null)
        {
            prevLighting.SetActive(false);
        }

        // Lighting for if alien is facing to the right
        if (alienSpriteRender.sprite.name == "Alien-Idle_0" || alienSpriteRender.sprite.name == "Alien-Idle_1" || alienSpriteRender.sprite.name == "Alien-Idle_2" || alienSpriteRender.sprite.name == "Alien-Idle_3" || alienSpriteRender.sprite.name == "Alien-Idle_4")
        {
            prevLighting = lightingTransform.GetChild(1).gameObject;
            prevLighting.SetActive(true);
        }
        // Lighting for if alien is facing to the front
        else if (alienSpriteRender.sprite.name == "Alien-MoveFront_0" || alienSpriteRender.sprite.name == "Alien-MoveFront_1" || alienSpriteRender.sprite.name == "Alien-MoveFront_2")
        {
            prevLighting = lightingTransform.GetChild(2).gameObject;
            prevLighting.SetActive(true);
        }
        // Lighting for if alien is facing to the left
        else if ((alienSpriteRender.sprite.name == "Alien-Idle_0" || alienSpriteRender.sprite.name == "Alien-Idle_1" || alienSpriteRender.sprite.name == "Alien-Idle_2" || alienSpriteRender.sprite.name == "Alien-Idle_3" || alienSpriteRender.sprite.name == "Alien-Idle_4") && alienSpriteRender.flipX == true)
        {
            prevLighting = lightingTransform.GetChild(2).gameObject;
            prevLighting.SetActive(true);
        }
        else
        {
            globalAlienLight.intensity = 0.001f;
            globalAlienLight.color = new Color(255, 255, 255);
        }
    }
}