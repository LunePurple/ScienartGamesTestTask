#nullable enable

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Slider Slider = null!;
        [SerializeField] private TextMeshProUGUI Text = null!;

        private void Start()
        {
            Health.OnLocalPlayerHealthChanged += Health_OnLocalPlayerHealthChanged;
        }

        private void OnDestroy()
        {
            Health.OnLocalPlayerHealthChanged -= Health_OnLocalPlayerHealthChanged;
        }

        private void UpdateVisual(Health.HealthChangedData data)
        {
            Slider.value = (float)data.Current / data.Max;
            Text.text = data.Current.ToString();
        }

        private void Health_OnLocalPlayerHealthChanged(Health.HealthChangedData data)
        {
            UpdateVisual(data);
        }
    }
}