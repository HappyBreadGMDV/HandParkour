using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;

    private float maxHp;

    public RectTransform HpBar;
    private RectTransform StartHpBar;

    public void Start()
    {
        StartHpBar = HpBar;
        maxHp = hp;
    }

    public void FixedUpdate()
    {
        HpBar.DOSizeDelta(new Vector2(StartHpBar.sizeDelta.x, hp), 1f);
    }
}
