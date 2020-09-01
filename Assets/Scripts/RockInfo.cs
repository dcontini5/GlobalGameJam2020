using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInfo : MonoBehaviour
{


    public float health;
    public static int rarity;
    public GameObject dustParticle;
    public float damageReceived;
    public float shake_decay = 0.2f;
    public float shake_intensity = .3f;



    private Vector3 originPosition;
    private Quaternion originRotation;
    private float temp_shake_intensity = 0;
    private ParticleSystem SUCC = null;

    public static GameObject HUD = null;
    public GameObject Player = null;

    private bool isBeingMined = false;

    public bool IsBeingMined
    {
        get
        {
            return isBeingMined;

        }
        set
        {
            isBeingMined = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        SUCC = GetComponentInChildren<ParticleSystem>();

        if(SUCC)
        {
            SUCC.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(isBeingMined)
        {
            DecreaseHealth();

            if(SUCC != null)
            {
                SUCC.Play();
            }
        }
        else
        {

            if (SUCC != null)
            {
                SUCC.Stop();
            }
        }
        if (temp_shake_intensity > 0)
        {
            transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
            transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);
            temp_shake_intensity -= shake_decay;
        }

    }

    void Shake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        temp_shake_intensity = shake_intensity;

    }

    void DecreaseHealth()
    {
        health += -damageReceived;
        if (health < 0.5f)
        {

            Instantiate(dustParticle, transform.position, transform.rotation);

            if(HUD != null)
            {
                if(HUD.GetComponentInChildren<PlayerHUDScript>() != null)
                {
                    HUD.GetComponentInChildren<PlayerHUDScript>().CollectDiamond(gameObject);
                }
            }
            if(Player != null)
            {
                if(Player.GetComponentInChildren<PlayerGemMining>())
                {
                    Player.GetComponentInChildren<PlayerGemMining>().RemoveFromList(gameObject);
                }
            }

            Destroy(gameObject);

        }
        transform.localScale *= 0.9f;
        Shake();
    }


}
