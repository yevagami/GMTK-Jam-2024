using System.Collections;
using UnityEngine;

public class FlashingMessageScript : MonoBehaviour
{
    public Animator anim;

    private void Awake() {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 1.0f);
        Destroy(gameObject);
    }
}
