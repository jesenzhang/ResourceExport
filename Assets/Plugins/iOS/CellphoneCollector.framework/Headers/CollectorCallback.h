#ifndef Unity_iPhone_CollectorCallback_h
#define Unity_iPhone_CollectorCallback_h


enum COLLECTORRESULET
{
    CR_TIMEOUT = 0,//server error
    CR_BIND_SUCESS,
    CR_UNBIND_SUCESS,
    CE_FEEDBACK_SUCESS,
    CR_CLOSE,
};
@protocol CollectorCallback

-(void) CollectorResulet:(enum COLLECTORRESULET) res;

@end
#endif
