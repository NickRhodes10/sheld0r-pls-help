using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class BuyScript : MonoBehaviour
{
    public GameObject itemToSell;
    public UseItem item;

    [SerializeField] private string objectName;
    [SerializeField] private string objectDesc;
    [SerializeField] public int cost = 4;


    [SerializeField] TMPro.TMP_Text Name;
    [SerializeField] TMPro.TMP_Text Description;
    [SerializeField] TMPro.TMP_Text Cost;


    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        cost = Random.Range(2, 8);
        Name.text = objectName;
        Description.text = objectDesc;
        Cost.text = "Cost: " + cost + " Gold";
        anim = GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        //activate animation that transition into stay
        anim.Play("OnScreen");

        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerRaycastInteraction>().ObjectLookingAt == itemToSell && Input.GetKeyDown(KeyCode.F))
            {
                Hotbar.instance.useMagic = item;
                GameManager.PlayerManager.pm.goldAmount -= cost;
                anim.Play("OffScreen");
                Destroy(itemToSell);
                anim.enabled = false;
                Name.enabled = false;
                Description.text = "Sold";
                Cost.enabled = false;
                anim.enabled = true;
                anim.Play("OnScreen");
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerRaycastInteraction>().ObjectLookingAt == itemToSell && Input.GetKeyDown(KeyCode.F))
            {
                Destroy(itemToSell);
                anim.enabled = false;
                Name.enabled = false;
                Description.text = "Sold";
                Cost.enabled = false;
                anim.enabled = true;
                anim.Play("OnScreen");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        anim.Play("OffScreen");
    }
}
