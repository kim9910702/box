using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using GBManager.Controls;
using GBManager.Android.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GBMWebView), typeof(GBMWebViewRenderer))]
namespace GBManager.Android.Controls
{
    public class GBMWebViewRenderer : WebViewRenderer
    {
        private Context _context;

        public GBMWebViewRenderer(Context context) : base(context)
        {
            _context = Context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("GBM");
                ((GBMWebView)Element).Clear();
            }

            if(e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.DomStorageEnabled = true;
                Control.Settings.CacheMode = global::Android.Webkit.CacheModes.NoCache;

                Control.SetWebViewClient(new JavascriptWebViewClient(this, ""));
                Control.SetWebChromeClient(new WebChromeClient());

                Control.AddJavascriptInterface(new GBMBridge(this), "GBM");

                GBMWebView webView = ((GBMWebView)Element);
                Control.LoadUrl($"http://localhost:{webView.ServerPort}/{webView.URI}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                ((GBMWebView)Element).Clear();

            base.Dispose(disposing);
        }
    }
}