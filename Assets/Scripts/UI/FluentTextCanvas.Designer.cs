//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QFramework.Example
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    // Generate Id:0e445c02-34fe-46c4-9237-7c418c5f62e8
    public partial class FluentTextCanvas
    {
        
        public const string NAME = "FluentTextCanvas";
        
        private FluentTextCanvasData mPrivateData = null;
        
        public FluentTextCanvasData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new FluentTextCanvasData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            mData = null;
        }
    }
}
