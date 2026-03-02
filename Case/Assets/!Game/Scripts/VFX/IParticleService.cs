using _Game.Scripts.Services;
using UnityEngine;

namespace _Game.Scripts.Core.VFX
{
    public interface IParticleService : IService
    {
        public void Play(string effectName, Vector3 position, Quaternion rotation, Color color);
    }
}


