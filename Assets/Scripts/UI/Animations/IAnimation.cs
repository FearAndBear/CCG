using Cysharp.Threading.Tasks;

namespace CCG.Animations
{
    public interface IAnimation
    {
        bool IsPlaying { get; }
        UniTask StartAnimation(bool isReverse = false);
    }
}