﻿using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {
        PopupNavigation popupNavigation;

        public TMToast()
        {
            popupNavigation = new PopupNavigation();
        }

       
        public void Show(string message, string actionButtonText = null, Action? action = null, ToastTheme theme = ToastTheme.Default, bool isDismissable = false)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("Message is required");
            }
            popupNavigation.PushAsync(new TMToastContents(message, actionButtonText, popupNavigation, theme, action, isDismissable), false);
        }
    }
}
