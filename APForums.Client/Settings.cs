using APForums.Client.Data;
using APForums.Client.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APForums.Client
{
    public class Settings
    {

        public static AuthInfo authInfo = null;

        public static User userInfo = null;

        public static void UpdateUserInfo(User user)
        {
            userInfo = user;

            OnUserInfoUpdate?.Invoke();
        }

        public static void UpdateUserInfo()
        {
            OnUserInfoUpdate?.Invoke();
        }

        public static IDisposable SubscribeToUserInfoUpdate(Action handler)
        {
            OnUserInfoUpdate += handler;

            return new DisposableAction(() => OnUserInfoUpdate -= handler);
        }

        public static event Action OnUserInfoUpdate;

        public static string PrimaryColor = "Blue";
        public static string SecondaryColor = "Black";

        public static Dictionary<string, ColorSet> SelectablePrimaryColors = new()
        {
            {"Purple", new("147 51 234", "255 255 255")},
            {"Indigo", new("79 70 229", "255 255 255")},
            {"Fuchsia", new("192 38 211", "255 255 255")},
            {"Rose", new("225 29 72", "255 255 255")},
            {"Sky", new("2 132 199", "255 255 255")},
            {"Cyan", new("8 145 178", "255 255 255")},
            {"Blue", new("37 99 235", "255 255 255")},
            {"Green", new("34 197 94", "255 255 255")},
            {"Emerald", new("5 150 105", "255 255 255")},
            {"Orange", new("234 88 12", "255 255 255")},
            {"Red", new("220 38 38", "255 255 255")},
            {"Grey", new("75 85 99", "255 255 255")},
            {"Stone", new("88 83 78", "255 255 255")},
            {"Black", new("0 0 0", "255 255 255")},
        };

        public static Dictionary<string, ColorSet> SelectableSecondaryColors = new()
        {
            {"Purple", new("147 51 234", "255 255 255")},
            {"Indigo", new("79 70 229", "255 255 255")},
            {"Fuchsia", new("192 38 211", "255 255 255")},
            {"Rose", new("225 29 72", "255 255 255")},
            {"Sky", new("2 132 199", "255 255 255")},
            {"Cyan", new("8 145 178", "255 255 255")},
            {"Blue", new("37 99 235", "255 255 255")},
            {"Green", new("34 197 94", "255 255 255")},
            {"Emerald", new("5 150 105", "255 255 255")},
            {"Orange", new("234 88 12", "255 255 255")},
            {"Red", new("220 38 38", "255 255 255")},
            {"Grey", new("75 85 99", "255 255 255")},
            {"Stone", new("88 83 78", "255 255 255")},
            {"Black", new("0 0 0", "255 255 255")},
        };

        public static void CacheColors()
        {
            Preferences.Default.Set(nameof(PrimaryColor), PrimaryColor);
            Preferences.Default.Set(nameof(SecondaryColor), SecondaryColor);
        }

        public static void ClearColorCache()
        {
            Preferences.Remove(nameof(PrimaryColor));
            Preferences.Remove(nameof(SecondaryColor));
        }

        public static bool InitiateColors()
        {
            var Primary = Preferences.Default.Get(nameof(PrimaryColor), PrimaryColor);
            var Secondary = Preferences.Default.Get(nameof(SecondaryColor), SecondaryColor);
            if (Primary.Equals(PrimaryColor) && Secondary.Equals(SecondaryColor))
            {
                return false;
            } else
            {
                PrimaryColor = Primary;
                SecondaryColor = Secondary;
                return true;
            }
        }

    }

    public class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
    
    public class ColorSet
    {

        public ColorSet(string color, string text)
        {
            RGBColor = color;
            RGBText = text;
        }

        public string RGBColor { get; set; }

        public string RGBText { get; set; }
    }
}
