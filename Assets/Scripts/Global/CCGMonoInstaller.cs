using CCG;
using CCG.UI;
using UnityEngine;
using Zenject;

namespace Global
{
    public class CCGMonoInstaller : MonoInstaller
    {
        [SerializeField] private HandContainer _handContainer;
        
        public override void InstallBindings()
        {
            Container.Bind<HandContainer>().FromInstance(_handContainer).AsSingle();
        }
    }
}