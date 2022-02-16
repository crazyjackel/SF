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


    private Color _color;
    public Color GetColor() => _color;

    public SmartColor() { }
    public SmartColor(string name, int id, Color color) : base(name, id)
    {
        this._color = color;
    }
}