using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPrototype
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        private void OnEnable()
        {
            this.GetCachedComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            this.GetCachedComponent<Button>().onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            EventBus.Instance.Raise(new SoundEvents.SoundEvent { type = SoundEvents.SoundType.ButtonClick });
        }
    }
}
