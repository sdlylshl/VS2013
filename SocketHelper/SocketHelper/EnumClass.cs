/********************************************************************
 * *
 * * Copyright (C) 2013-? Corporation All rights reserved.
 * * 作者： BinGoo QQ：315567586 
 * * 请尊重作者劳动成果，请保留以上作者信息，禁止用于商业活动。
 * *
 * * 创建时间：2014-08-05
 * * 说明：
 * *
********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace SocketHelper
{
    public class EnumClass
    {
        public enum SocketState
        {
            /// <summary>
            /// 正在连接
            /// </summary>
            Connecting=0,
            /// <summary>
            /// 已连接
            /// </summary>
            Connected=1,
            /// <summary>
            /// 重新连接
            /// </summary>
            Reconnection=2,
            /// <summary>
            /// 断开连接
            /// </summary>
            Disconnect=3
        }
    }
}
