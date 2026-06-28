using UnityEngine;
using System.Runtime.InteropServices;

[DisallowMultipleComponent]
public class PlayableManager : MonoBehaviour
{
    public static PlayableManager Instance { get; private set; }

    [Header("Store URLs")]
    [SerializeField] private string _appStoreUrl = "https://apps.apple.com";
    [SerializeField] private string _playStoreUrl = "https://play.google.com";

    [DllImport("__Internal")]
    private static extern void CallStoreRedirect(string url);

    [DllImport("__Internal")]
    private static extern void LogPlayableEvent(string eventName);

    private bool _hasRedirected = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SendEvent("playable_impression");
    }
    public void SendEvent(string eventName)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            LogPlayableEvent(eventName);
        #else
            Debug.Log($"<color=yellow>[Data Log] Event: {eventName}</color>");
        #endif
    }

    public void RedirectToStore()
    {
        if (_hasRedirected) return;
        _hasRedirected = true;

        SendEvent("cta_click_or_auto_redirect");
        
        Time.timeScale = 0f;

        string targetUrl = _playStoreUrl;
        #if UNITY_IOS
            targetUrl = _appStoreUrl;
        #endif

        #if UNITY_WEBGL && !UNITY_EDITOR
            CallStoreRedirect(targetUrl);
        #else
            Debug.Log($"<color=red>[The MRAID event was supposed to be triggered] URL: {targetUrl}</color>");
            Application.OpenURL(targetUrl);
        #endif
    }
}