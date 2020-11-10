using System.Linq;
using _Scripts.Managers;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI_And_Animation
{
    public class UIAbilityHolder : MonoBehaviour
    {
        private static UIAbilityHolder _instance;
        public Transform stickLocation;
        public Vector3 offset;

        public TextMeshProUGUI unityName;
        public TextMeshProUGUI health;
        public TextMeshProUGUI armor;
        public TextMeshProUGUI damage;
        public Image armorTypeIcon;
        public Image weaponTypeIcon;
        public Transform uiStatsHolderTransform;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI defText;

        public Image defImage;
        private Camera _camera;
        private Vector3 _pos;


        private float _startPosY;
        private float _startPosY2;

        private void Start()
        {
            FixUILocation();

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                _startPosY = offset.y;
                _startPosY2 = _startPosY + 1.5f;
                GameEnvironment.Instance.OnMouseSelectNpcAction += ShowData;
                GameEnvironment.Instance.OnSelfTargetSpell += ShowData;

                gameObject.SetActive(false);
            }
        }

        private void FixUILocation()
        {
            _camera = GameEnvironment.Instance.mainCamera;
            _pos = _camera.WorldToScreenPoint(stickLocation.position + offset);
            if (transform.position != _pos)
            {
                transform.position = _pos;
            }
        }


        private void ShowData(GameObject go, NpcInfo npcInfo, bool isVisible, float healthTemp)
        {
            if (!GameEnvironment.IsOurTurn) return;

            if (!isVisible || !npcInfo.isTeamPlayer || GameEnvironment.isGameOver)
            {
                gameObject.SetActive(false);
                stickLocation.gameObject.SetActive(true);
                return;
            }


            stickLocation.gameObject.SetActive(false);
            offset.y = _startPosY2;
            FixUILocation();
            gameObject.SetActive(true);
            unityName.text = npcInfo.unitName;
            this.health.text = (int) healthTemp + "";
            armor.text = npcInfo.armor + "% ";
            armorTypeIcon.sprite = npcInfo.armorTexture;
            damage.text = npcInfo.hasAbilities[0].abilityMinDmg + "-" + npcInfo.hasAbilities[0].abilityMaxDmg;
            weaponTypeIcon.sprite = npcInfo.hasAbilities[0].abilityIcon;
            attackText.text = npcInfo.hasAbilities[0].abilityName;
            if (npcInfo.hasAbilities.Count() < 2)
            {
                defText.gameObject.SetActive(false);
                defImage.gameObject.SetActive(false);
            }
            else
            {
                defImage.gameObject.SetActive(true);
                defText.gameObject.SetActive(true);
                defText.text = npcInfo.hasAbilities[1].abilityName;
            }
        }
    }
}