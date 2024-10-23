using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private Transform target;

    public LayerMask bounds;
    // Start is called before the first frame update
    void Start()
    {
        target.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(target.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, bounds))
                {
                    target.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(target.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, bounds))
                {
                    target.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
        }
    }


}
