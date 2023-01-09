using Android.Webkit;
using Xamarin.Forms.Platform.Android;

namespace GBManager.Android.Controls
{
    public class JavascriptWebViewClient : FormsWebViewClient
    {
        string _initial_js;

        public JavascriptWebViewClient(GBMWebViewRenderer renderer, string js) : base(renderer)
        {
            _initial_js = js;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);

            if(_initial_js.Length > 0)
                view.EvaluateJavascript(_initial_js, null);
        }
    }
}
