#import "KakaoGetSocialBridgeUtils.h"

@implementation KakaoGetSocialBridgeUtils

// Converts C style string to NSString
+ (NSString *)createNSStringFrom:(const char *)cstring {
    return [NSString stringWithUTF8String:(cstring ?: "")];
}

+ (NSArray *)createNSArray:(int)count values:(const char **)values {
    if (count == 0) {
        return nil;
    }
    
    NSMutableArray *mutableArray = [NSMutableArray array];
    
    for (int i = 0; i < count; i++) {
        mutableArray[i] = [self createNSStringFrom:values[i]];
    }
    
    return mutableArray;
}

+ (char *)cStringCopy:(const char *)string {
    char *res = (char *)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

+ (char *)createCStringFrom:(NSString *)string {
    if (!string) {
        string = @"";
    }
    return [self cStringCopy:[string UTF8String]];
}

@end
