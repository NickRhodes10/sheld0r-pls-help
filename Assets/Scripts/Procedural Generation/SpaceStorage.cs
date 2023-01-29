using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStorage : MonoBehaviour
{
    [SerializeField] private Collider[] _myTransforms;

    public bool isVisualized = true;

    private void Awake()
    {
        if (SpaceManager.instance == null)
        {
            return;
        }

        //GetComponent<BoxCollider>().isTrigger = true;

        //_myTransforms = Physics.OverlapBox(transform.position, SpaceManager.instance.spaceSize);
        //_myTransforms = Physics.OverlapBox(transform.position, SpaceManager.instance.spaceSize, Quaternion.identity, SpaceManager.instance.layerMask);
    }

    /*
    private void Update()
    {
        Visualize(isVisualized);
    }
    */
    /*
    private void OnEnable()
    {
        _myTransforms = Physics.OverlapBox(transform.position, SpaceManager.instance.spaceSize, Quaternion.identity, SpaceManager.instance.layerMask);
    }

    
    private void Start()
    {
        _myTransforms = Physics.OverlapBox(transform.position, SpaceManager.instance.spaceSize, Quaternion.identity, SpaceManager.instance.layerMask);
    }
    */

    public void FillTransforms()
    {
        _myTransforms = Physics.OverlapBox(transform.position, SpaceManager.instance.spaceSize / 2, Quaternion.identity, SpaceManager.instance.layerMask);
    }

    public void Visualize(bool flag)
    {
        for (int i = 0; i < _myTransforms.Length; i++)
        {
            _myTransforms[i].gameObject.SetActive(flag);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(transform.position, SpaceManager.instance.spaceSize);
        Gizmos.DrawWireCube(transform.position, SpaceManager.instance.spaceSize);
    }
}
