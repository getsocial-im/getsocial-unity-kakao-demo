#include "KakaoGetSocialBridgeUtils.h"
#import <KakaoOpenSDK/KakaoOpenSDK.h>

extern "C" {
    void _sendKakaoInvite(const char* text, const char* referralDataUrl)
    {
        NSLog(@"TEST 1");
        
        NSString* textStr = [KakaoGetSocialBridgeUtils createNSStringFrom:text];
        NSString* referralDataUrlStr = [KakaoGetSocialBridgeUtils createNSStringFrom:referralDataUrl];
        
        KakaoTalkLinkObject *label = [KakaoTalkLinkObject createLabel:textStr];
        
        KakaoTalkLinkObject *webLink= [KakaoTalkLinkObject createWebLink:referralDataUrlStr
                                                                     url:referralDataUrlStr];
        
        [KOAppCall openKakaoTalkAppLink:@[label,webLink]];
        
        NSLog(@"TEST 2");
    }
    
    bool _isKakaoInstalled()
    {
        // check if Kakao Messenger is installed
        BOOL installed = [KOAppCall canOpenKakaoTalkAppLink];
        return installed;
    }
}
