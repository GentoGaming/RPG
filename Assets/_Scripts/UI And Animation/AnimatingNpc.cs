using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace _Scripts.UI_And_Animation
{
    public class AnimatingNpc : MonoBehaviour
    {
        private static AnimatingNpc _instance;
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Ready = Animator.StringToHash("Ready");
        private static readonly int DamageTaken = Animator.StringToHash("DamageTaken");
        private static readonly int DieBack = Animator.StringToHash("DieBack");
        private static readonly int Jab = Animator.StringToHash("Jab");
        private static readonly int Slash = Animator.StringToHash("Slash");
        private static readonly int WeaponTypeS = Animator.StringToHash("WeaponType");
        private static readonly int MagazineType = Animator.StringToHash("MagazineType");
        private static readonly int HoldType = Animator.StringToHash("HoldType");
        private static readonly int Stand = Animator.StringToHash("Stand");
        private static readonly int Defence = Animator.StringToHash("Defence");

        public static AnimatingNpc Instance { get { return _instance; } }

        // Start is called before the first frame update
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }

        public void AnimatingIsWalking(GameObject activeNpc, bool isActive)
        {
            Character character = activeNpc.GetComponentInChildren<Character>();
            if (!character.Animator.isInitialized) return;
            character.Animator.SetBool(Walk, isActive);
        }


        public static void AnimatingDamage(GameObject activeNpc)
        {
            Character character = activeNpc.GetComponentInChildren<Character>();
            if (!character.Animator.isInitialized) return;
            character.Animator.SetBool(Ready, true);
            character.Animator.SetTrigger(DamageTaken);
        }


        public void AnimatingDieBack(GameObject activeNpc)
        {
            Character character = activeNpc.GetComponentInChildren<Character>();
            if (!character.Animator.isInitialized) return;
            character.Animator.SetBool(DieBack, true);
        }


        public void AnimatingAttackJab(GameObject activeNpc)
        {
            Character character = activeNpc.GetComponentInChildren<Character>();
            if (!character.Animator.isInitialized) return;
            character.Animator.SetBool(Ready, true);
            character.Animator.SetTrigger(Jab);
        }


        public void AnimatingAttackSlash(GameObject activeNpc)
        {
            Character character = activeNpc.GetComponentInChildren<Character>();
            if (!character.Animator.isInitialized) return;
            character.Animator.SetBool(Ready, true);
            character.Animator.SetTrigger(Slash);
        }

        public void AnimatingDefensive(GameObject activeNpc, bool condition)
        {
            Character character = activeNpc.GetComponentInChildren<Character>();
            if (!character.Animator.isInitialized) return;
            character.Animator.SetBool(Ready, condition);
            character.Animator.SetBool(Stand, condition);
            character.Animator.SetBool(Defence, condition);

        }

        public void AnimatingIdleAndActive(GameObject activeNpc, bool isActive)
        {
            int animationState = -1;
            Character character = activeNpc.GetComponentInChildren<Character>();
         
            if (!character.Animator.isInitialized) return;

            var state = 100 * (int) character.WeaponType;

            character.Animator.SetInteger(WeaponTypeS, (int) character.WeaponType);

            if (character.WeaponType == WeaponType.Firearms1H || character.WeaponType == WeaponType.Firearms2H || character.WeaponType == WeaponType.FirearmsPaired)
            {
                character.Animator.SetInteger(MagazineType, (int)character.Firearm.Params.MagazineType);
                character.Animator.SetInteger(HoldType, (int)character.Firearm.Params.HoldType);
                state += (int) character.Firearm.Params.HoldType;
            }

            if (state == animationState) return; // No need to change animation.
            animationState = state;
            character.Animator.SetBool(Ready, isActive);
            character.Animator.SetBool(Stand, isActive);
        }
    }
}
