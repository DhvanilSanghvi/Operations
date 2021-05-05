using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProseTutorial
{
    public static class Semantics
    {
        public static int Add(List<int> v, int op1, int op2)
        {
            return op1+op2;
        }

        public static int AddFinal(List<int> v, int op1, int op2)
        {
            return op1+op2;
        }

        public static int Mul(List<int> v, int op1, int op2)
        {
            return op1*op2;
        }
        public static int MulFinal(List<int> v, int op1, int op2)
        {
            return op1*op2;
        }

        public static double Div(List<int> v, int op1, int op2)
        {
            if(op2 == 0){
                return null;
            }
            if(op1 > op2){
                return (double)op1/(double)op2;
            }
            return null;
        }

        public static double DivFinal(List<int> v, int op1, int op2)
        {
            if(op2 == 0){
                return null;
            }
            if(op1 > op2){
                return (double)op1/(double)op2;
            }
            return null;
        } 

        public static int Element(List<int> v, int k)
        {
            if (k >= v.Count)   return null;
            return v[k];
        }

        public static int ElementFinal(List<int> v, int k)
        {
            if (k >= v.Count)   return null;
            return v[k];
        }

    }
}