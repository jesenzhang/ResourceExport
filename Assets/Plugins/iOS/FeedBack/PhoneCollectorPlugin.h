#pragma once

#import <Foundation/Foundation.h>
#import "UnityAppController.h"
#import <CellphoneCollector/CellphoneCollector.h>

@interface PhoneCollectorPlugin : NSObject<CollectorCallback>

+(PhoneCollectorPlugin *) Instance;

extern "C"
{
    void InitPlugin();
    void Feedback(const char * aid, const char * sid, const char * uid, const char* account, const char* appsecret);
    void  Bind(const char* ver, const char* aid, const char* locat, const char* uid, const char* rid, const char* sid, int lv);
}

@end
