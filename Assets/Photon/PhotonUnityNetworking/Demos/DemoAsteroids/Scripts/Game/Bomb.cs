using Photon.Realtime;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public ParticleSystem bombParticles;
    public Player owner;

    public void InitializeBomb(Player owner, ParticleSystem bombParticles, float lag)
    {
        this.owner = owner;
        this.bombParticles = bombParticles;

        if (owner.ActorNumber == 0) {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.position += rigidbody.velocity * lag;
        }
    }
}
