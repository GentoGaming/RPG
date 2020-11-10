using _Scripts.Managers;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI_And_Animation
{
    public class UIToWorldSpace : MonoBehaviour
    {
        public Transform lookAt;
        public Vector3 offset;
        private Camera _camera;
        private Vector3 _pos;
        
        private static UIToWorldSpace _instance;
        
        public TextMeshProUGUI unityName;
        public TextMeshProUGUI health;
        public TextMeshProUGUI armor;
        public TextMeshProUGUI damage;
        public Image armorTypeIcon;
        public Image weaponTypeIcon;


        private void ShowData(GameObject go, NpcInfo npcInfo, bool isVisible, float unitHealth)
        {
            if (!isVisible || GameEnvironment.isGameOver)
            { 
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
            unityName.text = npcInfo.unitName;
            health.text = (int)unitHealth + "";
            armor.text = npcInfo.armor + "% " ;
            armorTypeIcon.sprite = npcInfo.armorTexture;
            damage.text = npcInfo.hasAbilities[0].abilityMinDmg + "-" + npcInfo.hasAbilities[0].abilityMaxDmg;
            weaponTypeIcon.sprite = npcInfo.hasAbilities[0].abilityIcon;
            lookAt = go.transform;
        }
        
        
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
                GameEnvironment.Instance.OnMouseOverNpcAction += ShowData;
                gameObject.SetActive(false);
            }
        }
        
        void Start()
        {
            _camera = GameEnvironment.Instance.mainCamera;
        }

        
        
        
        void Update()
        {
            _pos = _camera.WorldToScreenPoint(lookAt.position + offset);
            if (transform.position != _pos)
            {
                transform.position = _pos;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        { 
            gameObject.SetActive(false);
        }
    }
}
