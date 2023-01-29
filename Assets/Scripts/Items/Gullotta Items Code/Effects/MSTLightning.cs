using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MSTLightning : MonoBehaviour
{

    public int LightningLVL = 0;
    public int MaxEnemyHit = 3;
    public int LightningDamage = 1;
    public Material material;

    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private LineRenderer _lineRenderer;

    private List<Collider> _hits = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Enemy");
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = material;
        LightningLVL = GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().CurrentMagicLVL;
        Cast();
    }

    private void Update()
    {
        if(LightningLVL == 1)
        {
            LightningDamage = 2;
            radius = 15;
            MaxEnemyHit = 4;
        }
        if(LightningLVL == 2)
        {
            LightningDamage = 3;
            radius = 20;
            MaxEnemyHit = 5;
        }
        if(LightningLVL == 3)
        {
            LightningDamage = 5;
            radius = 30;
            MaxEnemyHit = 6;
        }


        if(_lineRenderer == null || _hits == null)
        {
            return;
        }

        Debug.Log("Hits: " + _hits.Count);
        List<Vector3> points = new List<Vector3>();

        for(int i = 0; i < _hits.Count; i++)
        {
            points.Add(_hits[i].transform.position);

        }

        if(points.Count > MaxEnemyHit)
        {
            _lineRenderer.positionCount = MaxEnemyHit + 1;
        }
        else
        {
            _lineRenderer.positionCount = points.Count;
        }


        _lineRenderer.SetPositions(points.ToArray());
    }

    void Cast()
    {
        List<Collider> unsorted = new List<Collider>(Physics.OverlapSphere(transform.position, radius, layerMask));
        List<Collider> sorted = new List<Collider>();

        if(GetComponent<Collider>() != null)
        {
            unsorted.Insert(0, GetComponent<Collider>());
        }

        sorted.Add(unsorted[0]);
        unsorted.RemoveAt(0);

        while(unsorted.Count > 0)
        {
            float curBestDistance = Mathf.Infinity;

            int unsortedIndex = 0;

            for(int i = 0; i < sorted.Count; i++)
            {
                for (int j = 0; j < unsorted.Count; j++)
                {

                    float dist = Vector3.Distance(sorted[i].transform.position, unsorted[j].transform.position);

                    if (dist < curBestDistance)
                    {
                        curBestDistance = dist;
                        unsortedIndex = j;
                    }

                }
            }

            sorted.Add(unsorted[unsortedIndex]);
            unsorted.Remove(unsorted[unsortedIndex]);
        }


        Debug.Log(sorted.Count);
        _hits = sorted;

        StartCoroutine(Delay());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private IEnumerator Delay()
    {
        
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            if (_hits[i].GetComponent<EnemyStats>() == true)
            {
                _hits[i].GetComponent<EnemyStats>().currentHealth -= LightningDamage;
            }
        }
        Destroy(this);
        Destroy(_lineRenderer);
    }

}
