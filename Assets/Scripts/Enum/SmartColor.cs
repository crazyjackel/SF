using RedMoon.Smartables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SmartColor : SmartEnum<SmartColor>
{
    public static readonly SmartColor Default = new SmartColor(nameof(Default), 0, Color.grey);
    public static readonly SmartColor Red = new SmartColor(nameof(Red), 1, Color.red);
    public static readonly SmartColor Green = new SmartColor(nameof(Green), 2, Color.green);
    public static readonly SmartColor Blue = new SmartColor(nameof(Blue), 3, Color.blue);
    public static readonly SmartColor Yellow = new SmartColor(nameof(Yellow), 4, Color.yellow);


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