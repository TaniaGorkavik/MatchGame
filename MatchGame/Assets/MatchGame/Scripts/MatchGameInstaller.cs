using MatchGame.Utils;
using UnityEngine;
using Zenject;

namespace MatchGame
{
    public class MatchGameInstaller : MonoInstaller
    {
        [SerializeField] private MatchGame _matchGame;
        [SerializeField] private MatchBoard _matchBoard;


        [Inject]
        private void Construct()
        {
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MatchGame>().FromInstance(_matchGame);
            Container.BindInterfacesAndSelfTo<MatchBoard>().FromInstance(_matchBoard);

            Container.BindCustomClass<InputManager>();
        }
    }
}