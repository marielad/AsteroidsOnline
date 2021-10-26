using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class Bomb : MonoBehaviour
{
    public GameObject particles;
    public float disappearanceTime = AsteroidsGame.BOMB_DESPAWN_TIME;


    private ParticleSystem bombParticles;
    private GameObject[] destroyAsteroids;
    private GameObject[] destroySpaceships;
    private PhotonView photonView;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        bombParticles = particles.gameObject.GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (disappearanceTime <= 0.0)
        {
                PhotonNetwork.Destroy(gameObject);
        }
        if (disappearanceTime > 0.0f)
        {
            disappearanceTime -= Time.deltaTime;
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            Debug.Log("EXPLOTAR BOMBAAAAAAA -------------------------------------------------------------------------");
            if (!bombParticles.isPlaying)
            {
                bombParticles.Play();
            }

            StartCoroutine(Explosion(bullet));
        }
    }

    private IEnumerator Explosion(Bullet bullet)
    {
        destroyAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        //Debug.Log("PUNTOS POR ASTEROIDES GRANDES: " + destroyAsteroids.Length * 2);

        destroySpaceships = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log("PUNTOS POR NAVES: " + (destroySpaceships.Length-1));

        gameObject.GetComponent<Renderer>().enabled = false;

        foreach (var asteroid in destroyAsteroids)
        {
            asteroid.GetComponent<Asteroid>().DestroyAsteroidGlobally();
        }

        foreach (var spaceship in destroySpaceships)
        {
            if (spaceship.GetComponent<Spaceship>().photonView.Owner != bullet.Owner)
            {
                bullet.Owner.AddScore(5);
                spaceship.GetComponent<Spaceship>().DestroySpaceship();
            }
        }

        yield return new WaitForSeconds(3.0f);


        if (bombParticles.isPlaying)
        {
            bombParticles.Stop();
        }

        PhotonNetwork.Destroy(gameObject);
    }

}
