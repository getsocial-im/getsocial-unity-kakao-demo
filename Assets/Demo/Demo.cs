using UnityEngine;
using System.Collections;
using GetSocialSdk.Core;

public class Demo : MonoBehaviour
{
    void Start()
    {
        GetSocial.Instance.RegisterPlugin(KakaoInvitePlugin.ProviderName, new KakaoInvitePlugin());

        GetSocial.Instance.Init(() =>
            {
                Debug.Log("GetSocial Successfully Initialized");
            }, 
            () =>
            {
                Debug.Log("Failed to initialize GetSocial");
            });
    }

    public void OnClick()
    {
        GetSocial.Instance.CreateSmartInviteView().Show();
    }
}
