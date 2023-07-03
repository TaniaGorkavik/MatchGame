using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MatchGame
{
    public class MatchGame : MonoBehaviour, IInitializable
    {
        [SerializeField] private List<LevelConfig> _levels = new List<LevelConfig>();

        public LevelConfig CurrentLevel => _levels[0];
        
        [Inject]
        private void Construct()
        {

        }

        public void Initialize()
        {
            
        }
    }
}