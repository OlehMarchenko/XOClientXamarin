using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XOClientXamarin.Droid.Api
{
    public class Commander
    {
        public static void ListenLoop(string message)
        {
            if (message.StartsWith("{"))
            {
                message = message.Replace(";", ",");
                FragmentManager.gameFrag.GameTurn(message);
                return;
            }

            string command = message.Substring(0, message.IndexOf(":"));
            message = message.Replace("\r\n", "");
            message = message.Substring(message.IndexOf(":") + 1);
            string[] args = message.Split(',');

            switch (command)
            {
                case "broadcast":
                    {
                        FragmentManager.nameList = args;
                        FragmentManager.lobbyFrag.AddToPlayerList();
                        break;
                    }
                case "invite":
                    {
                        FragmentManager.lobbyFrag.InvitationBox(args[0]);
                        break;
                    }
                case "success":
                    {
                        if (args[0].Equals("yes"))
                        {
                            FragmentManager.SessionKey = args[1];
                            FragmentManager.ShowGame(FragmentManager.lobbyFrag.FragmentManager.BeginTransaction());
                        }
                        else
                        {
                            FragmentManager.lobbyFrag.MessageBox("Connection denied", "Invitation issue");
                        }
                        break;
                    }
                case "auth":
                    {
                        if (args[0].Equals("yes"))
                        {
                            FragmentManager.lobbyFrag.Authorization(true);
                        }
                        else
                        {
                            FragmentManager.lobbyFrag.Authorization(false);
                        }
                        break;
                    }
                case "reg":
                    {
                        if (args[0].Equals("yes"))
                        {
                            FragmentManager.lobbyFrag.Registration(true);
                        }
                        else
                        {
                            FragmentManager.lobbyFrag.Registration(false);
                        }
                        break;
                    }
                case "game":
                    {
                        if (args[0].Equals("quit"))
                        {
                            FragmentManager.lobbyFrag.MessageBox("Player left the game. You win!", "Game over");
                            FragmentManager.gameFrag.init = false;
                        }
                        break;
                    }
                case "changepass":
                    {
                        string msg = null;
                        if (args[0].Equals("yes"))
                        {
                            msg = "Password successfully changed!";
                        }
                        else
                        {
                            msg = "Invalid login or/and password";
                        }
                        FragmentManager.lobbyFrag.MessageBox(msg, "Password change");
                        break;
                    }
            }
        }
    }
}