using System.Collections;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] float maxRocketDur = 5f;
    [SerializeField] float particleSystemDur = 3f;
    Transform fireworks;
    Transform rocketMesh;
    bool collisionDetected = false;
    void Awake()
    {
        fireworks = transform.Find("Fireworks");
        rocketMesh = transform.Find("RocketMeshRenderer");
    }


    IEnumerator Start()
    {
        yield return new WaitForSeconds(maxRocketDur);
        if (!collisionDetected && gameObject)
        {
            DestroyGameObject();
        }
    }


    IEnumerator OnCollisionEnter(Collision collision)
    {
        if (gameObject)
        {
            collisionDetected = true;
            rocketMesh.gameObject.SetActive(false);
            foreach (ParticleSystem ps in fireworks.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Play();
            }
            GameObject collidedObject = collision.gameObject;
            if (collidedObject.CompareTag("Obstacle"))
            {
                StartCoroutine(DestroyObstacle(collidedObject));
            }
            yield return new WaitForSeconds(particleSystemDur);

            DestroyGameObject();
        }
    }

    void DestroyGameObject()
    {
        foreach (ParticleSystem ps in fireworks.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (gameObject)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyObstacle(GameObject obstacle)
    {
        yield return new WaitForSeconds(0.5f);
        if (obstacle)
        {
            Destroy(obstacle);
        }
        
    }
}
