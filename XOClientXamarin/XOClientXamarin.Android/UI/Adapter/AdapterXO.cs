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
using Java.Lang;

using FragmentManagerXO = XOClientXamarin.Droid.Api.FragmentManager;

namespace XOClientXamarin.Droid.UI.Adapter
{
    public class AdapterXO : BaseAdapter
    {
        private Context context;
        private LayoutInflater layoutInflater;
        private List<string> nameList;
        public override int Count
        {
            get
            {
                return nameList.Count;
            }
        }


        public AdapterXO(Context context, List<string> nameList)
        {
            if (context == null)
                this.context = FragmentManagerXO.lobbyContext;
            else
            {
                FragmentManagerXO.lobbyContext = context;
                this.context = context;
            }
            this.layoutInflater = (LayoutInflater)this.context.GetSystemService(Context.LayoutInflaterService);
            this.nameList = nameList;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = layoutInflater.Inflate(Resource.Layout.lobby_list_item, parent, false);
            }

            TextView tvName = view.FindViewById<TextView>(Resource.Id.lobby_list_item_tvName);
            tvName.Text = nameList[position];

            tvName.LongClick += delegate
            {
                InvitationBox(tvName.Text);
            };

            return view;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            Java.Lang.Object obj = nameList[position];
            return obj;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        private void InvitationBox(string playerName)
        {
            AlertDialog.Builder aDialogBuilder = new AlertDialog.Builder(context);
            aDialogBuilder.SetTitle("Invitation");
            aDialogBuilder.SetMessage("Do you want to play with " + playerName + "?");

            aDialogBuilder.SetPositiveButton("Yes", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                Invite(playerName);
                (sender as Dialog).Cancel();
            }));

            aDialogBuilder.SetNegativeButton("No", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                (sender as Dialog).Cancel();
            }));

            aDialogBuilder.SetCancelable(false);
            aDialogBuilder.Create();
            aDialogBuilder.Show();
        }

        private void Invite(string playerName)
        {
            FragmentManagerXO.SendInviteRequest(playerName);
        }

    }
}