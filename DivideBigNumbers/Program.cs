using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtMath;

namespace DivideBigNumbers
{
	class Program
	{
		static void Main(string[] args)
		{
			//if (args.Length != 3)
			//{
			//	Console.WriteLine("usage: DivideBigNumbers Nominator Denominator);
			//}
			//else
			//{
			//	Console.WriteLine(Divide(args[1], args[2]);
			//}
			char desimalTegn = ',';
			bool okDesimal = false;
			while (!okDesimal)
			{
				Console.Write("Desimaltegn: (,)");
				var tegn = Console.ReadKey();
				if (tegn.Key != ConsoleKey.Enter)
				{
					if (tegn.KeyChar == ',' || tegn.KeyChar == '.')
					{
						desimalTegn = tegn.KeyChar;
						okDesimal = true;
					}
					else
					{
						Console.WriteLine();
						Console.WriteLine("Ugyldig desimaltegn");
					}
				}
				else
				{
					okDesimal = true;
				}
			}

			string teller = string.Empty;
			string nevner = string.Empty;
			Console.WriteLine();
			teller = ReadAndCheck("Teller");
			do
			{
				if (teller != string.Empty)
				{
					nevner = ReadAndCheck("Nevner");
				}
				else
				{
					break;
				}
				ExtendedMath.Divide(teller, nevner, desimalTegn, 50);
				Console.WriteLine();
				teller = ReadAndCheck("Teller");
			}
			while (teller != string.Empty);
		}

		static string ReadAndCheck(string label)
		{
			string verdi;
			bool b;
			decimal result;
			do
			{
				Console.Write($"{label}: ");
				verdi = Console.ReadLine();

				b = decimal.TryParse(verdi, out result);
				if (!b)
				{
					Console.WriteLine("Verdien er ikke numerisk");
				}
			} while (verdi != string.Empty && !b);
			return verdi;
		}

	}
}
