﻿using Dt.Core;
using System;
using UIKit;

namespace Dt.Sample
{
    public class Application
    {
        static void Main(string[] args)
        {
            try
            {
                UIApplication.Main(args, null, typeof(App));
            }
            catch (Exception ex)
            {
                Kit.OnIOSUnhandledException(ex);
            }
        }
    }
}