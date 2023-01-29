using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveLightning : MonoBehaviour
{
    public int LightningLevel = 0;

    public float SnapRadius = 1.00f;
    public int InitialDamage = 1;
    public Material Lightning;
    public Transform snapTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SphereCollider>().radius = SnapRadius;
        LightningLevel = GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().CurrentMagicLVL;
        Debug.Log(LightningLevel);
        if (snapTarget != null)
        {
            transform.LookAt(snapTarget.transform.position);
        }
        if(LightningLevel == 0)
        {
            SnapRadius = 1f;
            InitialDamage = 1;
            if(GetComponent<ProjectileMotion>() == true)
            {
                GetComponent<ProjectileMotion>().speed = 10;
            }

        }
        if(LightningLevel == 1)
        {
            SnapRadius = 1.1f;
            InitialDamage = 2;
            if (GetComponent<ProjectileMotion>() == true)
            {
                GetComponent<ProjectileMotion>().speed = 15;
            }
        }
        if (LightningLevel == 2)
        {
            SnapRadius = 1.15f;
            InitialDamage = 3;
            if (GetComponent<ProjectileMotion>() == true)
            {
                GetComponent<ProjectileMotion>().speed = 20;
            }
        }
        if(LightningLevel == 3)
        {
            SnapRadius = 1.2f;
            InitialDamage = 4;
            if (GetComponent<ProjectileMotion>() == true)
            {
                GetComponent<ProjectileMotion>().speed = 25;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            snapTarget = null;
            collision.gameObject.AddComponent<MSTLightning>();
            collision.gameObject.GetComponent<MSTLightning>().LightningLVL = LightningLevel;
            collision.gameObject.GetComponent<MSTLightning>().material = Lightning;
            collision.gameObject.GetComponent<EnemyStats>().currentHealth -= InitialDamage;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && snapTarget == null)
        {
            snapTarget = other.transform;

        }
    }
}
