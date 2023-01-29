using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastInteraction : MonoBehaviour
{
    public GameObject ObjectLookingAt;

    public TMPro.TMP_Text interactionPrompt;

    public float InteractionRadius = 2;
    public LayerMask layermask;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        interactionPrompt.text = "Press 'F'";
        mainCam = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * 5, Color.yellow);

        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, InteractionRadius, layermask))
        {
            ObjectLookingAt = hit.collider.gameObject;

            interactionPrompt.enabled = true;
            if(hit.collider.tag == "BuyItem")
            {
                if (hit.collider.GetComponent<BuyScript>().cost > GameManager.PlayerManager.pm.goldAmount)
                {
                    interactionPrompt.text = "Insufficient";
                }
                else
                {
                    interactionPrompt.text = "Press F to Buy";
                }

            }
            else
            {
                interactionPrompt.text = "Press F";
            }
            Debug.Log("HIT");
            Debug.Log(hit.collider.gameObject.tag);

        }
        else
        {
            interactionPrompt.enabled = false;
        }

    }

    public void Interaction(bool isInteracting)
    {
        StartCoroutine(InteractionActivation(isInteracting));
    }

    public void ResetInteraction()
    {
        interactionPrompt.enabled = false;
    }


    private IEnumerator InteractionActivation(bool isInteracting)
    {
        isInteracting = true;
        yield return new WaitForSeconds(1f);
        isInteracting = false;
    }

}
