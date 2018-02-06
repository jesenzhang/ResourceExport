//
//  CellphoneCollector.h
//  CellphoneCollector
//
//  Created by EngineNo1 on 16/12/12.
//  Copyright (c) 2016年 cyou_engine. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "CollectorCallback.h"

//! Project version number for CellphoneCollector.
FOUNDATION_EXPORT double CellphoneCollectorVersionNumber;

//! Project version string for CellphoneCollector.
FOUNDATION_EXPORT const unsigned char CellphoneCollectorVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <CellphoneCollector/PublicHeader.h>

enum LOCATION
{
    LC_CN = 0,
    LC_US,
};

void Initialize(UIView* parent, UIViewController* con, enum LOCATION loc);
void BindCellphone(NSString* ver, NSString* aid, NSString* locat, NSString* uid, NSString* rid, NSString* sid, int lv, id<CollectorCallback> callback);
void SubmitFeedback(NSString* aid, NSString* sid, NSString* uid, NSString* account, NSString* appsecret, id<CollectorCallback> callback);
