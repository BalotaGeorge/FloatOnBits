using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRepresentation
{
    class Program
    {
        static void Main(string[] args)
        {
            float Number = float.MaxValue;
            Console.Write("Enter a number: ");
            do
            {
                try { Number = Convert.ToSingle(Console.ReadLine()); }
                catch (FormatException) { Console.Write("Enter a actual number. Try again: "); }
                catch (OverflowException) { Console.Write("Enter a smaller number. Try again: "); }
                if (Number != float.MaxValue && Converter.Convert(Number.ToString(), 10, 2) == "") 
                {
                    Console.Write("Enter aa smaller number. Try again: "); 
                    Number = float.MaxValue;
                }
            } while (Number == float.MaxValue);
            char[] Binary = Converter.Convert(Number.ToString(), 10, 2).ToCharArray();
            int[] Representation = new int[32];
            int comma = 0;
            for (int i = 0; i < Binary.Length; i++)
            {
                if (i == 0) Representation[i] = Binary[i] == '-' ? 1 : 0;
                if (Binary[i] == '.')
                {
                    comma = i;
                    for (int j = comma; j >= 2; j--)
                    {
                        char temp = Binary[j - 1];
                        Binary[j - 1] = Binary[j];
                        Binary[j] = temp;
                    }
                    comma--;
                }
            }
            //Console.WriteLine(comma);
            string Exponent = comma != 0 ? Converter.Convert((comma + 127).ToString(), 10, 2) : "";
            for (int i = Exponent.Length - 1, j = 8; i >= 0; i--, j--) Representation[j] = Exponent[i] == '1' ? 1 : 0;
            for (int i = 2, j = 9; i < Binary.Length; i++, j++) Representation[j] = Binary[i] == '1' ? 1 : 0;
            for (int i = 0; i < 32; i++)
            {
                if (i == 0 || i == 8) Console.Write(Representation[i] + " ");
                else Console.Write(Representation[i]);
            }
            Console.ReadLine();
        }
    }
}
