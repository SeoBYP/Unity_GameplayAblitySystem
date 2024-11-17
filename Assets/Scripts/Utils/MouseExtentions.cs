using UnityEngine;
using UnityEngine.InputSystem;

namespace GameplayAbilitySystem.Utils
{
    public static class MouseExtentions
    {
        /// <summary>
        /// 마우스의 위치를 World Position으로 변환하여 가져오는 코드
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseToWorldPosition()
        {
            // 마우스 화면 좌표를 가져옴
            Vector3 mousePos = Mouse.current.position.ReadValue();
            // 마우스 위치에서 카메라에서부터 월드로 쏘는 Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            
            // Raycast를 사용하여 충돌하는 위치를 확인
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 충돌 지점의 좌표를 pointOfInterest 위치로 설정
                Debug.Log($"Hit position: {hit.point}");
                return hit.point;
                //characterWeaponAbility.ActivateAbility();
            }
            return Vector3.zero;
        }
    }
}