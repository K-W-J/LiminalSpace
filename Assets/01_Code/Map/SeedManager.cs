using System;
using KWJ.Code.Core;
using UnityEngine;

namespace KWJ.Code.Map
{
    public class SeedManager : MonoSingleton<SeedManager>
    {
        [SerializeField] private int _seed;
        public int SeedValue { get; private set; }
        
        private void Awake()
        {
            if (_seed == 0)
                _seed = DateTime.Now.GetHashCode();

            SeedValue = _seed;
        }
        
        public System.Random CreateRandom(string systemName)
        {
            int systemSeed = SeedValue + systemName.GetHashCode();
            return new System.Random(systemSeed);
        }
    }
}