using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider; // Unity UI Slider 컴포넌트를 참조합니다.
    public Transform target; // HealthBar가 따라다닐 대상을 참조합니다.
    public Vector3 offset; // HealthBar의 위치 오프셋입니다.

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // HealthBar를 초기화하는 메소드입니다.
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    // HealthBar의 값을 설정하는 메소드입니다.
    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }

    // 매 프레임마다 호출되는 Unity 콜백 메소드입니다.
    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
