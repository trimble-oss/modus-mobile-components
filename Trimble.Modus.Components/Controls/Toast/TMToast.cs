﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {
        PopupNavigation popupNavigation;
        public TMToast() {

            popupNavigation  = new PopupNavigation();
        
        }
        public void Show(string message, ImageSource leftIconSource = null, string actionButtonText = null, ToastTheme theme = 0, Action? action = null)
        {
            if(string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("Message is required");
            }
           popupNavigation.PushAsync(new TMToastContents(message ,leftIconSource,  actionButtonText, popupNavigation,theme,action),false);
        }
      
    }
}
