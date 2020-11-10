using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.UI_And_Animation
{
    public class StaffAnim : MonoBehaviour
    {
        public GameObject prefab;
        public GameObject prefab2;
        private Vector3 _position;


        public static StaffAnim Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }
    
 
    
        public  void PlayAnim(Vector3 enemyPosition)
        {
            _position = enemyPosition;
            _position.z = -1;
            Invoke($"PlayingAnimation1", 0.3f);
            Invoke($"PlayingAnimation2", 0.45f);

        }

        private void PlayingAnimation1()
        {
            GameEnvironment.Instance.PlayElectricSound();
            Time.timeScale = 0.3f;
            GameObject staffParticle = Instantiate(prefab,gameObject.transform);
            staffParticle.transform.localPosition = new Vector3(0, 0, -0.77f);
            Destroy(staffParticle, 3f);
            Invoke($"BackTime", 0.5f);
        }

        private void BackTime()
        {
            GameEnvironment.Instance.StopSound();
            Time.timeScale = 1f;
 
        }
        private void PlayingAnimation2()
        {
            Time.timeScale = 0.6f;
            GameObject lightningParticle = Instantiate(prefab2, _position, new Quaternion(-0.707106829f,0,0,0.707106829f));
            Destroy(lightningParticle, 3f);
        
        
        
        }
    
    
    }
}
