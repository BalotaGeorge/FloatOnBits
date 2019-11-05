using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRepresentation
{
    class Converter
    {
        static private string sInitialNumber, sFinalNumber;
        static private int nInitialBase, nFinalBase, nSmallestPosBase, nCommaPlace;
        static private bool bNegativeMark, bAllGood;
        static public string Convert(string InputNumber, int InitialBase, int FinalBase)
        {
            sInitialNumber = InputNumber;
            nInitialBase = InitialBase;
            nFinalBase = FinalBase;
            ReadAndCheckNumber();
            if (nInitialBase < nSmallestPosBase || nInitialBase > 16) bAllGood = false;
            if (nFinalBase < 2 || nFinalBase > 16) bAllGood = false;
            if (nCommaPlace != -1)
            {
                string sBeforeCommaNumber = sInitialNumber.Substring(0, nCommaPlace);
                string sAfterCommaNumber = sInitialNumber.Substring(nCommaPlace + 1);
                string sFirstHalf = ConvertDecimalToBase(ConvertBaseToDecimal(sBeforeCommaNumber, nInitialBase), nFinalBase);
                string sSecondHalf = ConvertDecimalToCommaBase(ConvertCommaBaseToDecimal(sAfterCommaNumber, nInitialBase), nFinalBase);
                if (sFirstHalf.Length == 0) sFirstHalf = "0";
                if (bNegativeMark) sFinalNumber = "-" + sFirstHalf + "." + sSecondHalf;
                else sFinalNumber = sFirstHalf + "." + sSecondHalf;
            }
            else
            {
                sFinalNumber += bNegativeMark ? "-" : "";
                sFinalNumber += ConvertDecimalToBase(ConvertBaseToDecimal(sInitialNumber, nInitialBase), nFinalBase);
                if (sFinalNumber.Length == 0) sFinalNumber = "0";
            }
            if (bAllGood) return sFinalNumber;
            else return "";
        }
        static private void ReadAndCheckNumber()
        {
            nSmallestPosBase = 2;
            sFinalNumber = "";
            bAllGood = true;
            nCommaPlace = -1;
            for (int i = 0; i < sInitialNumber.Length; i++)
            {
                char t = sInitialNumber[i];
                if (!((t >= '0' && t <= '9') || (t >= 'A' && t <= 'F') || (t >= 'a' && t <= 'f') || t == '.' || t == ',')) bAllGood = false;
                else
                {
                    if ((t == '.' || t == ',') && i < sInitialNumber.Length - 1) nCommaPlace = i;
                    else
                    {
                        if (t >= '0' && t <= '9' && nSmallestPosBase < t - 48 + 1) nSmallestPosBase = t - 48 + 1;
                        if (t >= 'A' && t <= 'F' && nSmallestPosBase < t - 55 + 1) nSmallestPosBase = t - 55 + 1;
                        if (t >= 'a' && t <= 'f' && nSmallestPosBase < t - 87 + 1) nSmallestPosBase = t - 87 + 1;
                    }
                }
                if (i == 0 && t == '-')
                {
                    bAllGood = true;
                    bNegativeMark = true;
                }
            }
        }
        static private int ConvertBaseToDecimal(string Number, int Base)
        {
            int n = 0;
            for (int i = 0; i < Number.Length; i++)
            {
                if (Number[i] >= '0' && Number[i] <= '9')
                {
                    try { checked { n += (Number[i] - 48) * (int)Math.Pow(Base, Number.Length - i - 1); } }
                    catch (OverflowException) { bAllGood = false; }
                }
                else if (Number[i] >= 'A' && Number[i] <= 'Z')
                {
                    try { checked { n += (Number[i] - 55) * (int)Math.Pow(Base, Number.Length - i - 1); } }
                    catch (OverflowException) { bAllGood = false; }
                }
                else if (Number[i] >= 'a' && Number[i] <= 'z')
                {
                    try { checked { n += (Number[i] - 87) * (int)Math.Pow(Base, Number.Length - i - 1); } }
                    catch (OverflowException) { bAllGood = false; }
                }
            }
            return n;
        }
        static private string ConvertDecimalToBase(int Number, int Base)
        {
            string ret = "";
            char[] sym = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            int aux = Number, k = 0;
            while (aux != 0) { k++; aux /= Base; }
            int[] digits = new int[k];
            int index = 0;
            while (Number != 0)
            {
                digits[index++] = Number % Base;
                Number /= Base;
            }
            for (int i = index - 1; i >= 0; i--) ret += sym[digits[i]];
            return ret;
        }
        static private float ConvertCommaBaseToDecimal(string Number, int Base)
        {
            float n = 0f;
            for (int i = 0; i < Number.Length; i++)
            {
                if (Number[i] >= '0' && Number[i] <= '9') n += (Number[i] - 48) * (float)Math.Pow(Base, -(i + 1));
                else if (Number[i] >= 'A' && Number[i] <= 'Z') n += (Number[i] - 55) * (float)Math.Pow(Base, -(i + 1));
                else if (Number[i] >= 'a' && Number[i] <= 'z') n += (Number[i] - 87) * (float)Math.Pow(Base, -(i + 1));
            }
            return n;
        }
        static private string ConvertDecimalToCommaBase(float Number, int Base)
        {
            char[] sym = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string ret = "";
            int maxCommaDigits = 8;
            while (Number != 0 && maxCommaDigits > 0)
            {
                Number *= Base;
                int i = (int)Number;
                ret += sym[i];
                Number -= i;
                maxCommaDigits--;
            }
            return ret;
        }
    }
}
