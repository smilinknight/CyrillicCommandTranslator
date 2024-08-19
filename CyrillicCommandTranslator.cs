using System;
using System.Collections.Generic;
using System.Linq;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;


namespace CyrillicCommandTranslator
{
    [ApiVersion(2, 1)]
    public class CyrillicCommandTranslator : TerrariaPlugin
    {
        private Dictionary<string, string> _keyMapping;

        public override string Name => "Cyrillic command translator";
        public override string Author => "Elly";
        public override string Description => "A plugin that translates commands that were input with cyrillic layout into latin layout";
        public override Version Version => new Version(1, 0);

        public CyrillicCommandTranslator(Main game) : base(game)
        {
            LoadKeyMapping();
        }

        private void LoadKeyMapping()
        {
            _keyMapping = new Dictionary<string, string>
            {
                { "Й", "Q" },
                { "Ц", "W" },
                { "У", "E" },
                { "К", "R" },
                { "Е", "T" },
                { "Н", "Y" },
                { "Г", "U" },
                { "Ш", "I" },
                { "Щ", "O" },
                { "З", "P" },

                { "Ф", "A" },
                { "Ы", "S" },
                { "В", "D" },
                { "А", "F" },
                { "П", "G" },
                { "Р", "H" },
                { "О", "J" },
                { "Л", "K" },
                { "Д", "L" },

                { "Я", "Z" },
                { "Ч", "X" },
                { "С", "C" },
                { "М", "V" },
                { "И", "B" },
                { "Т", "N" },
                { "Ь", "M" },
            };
        }

        public override void Initialize()
        {
            ServerApi.Hooks.ServerChat.Register(this, OnChat);
        }

        private void OnChat(ServerChatEventArgs args)


        {
            if (args.Text.StartsWith("/") || args.Text.StartsWith(".")) 
            {
                var translatedCommand = TranslateCommand(args.Text);
                if (translatedCommand != args.Text)
                {
                    // Log or notify the player about the translation
                    TShock.Players[args.Who].SendMessage($"Translated command: {translatedCommand}", Color.Yellow);

                    // Cancel the original command
                    args.Handled = true;

                    // Create a new command with the translated text
                    TShockAPI.Commands.HandleCommand(TShock.Players[args.Who], translatedCommand);
                }
            }
        }

        private string TranslateCommand(string command)
        {
            var translated = string.Empty;

            foreach (var character in command)
            {
                if (_keyMapping.ContainsKey(character.ToString().ToUpper()))
                {
                    translated += _keyMapping[character.ToString().ToUpper()];
                }
                else
                {
                    translated += character; // Keep the character if it is not in the mapping
                }
            }

            return translated;
        }
    }
}



