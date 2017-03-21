using System;
using WebSocket4Net;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

using Java.Net;

using XOClientXamarin.Droid.Api;
using FragmentManagerXO = XOClientXamarin.Droid.Api.FragmentManager;
using System.Threading;

namespace XOClientXamarin.Droid
{
    [Activity(Label = "XOClientXamarin.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private WebSocket webSocket;
        public void ExecuteOnUi(Action function)
        {
            lock (this)
            {
                RunOnUiThread(function);
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FragmentManagerXO.SetActivity(this);
            FragmentManagerXO.SetUiThread(Thread.CurrentThread);

            SetContentView(Resource.Layout.Main);

            InitWebSocket();

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.main_activity_fragment_placeholder, FragmentManagerXO.regFrag);
            transaction.Commit();
        }

        public void InitWebSocket()
        {
            webSocket = new WebSocket("ws://10.0.3.2:8080/");
            webSocket.Open();
            webSocket.MessageReceived += WebSocket_MessageReceived;
            FragmentManagerXO.InitWebSocket(webSocket);
        }

        private void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Commander.ListenLoop(e.Message);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            FragmentManagerXO.SendLogout();
        }
    }
}


