

#import "PhoneCollectorPlugin.h"
#import <CellphoneCollector/CellphoneCollector.h>

@implementation PhoneCollectorPlugin

PhoneCollectorPlugin * inst = nil;

+(PhoneCollectorPlugin *) Instance
{
    if(inst == nil)
    {
        inst =  [[PhoneCollectorPlugin alloc] init];
    }
    return inst;
}


void InitPlugin()
{
   
    
    UIViewController * unityRootVC =  UnityGetGLViewController();
    UIView * unityView = UnityGetGLView();
    Initialize(unityView, unityRootVC,LC_CN);
    
    
}
void Feedback(const char * aid, const char * sid, const char * uid, const char* account, const char* appsecret)
{
    SubmitFeedback([NSString stringWithUTF8String:aid],
                   [NSString stringWithUTF8String:sid],
                   [NSString stringWithUTF8String:uid],
                   [NSString stringWithUTF8String:account],
                     [NSString stringWithUTF8String:appsecret],
                   [PhoneCollectorPlugin Instance]);
    //SubmitFeedback(@"1", @"2", @"3", @"4", @"1234", [PhoneCollectorPlugin Instance]);
}

 void  Bind(const char* ver, const char* aid, const char* locat, const char* uid, const char* rid, const char* sid, int lv)
{
   BindCellphone([NSString stringWithUTF8String:ver],
                 [NSString stringWithUTF8String:aid],
                 [NSString stringWithUTF8String:locat],
                 [NSString stringWithUTF8String:uid],
                 [NSString stringWithUTF8String:rid],
                 [NSString stringWithUTF8String:sid],
                 lv,
                 [PhoneCollectorPlugin Instance]);
    //BindCellphone(@"ver", @"aid", @"locat", @"uid", @"rid", @"sid", 100, inst);
}

-(void) CollectorResulet:(enum COLLECTORRESULET) res
{
    //UIAlertView* alert = [[UIAlertView alloc] initWithTitle:@"user id" message:hashid delegate:nil cancelButtonTitle:@"no" otherButtonTitles:@"yes", nil];
    //[alert show];
}

@end
