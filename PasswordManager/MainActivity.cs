using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Essentials;

namespace PasswordManager
{
       
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        string bigLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string smallLetter = "abcdefghijklmnopqrstuvwxyz";
        string digit = "1234567890";
        string special = "!@#$%^&*()_+";
        string dict = "";

        EditText passwordIn;
        TextView passwordOut;
        Button cButton,schowekButton;
        CheckBox Duze, Male, Cyfry, Specjalne;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            cButton = FindViewById<Button>(Resource.Id.buttonKonwert);
            passwordOut = FindViewById<TextView>(Resource.Id.textPasswordOut);
            passwordIn = FindViewById<EditText>(Resource.Id.editTextPasswordIn);
            Duze = FindViewById<CheckBox>(Resource.Id.checkBoxDuze);
            Male = FindViewById<CheckBox>(Resource.Id.checkBoxMale);
            Cyfry = FindViewById<CheckBox>(Resource.Id.checkBoxCyfry);
            Specjalne = FindViewById<CheckBox>(Resource.Id.checkBoxSpecjalne);
            schowekButton = FindViewById<Button>(Resource.Id.buttonSchowek);
            
            // Odwołania do kontrolek GUI
            cButton.Click += CButton_Click;
            schowekButton.Click += SchowekButton_Click;
        }

        private void SchowekButton_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetTextAsync(passwordOut.Text);

            var toast = Toast.MakeText(Application.Context, "Dodano do schowka", ToastLength.Long);
            toast.Show();
            return;
        }

        private string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                int ind = data[i] % dict.Length;
                sBuilder.Append(dict[ind]);
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        private void CButton_Click(object sender, System.EventArgs e)
        {
            dict = "";
            if (Male.Checked) dict += smallLetter;
            if (Duze.Checked) dict += bigLetter;
            if (Cyfry.Checked) dict += digit;
            if (Specjalne.Checked) dict += special;
            if (dict.Length < 1)
            {
                var toast = Toast.MakeText(Application.Context, "Wybierz co najmniej jeden zbiór znaków", ToastLength.Long);
                toast.Show();
                return;
            }

            int i;
            string str = passwordIn.Text;
            SHA256 sha256Hash = SHA256.Create();
            string hash = GetHash(sha256Hash, str);
            passwordOut.Text = hash;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        
    }
}