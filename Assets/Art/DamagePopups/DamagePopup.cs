/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using TMPro;
using UnityEngine;

namespace DamagePopups
{
    public class DamagePopup : MonoBehaviour
    {
        public static DamagePopup Create(Vector3 position, int damageAmount) {
            GameObject damagePopupTransform = Instantiate(Resources.Load<GameObject>("pfDamagePopup"), position, Quaternion.identity);
            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(damageAmount);

            return damagePopup;
        }

        private static int _sortingOrder = 2000;
        private const float DisappearTimerMAX = 1f;
        private TextMeshPro _textMesh;
        private float _disappearTimer;
        private Color _textColor;
        private Vector3 _moveVector;

        private void Awake() {
            _textMesh = transform.GetComponent<TextMeshPro>();
        }

        private void Setup(int damageAmount) {
            _textMesh.SetText(damageAmount.ToString());
                // Normal hit
                _textMesh.fontSize = 14;
                _textColor = new Color(255,0,0);
            
            _textMesh.color = _textColor;
            _disappearTimer = DisappearTimerMAX;

            _sortingOrder++;
            _textMesh.sortingOrder = _sortingOrder;

            _moveVector = new Vector3(0f, 0.03f) * 60f;
        }

        private void Update() {
            if(_textMesh==null) return;
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= _moveVector * 8f * Time.deltaTime;

            if (_disappearTimer > DisappearTimerMAX * .5f) {
                // First half of the popup lifetime
                float increaseScaleAmount = 1f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            } else {
                // Second half of the popup lifetime
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }

            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer < 0) {
                // Start disappearing
                float disappearSpeed = 3f;
                _textColor.a -= disappearSpeed * Time.deltaTime;
                _textMesh.color = _textColor;
                if (_textColor.a < 0) {
                    Destroy(gameObject);
                }
            }
        }

    }
}
