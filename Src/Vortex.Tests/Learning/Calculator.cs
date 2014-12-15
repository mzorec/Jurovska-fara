using System;

namespace Vortex.Tests.Learning
{
    public class Calculator : ICalculator
    {
        public int Calculate(string a)
        {
            if (a.Contains("+"))
            {
                string[] delimiter = new string[] { "+" };
                var content = a.Split(delimiter, StringSplitOptions.None);
                var result = 0;
                for (int i = 0; i < content.Length; i++)
                {
                    result = result + int.Parse(content[i]);
                }
                ////string firstt = content[0];
                ////string secondt = content[1];
                ////int first = int.Parse(firstt);
                ////int second = int.Parse(secondt);
                ////int result = first + second;
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
