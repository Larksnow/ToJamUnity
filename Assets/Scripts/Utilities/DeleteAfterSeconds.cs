using System.Collections;
using UnityEngine;

public class DeleteAfterSeconds : MonoBehaviour
{
    [SerializeField] float seconds;
    void Start()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }

}
