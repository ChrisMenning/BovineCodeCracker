using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BovineCodeCracker
{
    public class IntToSymbol
    {
        public IntToSymbol()
        {

        }

        public string convert(int i)
        {
            string symbol = string.Empty;

            switch (i)
            {
                case 0:
                    symbol = "🌮";
                    break;
                case 1:
                    symbol = "☾";
                    break;
                case 2:
                    symbol = "✯";
                    break;
                case 3:
                    symbol = "⚓";
                    break;
                case 4:
                    symbol = "❄";
                    break;
                case 5:
                    symbol = "⚕";
                    break;
                case 6:
                    symbol = "☘";
                    break;
                case 7:
                    symbol = "☠";
                    break;
                case 8:
                    symbol = "⚖";
                    break;
                case 9:
                    symbol = "♔";
                    break;
            }
            return symbol;
        }
    }
}
