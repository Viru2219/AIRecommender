using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.Recommenders
{
    public interface IRecommender
    {
        double GetCorrelation(List<int> baseData, List<int> otherData);
    }

    public class Recommender : IRecommender
    {
        public double GetCorrelation(List<int> baseData1, List<int> otherData1)
        {
            List<int> baseData = new List<int>(baseData1);
            List<int> otherData = new List<int>(otherData1);
            if (otherData.Count == 0 || baseData.Count == 0)
            {
                throw new Exception("The array is null");
            }
            for (int i=0;i<baseData.Count; i++)
            {
                if (otherData.Count < baseData.Count)
                {
                    while (otherData.Count < baseData.Count)
                        otherData.Add(0);
                }
                else if (baseData.Count < otherData.Count)
                {
                    while (baseData.Count < otherData.Count)
                        baseData.Add(0);
                }
                if (baseData[i] == 0)
                {
                    baseData[i] = 1;
                    otherData[i] += 1;
                }
            }
            for (int i=0; i<otherData.Count; i++)
            {
                if (otherData.Count < baseData.Count)
                {
                    while (otherData.Count < baseData.Count)
                        otherData.Add(0);
                }
                else if (baseData.Count < otherData.Count)
                {
                    while (baseData.Count < otherData.Count)
                        baseData.Add(0);
                }
                if (otherData[i] == 0)
                {
                    otherData[i] = 1;
                    baseData[i] += 1;
                }
            }
            if (baseData.Count > otherData.Count)
            {
                int excess = baseData.Count - otherData.Count;
                //int a = otherData.Count;
                int x = baseData.Count - otherData.Count;
                for (int i = 0; i < x; i++)
                    otherData.Add(1);
            }
            if (otherData.Count > baseData.Count)
            {
                int x = otherData.Count - baseData.Count;
                otherData.RemoveRange(baseData.Count, x);
            }

            int N = baseData.Count;
            int sumX = baseData.Sum();
            int sumY = otherData.Sum();
            int sumXY = 0;
            int sumX2 = 0;
            int sumY2 = 0;
            for (int i=0; i<N; i++)
            {
                sumXY += baseData[i]*otherData[i];
                sumX2 = baseData[i]*baseData[i];
                sumY2 = otherData[i] * otherData[i];
            }
            double r = (N*sumXY-sumX*sumY)/Math.Sqrt((N*sumX2-sumX*sumX)*(N*sumY2-sumY*sumY));
            return r;
        }
    }
}
