using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherChecker : MonoBehaviour
{
    [SerializeField] GameObject MainBody;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            MainBody.GetComponent<AI_Chess>().Retreat();
        }
    }
}
