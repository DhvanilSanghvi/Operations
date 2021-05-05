using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProseTutorial
{
    public static class Semantics
    {
        public static uint? Add(List<int> v, uint? op1, uint? op2)
        {
            return op1+op2;
        }

        public static uint? AddFinal(List<int> v, uint? op1, uint? op2)
        {
            return op1+op2;
        }

        public static uint? Mul(List<int> v, uint? op1, uint? op2)
        {
            return op1*op2;
        }
        public static uint? MulFinal(List<int> v, uint? op1, uint? op2)
        {
            return op1*op2;
        }

        public static double Div(List<int> v, uint? op1, uint? op2)
        {
            if(op2 == 0){
                return null;
            }
            if(op1 > op2){
                return (double)op1/(double)op2;
            }
            return null;
        }

        public static double DivFinal(List<int> v, uint? op1, uint? op2)
        {
            if(op2 == 0){
                return null;
            }
            if(op1 > op2){
                return (double)op1/(double)op2;
            }
            return null;
        }

        public static uint? Element(List<int> v, int k)
        {
            if (k >= v.Count)   return null;
            return v.Keys.ToList()[k];
        }

        public static uint? ElementFinal(List<int> v, int k)
        {
            if (k >= v.Count)   return null;
            return v.Keys.ToList()[k];
        }

    }
}