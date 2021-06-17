using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if WINDOWS_UWP
using Windows.Foundation.Collections;
#endif

namespace HoloLensExtensionSample
{
    public class ExtensionHost : MonoBehaviour
    {
        [SerializeField]
        private TextMesh textMesh;

#if WINDOWS_UWP
        ExtensionManager ExtensionManager { get; set; } = new ExtensionManager("Sample.com.MathExt");
#endif

        void Start()
        {
            /*
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
            }, true);
            */

#if WINDOWS_UWP
            try
            {
                //var dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
                var dispatcher = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher;
                ExtensionManager.Initialize(dispatcher);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
#endif
        }

        /// <summary>
        /// 拡張機能の利用
        /// </summary>
        public async void UseExt()
        {
            Debug.Log("Button Tapped");
#if WINDOWS_UWP
            ValueSet message = new ValueSet();
            message.Add("arg1", 2);
            message.Add("arg2", 3);

            //Extension ext = btn.DataContext as Extension;
            foreach (var e in ExtensionManager.Extensions)
            {
                Debug.Log(e.AppExtension.DisplayName);
            }

            Extension ext = ExtensionManager.Extensions[0];
            double result = await ext.Invoke(message);
            Debug.Log(result.ToString());

            textMesh.text = result.ToString();

            if (result.ToString().Equals("NaN"))
            {
                ext.Disable();
                await ext.Enable();
            }
#endif
        }
    }
}
