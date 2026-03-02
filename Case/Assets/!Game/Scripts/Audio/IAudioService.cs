using _Game.Scripts.Services;

namespace _Game.Scripts.Audio
{
    public interface IAudioService : IService
    {
        public void Play(string clipName);
    }
}

