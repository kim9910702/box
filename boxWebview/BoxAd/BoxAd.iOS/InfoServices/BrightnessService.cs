using Foundation;
using BoxAd.Interfaces;
using BoxAd.iOS.InfoServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(BrightnessService))]
namespace BoxAd.iOS.InfoServices
{
    public class BrightnessService : IBrightnessService
    {
        public float Get()
        {
            return (float)UIScreen.MainScreen.Brightness;
        }

        public bool Set(float brightness)
        {
            if (brightness < 0.0f)
                brightness = 0.0f;

            else if (brightness > 1.0f)
                brightness = 1.0f;

            UIScreen.MainScreen.Brightness = (nfloat)brightness;

            return true;
        }
    }
}