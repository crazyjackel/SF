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
    private SaveFile m_file;

    [SerializeField]
    private string m_levelToBeat;
    [SerializeField]
    private string m_previousWorld;
    [SerializeField]
    private string m_nextWorld;


    public ReactiveCommand<ClickEvent> OnClickNext;
    public ReactiveCommand<ClickEvent> OnClickPrevious;

    public override void OnInitialization()
    {
        OnClickNext = new ReactiveCommand<ClickEvent>(Observable.Return(m_nextWorld != null && m_levelToBeat != null && m_file.IsLevelComplete(m_levelToBeat)));
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
        OnClickNext = new ReactiveCommand<ClickEvent>(Observable.Return(m_previousWorld != null));
        OnClickNext.Subscribe(x =>
        {
            try
            {
                SceneManager.LoadScene(m_previousWorld, LoadSceneMode.Single);
            }
            catch (Exception)
            {

            }
        });
    }
}
