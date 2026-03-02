using _Game.Scripts.Services;

namespace _Game.Scripts.Core.Audio
{
    public interface IAudioService : IService
    {
        public void Play(string clipName);
    }
}

