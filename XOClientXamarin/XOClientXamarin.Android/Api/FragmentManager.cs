using Android;
using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOClientXamarin.Droid.UI.Fragments;
using WebSocket4Net;
using System.Threading;

namespace XOClientXamarin.Droid.Api
{
    public class FragmentManager
    {
        public static WebSocket ws;
        public static FragmentLobby lobbyFrag;
        public static FragmentRA regFrag;
        public static FragmentGame gameFrag;
        public static string UserName = null;
        public static string SessionKey = null;
        public static Context lobbyContext = null;
        public static string[] nameList = new string[1];
        public static MainActivity activity;
        public static Thread UiThread;


        public static void SetActivity(MainActivity activity)
        {
            FragmentManager.activity = activity;
        }

        public static void SetUiThread(Thread thread)
        {
            UiThread = thread;
        }
        static FragmentManager()
        {
            lobbyFrag = new FragmentLobby();
            regFrag = new FragmentRA();
            gameFrag = new FragmentGame();
            nameList[0] = null;
        }

        public static void ReInitialize()
        {
            lobbyFrag = new FragmentLobby();
            regFrag = new FragmentRA();
            gameFrag = new FragmentGame();
            UserName = null;
            SessionKey = null;
            lobbyContext = null;
            nameList = new string[1];
            nameList[0] = null;
        }

        public static void ShowLobby(FragmentTransaction transaction)
        {
            transaction.Replace(Resource.Id.main_activity_fragment_placeholder, lobbyFrag);
            transaction.Commit();
        }

        public static void ShowGame(FragmentTransaction transaction)
        {
            transaction.Replace(Resource.Id.main_activity_fragment_placeholder, gameFrag);
            transaction.Commit();
        }

        public static void ShowRegAuth(FragmentTransaction transaction)
        {
            transaction.Replace(Resource.Id.main_activity_fragment_placeholder, regFrag);
            transaction.Commit();
        }

        public static void InitWebSocket(WebSocket webSocket)
        {
            ws = webSocket;
        }

        public static void Send(string message)
        {
            ws.Send(message);
        }

        public static void SendLogin(string name, string pass)
        {
            Send("auth:" + name + "," + pass);
        }

        public static void SendReg(string name, string pass)
        {
            Send("reg:" + name + "," + pass);
        }

        public static void SendLogout()
        {
            Send("logout:");
        }

        public static void SendChangePassword(string login, string password, string newPassword)
        {
            Send("changepass:" + login + "," + password + "," + newPassword);
        }

        public static void SendForgotPassword(string username, string email)
        {
            Send("forgotpass:" + username + "," + email);
        }

        public static void SendInviteResponse(string response)
        {
            Send(response);
        }

        public static void SendInviteRequest(string name)
        {
            Send("invite:" + name);
        }
    }
}
