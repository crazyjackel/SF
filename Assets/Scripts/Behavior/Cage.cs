using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelPopUpClickable))]
[RequireComponent(typeof(SpriteRenderer))]
public class Cage : MonoBehaviour
{
    [SerializeField]
    private LevelPopUpClickable clickable;
    public LevelPopUpClickable Clickable => clickable;

    [SerializeField]
    private SpriteRenderer sprRenderer;
    public SpriteRenderer SpriteRenderer => sprRenderer;

    [SerializeField]
    private bool isGolden;
    public bool IsGolden => isGolden;

    [SerializeField]
    private string previousLevelName;
    public string PreviousLevelName => previousLevelName;

    private void Awake()
    {
        clickable = GetComponent<LevelPopUpClickable>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }
}
