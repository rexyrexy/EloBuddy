using System;
using EloBuddy.SDK.Events;

namespace AutoCast
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            MenuInit.Initialize();
            Casts.Cast();
        }
    }
}
