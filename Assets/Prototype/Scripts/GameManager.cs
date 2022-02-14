
using System.Collections;
using UnityEngine;


namespace Prototype.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [SerializeField] private WaveManager waveManager;

        private bool building; 


        void Start()
        {
            StartCoroutine(GameLoop());
        }

        private IEnumerator GameLoop()
        {
            while (true)
            {
                if (building)
                {
                    //TODO Build State 
                }
                else
                {
                    //StartCoroutine(waveManager.StartWave());
                }
                yield return null;
            }
        }
    }
}

