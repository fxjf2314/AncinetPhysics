using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probability
{
   public static double CalculatePFromCValue(double cValue)
    {
        double preProbability = 0.0;
        double averageCount = 0.0;
        int maxN =(int)Math.Ceiling(1.0/cValue);
        for (int i = 1; i <= maxN; i++)
        {
            double curProbability = Math.Min(1.0, i * cValue) * (1 - preProbability);
            preProbability += curProbability;
            averageCount += curProbability * 1;
        }
        return 1.0 / averageCount;
    }
    public static double CalculateCValue(double p, double error = 0.00005)
    {
        if (p == 0) return 0.0;
        double right = p;double left = 0.0;
        while (true)
        {
            double mid = (right + left) / 2.0;
            double testedRate = CalculatePFromCValue(mid);
            if(Math.Abs(testedRate-p)<=error) return mid;
            if (testedRate>p)
                right = mid;
            else left = mid;
        }
    }
}
