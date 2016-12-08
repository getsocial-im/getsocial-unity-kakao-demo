#import <Foundation/Foundation.h>
#import <UIKit/UIImage.h>

@interface KakaoGetSocialBridgeUtils : NSObject

+ (NSString *)createNSStringFrom:(const char *)cstring;

+ (char *)createCStringFrom:(NSString *)string;

+ (NSArray *)createNSArray:(int)count values:(const char **)values;

+ (char *)cStringCopy:(const char *)string;

@end
