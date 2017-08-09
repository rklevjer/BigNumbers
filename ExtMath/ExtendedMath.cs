using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtMath
{
	public static class ExtendedMath
	{
		public static void Divide(string teller, string nevner, char decimalSeparator, int maxDigits)
		{
			int sifferEtterKommaTeller;
			int sifferEtterKommaNevner;

			int digit;
			int[] multiplied;

			bool commaShallBeWritten = false;

			int tellerD = teller.IndexOf(decimalSeparator);
			int nevnerD = nevner.IndexOf(decimalSeparator);
			if (tellerD == -1)
			{
				tellerD = 0;
				sifferEtterKommaTeller = 0;
			}
			else
			{
				sifferEtterKommaTeller = teller.Length - tellerD - 1;
			}
			if (nevnerD == -1)
			{
				nevnerD = 0;
				sifferEtterKommaNevner = 0;
			}
			else
			{
				sifferEtterKommaNevner = nevner.Length - nevnerD - 1;
			}

			Console.Write($"{teller} / {nevner} = ");
			if (sifferEtterKommaTeller != sifferEtterKommaNevner)
			{
				if (sifferEtterKommaTeller > sifferEtterKommaNevner)
				{
					nevner += new string('0', sifferEtterKommaTeller - sifferEtterKommaNevner);
				}
				else
				{
					teller += new string('0', sifferEtterKommaNevner - sifferEtterKommaTeller);
				}
			}

			teller = teller.Replace(decimalSeparator.ToString(), "");
			nevner = nevner.Replace(decimalSeparator.ToString(), "");
			int[] tellerArray = GetDigits(teller);
			int[] nevnerArray = Strip(GetDigits(nevner));
			int written = 0;

			int[] restArray = new int[Math.Min(tellerArray.Length, nevnerArray.Length)];
			Array.Copy(tellerArray, 0, restArray, 0, restArray.Length);

			int tellerIndex = nevnerArray.Length;
			bool commaWritten = false;
			for (int i = 0; i < maxDigits; i++)
			{
				digit = ExtendedMath.GetDigit(restArray, nevnerArray);

				if (written > 0 || digit > 0 || i >= tellerArray.Length - nevnerArray.Length && !commaWritten)
				{
					Console.Write(digit);
					if (i >= tellerArray.Length - nevnerArray.Length && !commaWritten)
					{
						commaShallBeWritten = true;
						commaWritten = true;
					}
					written++;
				}

				multiplied = DigitArrayMultiplyByFactor(nevnerArray, digit);
				restArray = Subtract(restArray, multiplied);
				restArray = Strip(restArray);
				if (restArray.Length == 0)
				{
					if (i >= Math.Abs(tellerArray.Length - nevnerArray.Length))
					{
						break;
					}
				}
				if (commaShallBeWritten)
				{
					Console.Write(",");
					commaShallBeWritten = false;
				}
				restArray = ExtendedMath.AddRight(restArray);
				restArray[restArray.Length - 1] = tellerArray.Length > tellerIndex ? tellerArray[tellerIndex] : 0;
				tellerIndex++;
			}
		}

		private static int[] DigitArrayMultiplyByFactor(int[] digitArray, int factor)
		{
			int[] resultArray = new int[digitArray.Length];
			digitArray.CopyTo(resultArray, 0);
			var reminder = 0;
			for (int i = resultArray.Length - 1; i >= 0; i--)
			{
				var temp = resultArray[i] * factor + reminder;
				var lastDigit = temp % 10;
				reminder = (temp - lastDigit) / 10;
				resultArray[i] = lastDigit;
			}
			if (reminder == 0)
			{
				return resultArray;
			}
			else
			{
				int[] increasedArray = new int[resultArray.Length + 1];
				resultArray.CopyTo(increasedArray, 1);
				increasedArray[0] = reminder;
				return increasedArray;
			}
		}
		private static int DigitComparer(int[] tellerSiffer, int[] nevnerSiffer)
		{
			if (tellerSiffer.Length > nevnerSiffer.Length)
			{
				return 1;  // teller større enn nevner
			}
			else if (tellerSiffer.Length < nevnerSiffer.Length)
			{
				return -1;
			}
			else
			{
				for (int i = 0; i < tellerSiffer.Length; i++)
				{
					if (tellerSiffer[i] > nevnerSiffer[i])
					{
						return 1;
					}
					else if (tellerSiffer[i] < nevnerSiffer[i])
					{
						return -1;
					}
				}
				return 0;
			}
		}

		private static int[] RemoveALeadingZero(int[] intArray)
		{
			int j = -1;
			if (intArray[0] == 0)
			{
				j = 0;
			}
			int[] strippedArray = new int[intArray.Length - j - 1];
			for (int i = 0; i < strippedArray.Length; i++)
			{
				strippedArray[i] = intArray[++j];
			}
			return strippedArray;
		}

		private static int[] Strip(int[] intArray)
		{
			int j = -1;
			for (int i = 0; i < intArray.Length && intArray[i] == 0; i++)
			{
				j = i;
			}
			int[] strippedArray = new int[intArray.Length - j - 1];
			for (int i = 0; i < strippedArray.Length; i++)
			{
				strippedArray[i] = intArray[++j];
			}
			return strippedArray;
		}

		private static int GetLargestFactor(int[] restSiffer, int[] nevnerSiffer)
		{
			int[] toBeMultipliedArray = new int[nevnerSiffer.Length];
			int[] multipliedArray = new int[nevnerSiffer.Length];

			for (int i = 1; i < 10; i++)
			{
				nevnerSiffer.CopyTo(toBeMultipliedArray, 0);
				multipliedArray = DigitArrayMultiplyByFactor(toBeMultipliedArray, i);
				if (DigitComparer(restSiffer, multipliedArray) < 0)
				{
					return i - 1;
				}
				else if (DigitComparer(restSiffer, multipliedArray) == 0)
				{
					return i;
				}
			}
			return 9;
		}

		private static int[] Subtract(int[] restSiffer, int[] toSubtract)
		{
			if (restSiffer.Length < toSubtract.Length)
			{
				return restSiffer;
			}
			while (restSiffer.Length > toSubtract.Length)
			{
				toSubtract = AddLeft(toSubtract);
			}
			var j = restSiffer.Length;
			for (int i = toSubtract.Length - 1; i >= 0; i--)
			{
				if (restSiffer[i] >= toSubtract[i])
				{
					restSiffer[i] = restSiffer[i] - toSubtract[i];
				}
				else
				{
					restSiffer[i] = restSiffer[i] + 10 - toSubtract[i];
					restSiffer[i - 1]--;
					var k = i - 1;
					while (restSiffer[k] < 0 && k > 0)
					{
						restSiffer[k] = 10 + restSiffer[k];
						restSiffer[k - 1]--;
						k--;
					}
				}
			}

			return restSiffer;
		}

		private static int[] AddRight(int[] anArray)
		{
			int size = anArray.Length;
			int[] increasedArray = new int[size + 1];
			anArray.CopyTo(increasedArray, 0);
			return increasedArray;
		}

		private static int[] AddLeft(int[] anArray)
		{
			int size = anArray.Length;
			int[] increasedArray = new int[size + 1];
			anArray.CopyTo(increasedArray, 1);
			return increasedArray;
		}

		private static int GetDigit(int[] teller, int[] nevner)
		{
			if (teller.Count() < nevner.Count())
			{
				return 0;
			}
			else
			{
				return GetLargestFactor(teller, nevner);
			}
		}

		private static int[] GetDigits(string tall)
		{
			int[] siffer = new int[tall.Length];
			for (int i = 0; i < tall.Length; i++)
			{
				siffer[i] = Convert.ToInt32(tall.Substring(i, 1));
			}
			return siffer;
		}

		private static bool IsZero(int[] siffer)
		{
			for (int i = 0; i < siffer.Length; i++)
			{
				if (siffer[i] != 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}

