using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    //Level Manager
    [Header("Level Manager")]
    public LevelManager levelManager;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(levelManager != null) {
            SetScript set = collision.gameObject.transform.parent.gameObject.GetComponent<SetScript>();
            if(set != null) {
                levelManager.ResetLevel();
            }
        }
    }
}
