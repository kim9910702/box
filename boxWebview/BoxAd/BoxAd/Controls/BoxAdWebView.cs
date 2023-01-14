using EmbedIO;
using EmbedIO.Files;
using BoxAd.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BoxAd.Controls
{
    public class BoxAdWebView : WebView
    {
        public static readonly BindableProperty URIProperty = BindableProperty.Create(
            propertyName: "URI",
            returnType: typeof(string),
            declaringType: typeof(BoxAdWebView),
            defaultValue: default(string));

        public static readonly BindableProperty ServerPortProperty = BindableProperty.Create(
            propertyName: "ServerPort",
            returnType: typeof(int),
            declaringType: typeof(BoxAdWebView),
            defaultValue: default(int));

        public static readonly BindableProperty WebRootProperty = BindableProperty.Create(
            propertyName: "WebRoot",
            returnType: typeof(string),
            declaringType: typeof(BoxAdWebView),
            defaultValue: default(string));

        private WebServer webServer;

        public string URI
        {
            get { return (string)GetValue(URIProperty);  }
            set { SetValue(URIProperty, value); }
        }

        public int ServerPort
        {
            get { return (int)GetValue(ServerPortProperty); }
            set { SetValue(ServerPortProperty, value); }
        }

        public string WebRoot
        {
            get { return (string)GetValue(WebRootProperty); }
            set { SetValue(WebRootProperty, value); }
        }

        public BoxAdWebView()
        {
        }

        public void Clear()
        {
        }

        public bool CreateWebServer()
        {
            IFileProvider fileProvider = DependencyService.Get<IAssetFileProvider>().Create(WebRoot);
            FileModule fileModule = new FileModule("/", fileProvider);

            webServer = new WebServer(HttpListenerMode.EmbedIO, $"http://*:{ServerPort}")
                .WithCors()
                .WithModule(null, fileModule, null);

            if (webServer != null)
            {
                webServer.RunAsync();

                webServer.StateChanged += OnWebServerStateChanged;
                return true;
            }

            return false;
        }

        public void OnWebServerStateChanged(object sender, WebServerStateChangedEventArgs e)
        {
            System.Console.WriteLine($"[WEBSERVER] STATE CHANGED - {e.NewState}");
        }
    }
}
