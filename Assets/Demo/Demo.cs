using UnityEngine;
using GetSocialSdk.Core;
using UnityEngine.UI;
using System.Collections.Generic;

public class Demo : MonoBehaviour
{
    private string _referralData = string.Empty;

    public Text referralDataText;

    void Start()
    {
        GetSocial.Instance.SetOnReferralDataReceivedListener(referralData =>
            {
                var referralDataToString = "\n";

                foreach (Dictionary<string, string> dictionary in referralData)
                {
                    foreach (KeyValuePair<string, string> entry in dictionary)
                    {
                        referralDataToString += entry.Key + ": " + entry.Value + "\n";
                    }

                    referralDataToString += "---";
                }
               
                var referralDataLog = "Referral data received: \n" + referralDataToString;
                Debug.Log(referralDataLog);
                _referralData = referralDataLog;
            });
        
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

    void Update()
    {
        referralDataText.text = _referralData;
    }

    public void OnClick()
    {
        GetSocial.Instance.CreateSmartInviteView().Show();
    }
}
