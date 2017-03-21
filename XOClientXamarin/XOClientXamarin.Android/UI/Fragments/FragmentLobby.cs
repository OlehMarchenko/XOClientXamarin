using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using XOClientXamarin.Droid.UI.Adapter;
using FragmentManagerXO = XOClientXamarin.Droid.Api.FragmentManager;

namespace XOClientXamarin.Droid.UI.Fragments
{
    public class FragmentLobby : Fragment
    {
        Button bLogin = null;
        Button bLogout = null;
        TextView tvName = null;
        TextView tvStatus = null;
        ListView lvFreePlayersList = null;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_lobby, container, false);

            bLogin = view.FindViewById<Button>(Resource.Id.fragment_lobby_bLogin);
            bLogout = view.FindViewById<Button>(Resource.Id.fragment_lobby_bLogout);
            tvName = view.FindViewById<TextView>(Resource.Id.fragment_lobby_tvStatusBar_name);
            tvStatus = view.FindViewById<TextView>(Resource.Id.fragment_lobby_tvStatusBar);

            lvFreePlayersList = view.FindViewById<ListView>(Resource.Id.fragment_lobby_lvFreePlayersList);

            AddToPlayerList();

            if (FragmentManagerXO.UserName != null)
            {
                if (!FragmentManagerXO.UserName.Equals(""))
                {
                    SetStatusBarData(FragmentManagerXO.UserName, "Connected to the server");
                }
            }

            bLogin.Click += delegate
            {
                if (FragmentManagerXO.UserName == null || FragmentManagerXO.UserName.Equals(""))
                {
                    FragmentManagerXO.activity.InitWebSocket();
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    transaction.Replace(Resource.Id.main_activity_fragment_placeholder, new FragmentRA());
                    transaction.Commit();
                }
            };

            bLogout.Click += delegate
            {
                if (FragmentManagerXO.UserName == null) return;
                if (FragmentManagerXO.UserName.Equals("")) return;
                FragmentManagerXO.SendLogout();
                FragmentManagerXO.ReInitialize();
                FragmentManagerXO.nameList = new string[0];
                AddToPlayerList();
                FragmentManagerXO.UserName = "";
                SetStatusBarData(FragmentManagerXO.UserName, "Waiting for connection to the server...");
            };

            return view;
        }

        public void AddToPlayerList()
        {
            List<string> nameList = new List<string>();
            for (int i = 0; i < FragmentManagerXO.nameList.Length; i++)
            {
                if (FragmentManagerXO.nameList[i] != null)
                {
                    if (!FragmentManagerXO.nameList[i].Equals(FragmentManagerXO.UserName))
                    {
                        nameList.Add(FragmentManagerXO.nameList[i]);
                    }
                }
            }

            AdapterXO adapter = new AdapterXO(this.Context, nameList);

            FragmentManagerXO.activity.ExecuteOnUi(() => lvFreePlayersList.Adapter = adapter);
        }

        public void Authorization(bool state)
        {
            if (state)
            {
                SetStatusBarData(FragmentManagerXO.UserName, "Connected to the server");
            }
            else
            {
                FragmentManagerXO.activity.ExecuteOnUi(() =>
                {
                    MessageBox("Incorrect login and/or password!", "Authorization issue");
                });
                FragmentManagerXO.UserName = "";
                SetStatusBarData(FragmentManagerXO.UserName, "Waiting for connection to the server...");
            }
        }

        public void Registration(bool state)
        {
            if (state)
            {
                SetStatusBarData(FragmentManagerXO.UserName, "Connected to the server");
            }
            else
            {
                FragmentManagerXO.activity.ExecuteOnUi(() =>
                {
                    MessageBox("Login already exists!", "Registration issue");
                });
                FragmentManagerXO.UserName = "";
                SetStatusBarData(FragmentManagerXO.UserName, "Waiting for connection to the server...");
            }
        }

        private void SetStatusBarData(string name, string status)
        {
            FragmentManagerXO.activity.ExecuteOnUi(() =>
            {
                tvName.Text = "Your name: " + name;
                tvStatus.Text = status;
            });
        }

        public void MessageBox(string message, string header)
        {
            AlertDialog.Builder aDialogBuilder = new AlertDialog.Builder(this.Context);
            aDialogBuilder.SetTitle(header);
            aDialogBuilder.SetMessage(message);

            aDialogBuilder.SetPositiveButton("OK", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                (sender as Dialog).Cancel();
            }));

            FragmentManagerXO.activity.ExecuteOnUi(() => 
            {
                aDialogBuilder.SetCancelable(false);
                aDialogBuilder.Create();
                aDialogBuilder.Show();
            });
        }

        public void InvitationBox(string playerName)
        {
            AlertDialog.Builder aDialogBuilder = new AlertDialog.Builder(this.Context);
            aDialogBuilder.SetTitle("Invitation");
            aDialogBuilder.SetMessage("Player " + playerName + " wants to play with you!");

            aDialogBuilder.SetPositiveButton("OK", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                InvitationFeedBack(playerName, true);
                (sender as Dialog).Cancel();
            }));

            aDialogBuilder.SetNegativeButton("Cancel", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                InvitationFeedBack(playerName, false);
                (sender as Dialog).Cancel();
            }));

            FragmentManagerXO.activity.ExecuteOnUi(() =>
            {
                aDialogBuilder.SetCancelable(false);
                aDialogBuilder.Create();
                aDialogBuilder.Show();
            });
        }

        private void InvitationFeedBack(string playerName, bool result)
        {
            string msg;
            if (result)
            {
                msg = "inviteYes:";
            }
            else
            {
                msg = "inviteNo:";
            }
            msg += FragmentManagerXO.UserName + "," + playerName;
            FragmentManagerXO.SendInviteResponse(msg);
        }

    }
}