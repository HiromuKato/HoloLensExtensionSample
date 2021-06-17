using System;
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
#if WINDOWS_UWP
            try
            {
                Windows.ApplicationModel.Core.CoreApplicationView mainView = Windows.ApplicationModel.Core.CoreApplication.MainView;
                Windows.UI.Core.CoreWindow cw = mainView.CoreWindow;
                Windows.UI.Core.CoreDispatcher dispatcher = cw.Dispatcher;

                ExtensionManager.Initialize(dispatcher);
            }
            catch (Exception ex)
            {
                Debug.Log("Exception: " + ex.Message);
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
            // インストールされている拡張名を表示
            foreach (var e in ExtensionManager.Extensions)
            {
                Debug.Log(e.AppExtension.DisplayName);
            }

            // 2と3を拡張機能に送る
            ValueSet message = new ValueSet();
            message.Add("arg1", 2);
            message.Add("arg2", 3);

            try
            {
                // テストとして最初に見つかった拡張機能を実行
                Extension ext = ExtensionManager.Extensions[0];
                double result = await ext.Invoke(message);
                Debug.Log(result.ToString());

                textMesh.text = result.ToString();

                if (result.ToString().Equals("NaN"))
                {
                    // 拡張機能の実行に失敗したときのエラー処理
                    Debug.Log(result.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
#endif
        }
    }
}
