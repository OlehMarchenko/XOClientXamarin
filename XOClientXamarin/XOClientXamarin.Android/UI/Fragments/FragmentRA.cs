using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using FragmentManagerXO = XOClientXamarin.Droid.Api.FragmentManager;

namespace XOClientXamarin.Droid.UI.Fragments
{
    public class FragmentRA : Fragment
    {
        private string email;
        EditText etUsername;
        EditText etPassword;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_ra, container, false);

            Button bAuth = view.FindViewById<Button>(Resource.Id.fragment_ra_bAuth);
            Button bReg = view.FindViewById<Button>(Resource.Id.fragment_ra_bReg);
            Button bForgotPass = view.FindViewById<Button>(Resource.Id.fragment_ra_bForgotPass);
            Button bChangePass = view.FindViewById<Button>(Resource.Id.fragment_ra_bChangePass);

            etUsername = view.FindViewById<EditText>(Resource.Id.fragment_ra_etUsername);
            etPassword = view.FindViewById<EditText>(Resource.Id.fragment_ra_etPassword);

            bAuth.Click += delegate
            {
                FragmentManagerXO.UserName = etUsername.Text;
                FragmentManagerXO.SendLogin(etUsername.Text, etPassword.Text);
                ToLobby();
            };

            bReg.Click += delegate
            {
                FragmentManagerXO.UserName = etUsername.Text;
                FragmentManagerXO.SendReg(etUsername.Text, etPassword.Text);
                ToLobby();
            };

            bForgotPass.Click += delegate
            {
                FragmentManagerXO.UserName = etUsername.Text;
                OnForgotPass();
            };

            bChangePass.Click += delegate
            {
                OpenChangePassDialog();
                ToLobby();
            };

            return view;
        }


        private void MessageBox(String message, String header) {
            AlertDialog.Builder aDialogBuilder = new AlertDialog.Builder(this.Context);
            aDialogBuilder.SetTitle(header);
            aDialogBuilder.SetMessage(message);

            aDialogBuilder.SetPositiveButton("OK", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                (sender as Dialog).Cancel();
            }));

            aDialogBuilder.SetCancelable(false);
            aDialogBuilder.Create();
            aDialogBuilder.Show();
        }

        private void ToLobby() {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.main_activity_fragment_placeholder, FragmentManagerXO.lobbyFrag);
            transaction.Commit();
        }

        private void OnForgotPass() {
            if (!FragmentManagerXO.UserName.Equals("")) {
                OpenPassRetrievalDialog();
            }
            else {
                MessageBox("You should insert your login first!", "Password retrieval issue");
                ToLobby();
            }
        }

        private void OpenPassRetrievalDialog(){
            AlertDialog.Builder aDialogBuilder = new AlertDialog.Builder(this.Context);

            EditText etEmail = new EditText(this.Context);
            aDialogBuilder.SetView(etEmail);

            aDialogBuilder.SetTitle("Password retrival");
            aDialogBuilder.SetMessage("Enter email to retrieve your password:");

            aDialogBuilder.SetPositiveButton("OK", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                email = etEmail.Text;
                if (new Regex("^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$").IsMatch(email))
                {
                    FragmentManagerXO.SendForgotPassword(FragmentManagerXO.UserName, email);
                }
                else
                {
                    MessageBox("Incorrect e-mail address!", "Password retrieval issue");
                }
                ToLobby();
                (sender as Dialog).Cancel();
            }));

            aDialogBuilder.SetCancelable(false);
            aDialogBuilder.Create();
            aDialogBuilder.Show();
        }

        private void OpenChangePassDialog()
        {
            LayoutInflater inflater = (LayoutInflater)this.Context.GetSystemService(Context.LayoutInflaterService);
            View v = inflater.Inflate(Resource.Layout.dialog_change_password, null);

            EditText username = v.FindViewById<EditText>(Resource.Id.dialog_changePass_etUsername);
            EditText pass = v.FindViewById<EditText>(Resource.Id.dialog_changePass_etPassword);
            EditText newPass = v.FindViewById<EditText>(Resource.Id.dialog_changePass_etNewPassword);

            AlertDialog.Builder aDialogBuilder = new AlertDialog.Builder(this.Context);
            aDialogBuilder.SetView(v);
            aDialogBuilder.SetMessage("Change password");

            aDialogBuilder.SetPositiveButton("Confirm", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                FragmentManagerXO.SendChangePassword(username.Text, pass.Text, newPass.Text);
                (sender as Dialog).Cancel();
            }));

            aDialogBuilder.SetNegativeButton("Cancel", new EventHandler<DialogClickEventArgs>((sender, e) =>
            {
                (sender as Dialog).Cancel();
            }));
            
            aDialogBuilder.Show();
        }

    }
}