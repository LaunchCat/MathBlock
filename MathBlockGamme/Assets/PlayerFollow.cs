using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    Transform player;
    [SerializeField] float lerpSpeed = 0.5f;
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    void Update()
    {
        Vector3 posToMove = (player.position - transform.position) * lerpSpeed;
        posToMove.y = 0;
        transform.position += posToMove * Time.deltaTime;
    }
}
