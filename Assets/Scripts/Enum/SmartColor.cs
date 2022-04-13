using RedMoon.Smartables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SmartColor : SmartEnum<SmartColor>
{
    public static readonly SmartColor Default = new SmartColor(nameof(Default), 0, Color.grey);
    public static readonly SmartColor Red = new SmartColor(nameof(Red), 1, new Color(1,0,0.25f));
    public static readonly SmartColor Yellow = new SmartColor(nameof(Yellow), 2, Color.yellow);
    public static readonly SmartColor Green = new SmartColor(nameof(Green), 3, new Color(25f/256,125f/256,15f/256));
    public static readonly SmartColor Blue = new SmartColor(nameof(Blue), 4, new Color(0.35f,0.8f,0.8f));
    public static readonly SmartColor Purple = new SmartColor(nameof(Purple), 5, new Color(0.39f, 0, 0.44f));


    private Color _color;
    public Color GetColor() => _color;

    protected override void ConstructEnum(SmartColor @enum)
    {
        this._color = @enum._color;
    }

    public SmartColor(string name, int id, Color color) : base(name, id)
    {
        this._color = color;
    }
}