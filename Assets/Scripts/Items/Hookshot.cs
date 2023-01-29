using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : ProjectileMotion
{
    [SerializeField] public int hookshotLevel = 1;
    [SerializeField, Range(0.1f, 100f)] float _pullSpeed = 10f;

    private GameManager.PlayerManager playerManager;
    /*
    public GameObject player; 
    public Transform startPoint;
    public Transform endPoint;
    */
    //[SerializeField] public float speed = 1.0f;

    //private float startTime;
    //private float travelLength;

    /*
    private void Start()
    {
        startPoint = _source.transform;
        startTime = Time.deltaTime;
        travelLength = Vector3.Distance(startPoint.position, endPoint.position);
    }
    */

    private bool _flag = false;

    private void OnCollisionEnter(Collision other)
    {

        playerManager = user.GetComponent<GameManager.PlayerManager>();

        if (_flag == false)
        {
            StartCoroutine(HookshotPull(other.collider));
        }
    }

    IEnumerator HookshotPull(Collider col)
    {
        if (hookshotLevel == 1)
        {
            playerManager.CurrentStamina -= playerManager.CurrentStamina * 0.5f;          // At level one, hookshot costs 50% stam
        }
        else if (hookshotLevel == 2)
        {
            playerManager.CurrentStamina -= playerManager.CurrentStamina * 0.25f;         // At level two, hookshot costs 25% stam
            _pullSpeed = 15f;                                                             // At level two, the speed at which the player moves toward the hit spot increases 50%
        }
        else if (hookshotLevel == 3)
        {
            playerManager.CurrentStamina -= playerManager.CurrentStamina * 0.25f;         // lvl 3 same as lvl 2 but can also pull enemies
            _pullSpeed = 15;
        }


        _flag = true;

        GetComponent<Collider>().enabled = false;

        Vector3 startPos = user.position;
        Vector3 endPos = col.transform.position;
        Vector3 direction = endPos - startPos;

        float distance = direction.magnitude;
        float t = 0;
        float duration = distance / _pullSpeed;

        speed = 0;

        direction.Normalize();

        while (t < duration)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") && hookshotLevel == 3)                                                          // If hookshot collides with enemy && hookshotLevel == 3,
            {
                t += Time.deltaTime;
                col.transform.position = Vector3.Lerp(col.transform.position, user.transform.position - direction * 1.5f, (t / duration) * 0.01f);  // 0.01 to lower the speed of interpolation
                yield return null;
            }
            else
            {


                t += Time.deltaTime;
                user.position = Vector3.Lerp(startPos, endPos - direction * 1.5f, t / duration);
                yield return null;

                Debug.Log(duration + "/" + t);
            }

        }

        Destroy(gameObject);
    }


}