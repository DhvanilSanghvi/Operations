using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProseTutorial
{
    public static class Semantics
    {
        public static uint? Add(List<uint?> v, uint? op1, uint? op2)
        {
            return (uint?)op1+op2;
        }

        public static uint? Mul(List<uint?> v, uint? op1, uint? op2)
        {
            return op1*op2;
        }

        public static uint? Div(List<uint?> v, uint? op1, uint? op2)
        {
            if(op2 == 0){
                return null;
            }
            if(op1 > op2){
                return (uint?)((double)op1/(double)op2);
            }
            return null;
        }

        public static uint? Element(List<uint?> v, int k)
        {
            if (k >= v.Count)   return null;
            return v[k];
        }

    }
}