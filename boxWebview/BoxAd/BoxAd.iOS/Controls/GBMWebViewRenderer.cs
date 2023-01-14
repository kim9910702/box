using Foundation;
using BoxAd.Controls;
using BoxAd.iOS.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BoxAdWebView), typeof(GBMWebViewRenderer))]
namespace BoxAd.iOS.Controls
{
    public class GBMWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        WKUserContentController userController;
        GBMBridge gbmBridge;

        public GBMWebViewRenderer() : this(new WKWebViewConfiguration())
        {
        }

        public GBMWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            config.Preferences.JavaScriptEnabled = true;

            gbmBridge = new GBMBridge(this, userController);
            gbmBridge.Initialize();
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null)
            {
                gbmBridge.Clear();

                BoxAdWebView webView = e.OldElement as BoxAdWebView;
                webView.Clear();
            }

            if(e.NewElement != null)
            {
                BoxAdWebView webView = ((BoxAdWebView)Element);
                LoadRequest(new NSUrlRequest(new NSUrl($"http://localhost:{webView.ServerPort}/{webView.URI}")));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            gbmBridge.OnReceiveScriptMessage(userController, message);
        }

        protected override void Dispose(bool disposing)
        {
            gbmBridge.OnDispose(disposing);

            if(disposing)
            {
                ((BoxAdWebView)Element).Clear();
            }

            base.Dispose(disposing);
        }
    }
}