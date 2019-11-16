using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayerCallback : MonoBehaviour
{
    [SerializeField] private Animation m_anim;
    [SerializeField] private string m_animName;

    public void PlayAnim()
    {
        m_anim.Play(m_animName);
    }

}
