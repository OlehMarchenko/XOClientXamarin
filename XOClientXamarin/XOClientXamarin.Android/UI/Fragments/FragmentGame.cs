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

using FragmentManagerXO = XOClientXamarin.Droid.Api.FragmentManager;
using System.Threading;

namespace XOClientXamarin.Droid.UI.Fragments
{
    public class FragmentGame : Fragment
    {
        public bool init = false;
        private string playerTurn;
        private string unit;
        private Button[] buttons = null;
        private TextView tvStatusBar = null;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_game, container, false);

            buttons = new Button[9];
            buttons[0] = view.FindViewById<Button>(Resource.Id.fragment_game_b1);
            buttons[1] = view.FindViewById<Button>(Resource.Id.fragment_game_b2);
            buttons[2] = view.FindViewById<Button>(Resource.Id.fragment_game_b3);
            buttons[3] = view.FindViewById<Button>(Resource.Id.fragment_game_b4);
            buttons[4] = view.FindViewById<Button>(Resource.Id.fragment_game_b5);
            buttons[5] = view.FindViewById<Button>(Resource.Id.fragment_game_b6);
            buttons[6] = view.FindViewById<Button>(Resource.Id.fragment_game_b7);
            buttons[7] = view.FindViewById<Button>(Resource.Id.fragment_game_b8);
            buttons[8] = view.FindViewById<Button>(Resource.Id.fragment_game_b9);
            Button bExit = view.FindViewById<Button>(Resource.Id.fragment_game_bExit);
            tvStatusBar = view.FindViewById<TextView>(Resource.Id.fragment_game_tvStatusBar);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Tag = i + 1;
                buttons[i].Click += OnTileClick;
            }
            bExit.Click += delegate
            {
                ToLobby();
            };

            return view;
        }

        private void OnTileClick(object sender, EventArgs e)
        {
            if (!unit.Equals(""))
            {
                if (playerTurn.Equals("Turn"))
                {
                    //string json = new Gson().ToJson(new TTTPacket(playerTurn, unit, Convert.ToInt32((sender as Button).Tag), null, null), typeof(TTTPacket));
                    string json = TTTPacket.EncodeJson(new TTTPacket(playerTurn, unit, Convert.ToInt32((sender as Button).Tag), null, null));
                    json = json.Replace(",", ";");
                    FragmentManagerXO.Send("game:" + FragmentManagerXO.SessionKey + "," + json);
                }
            }
        }

        public void MessageBox(string message, string header)
        {
            AlertDialog.Builder aDialogBuilder = new AlertDialog.Builder(this.Context);
            aDialogBuilder.SetTitle(header);
            aDialogBuilder.SetMessage(message);

            aDialogBuilder.SetPositiveButton("OK", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                CleanUp();
                ToLobby();
                (sender as Dialog).Cancel();
            }));

            aDialogBuilder.SetCancelable(false);
            aDialogBuilder.Create();
            aDialogBuilder.Show();
        }

        private void ToLobby()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.main_activity_fragment_placeholder,
                    FragmentManagerXO.lobbyFrag);
            transaction.Commit();
        }

        private void CleanUp()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Text = "";
            }
            ChangeStatusBar(Resource.String.textview_statusbar.ToString());
        }

        public void GameTurn(string message)
        {
            //TTTPacket packet = new Gson().FromJson(message, typeof(TTTPacket));
            TTTPacket packet = TTTPacket.DecodeJson(message);

            if (!init)
            {
                Init(packet);
                init = true;
            }
            else
            {
                if (packet.GameResult != null)
                {
                    init = false;
                    RefreshGameField(packet);
                    string str = packet.GameResult;
                    FragmentManagerXO.activity.ExecuteOnUi(() => MessageBox(str, "Game over"));
                }
                else
                {
                    RefreshGameField(packet);
                }
            }
        }

        private void ChangeStatusBar(string text)
        {
            FragmentManagerXO.activity.ExecuteOnUi(() => tvStatusBar.Text = text);
        }

        private void Init(TTTPacket packet)
        {
            playerTurn = packet.PlayerTurn;
            unit = packet.Unit;
            ChangeStatusBar(unit + " | " + playerTurn);
        }

        private void RefreshGameField(TTTPacket packet)
        {
            FragmentManagerXO.activity.ExecuteOnUi(() => {
                string[] matrix = packet.Matrix;
                int i = 0;
                while (i < matrix.Length)
                {
                    buttons[i].Text = matrix[i];
                    i++;
                }
            });
            /*
            for (int i = 0; i < ; i++)
            {
                if (i < matrix.Length)
                {
                   
                }
            }*/
            playerTurn = packet.PlayerTurn;
            ChangeStatusBar(unit + " | " + playerTurn);
        }
    }
}