using RedMoon.ReactiveKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WorldSelectViewModel : ViewModel<WorldSelectViewModel>
{
    [SerializeField] 
    private SaveFile m_save;

    [SerializeField]
    private string m_levelToBeat;
    [SerializeField]
    private string m_previousWorld;
    [SerializeField]
    private string m_nextWorld;
    [SerializeField]
    private string m_mainWorld;


    public ReactiveCommand<ClickEvent> OnClickNext;
    public ReactiveCommand<ClickEvent> OnClickPrevious;
    public ReactiveCommand<ClickEvent> OnClickBack;

    public override void OnInitialization()
    {
        OnClickNext = new ReactiveCommand<ClickEvent>(Observable.Return(m_nextWorld != null && m_nextWorld != "" && m_levelToBeat != null && m_levelToBeat != "" && m_save.IsLevelComplete(m_levelToBeat)));
        OnClickNext.Subscribe(x =>
        {
            try
            {
                SceneManager.LoadScene(m_nextWorld, LoadSceneMode.Single);
            }
            catch (Exception)
            {

            }
        });
        OnClickPrevious = new ReactiveCommand<ClickEvent>(Observable.Return(m_previousWorld != null && m_previousWorld != ""));
        OnClickPrevious.Subscribe(x =>
        {
            try
            {
                SceneManager.LoadScene(m_previousWorld, LoadSceneMode.Single);
            }
            catch (Exception)
            {

            }
        });

        OnClickBack = new ReactiveCommand<ClickEvent>(Observable.Return(m_mainWorld != null && m_mainWorld != ""));
        OnClickBack.Subscribe(x =>
        {
            try
            {
                SceneManager.LoadScene(m_mainWorld, LoadSceneMode.Single);
            }
            catch (Exception)
            {

            }
        });
    }
}
