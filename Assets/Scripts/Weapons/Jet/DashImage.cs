using UnityEngine;

public class DashImage : MonoBehaviour
{
    private float activeTime = 0.1f; //How long the dash image will be activate
    private float timeActivated; //Keep track of how long the dash image avtivated
    private float alpha; 
    private float alphaSet = 0.8f; //Alpha will be set to this variable when the dash image is enabled
    private float alphaMultiplier = 0.85f; //dash image fade overtime, the small this variable, the faster the image fade
    
    private Transform player; //Reference to Player 

    private SpriteRenderer sr;
    private SpriteRenderer playerSr;

    private Color color;

    //Called when enable, similar to start()
    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<Transform>();
        playerSr = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        sr.sprite = playerSr.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        sr.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            DashImagePool.Instance.AddToPool(gameObject);
        }
    }

}
