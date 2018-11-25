using System;
using System.Collections.Generic;

public static class Calculus {

    public static float TakeDerivative(Func<float, float> f, float point)
    {
        const float delta = 0.00001f;
        float x1 = point + delta;
        float x2 = point - delta;
        float y1 = f(x1);
        float y2 = f(x2);
        return (y2 - y1) / (x2 - x1);
    }

    public static Func<float, float> LagrangeInterpolate(Dictionary<float, float> dataPoints)
    {
        List<Func<float, float>> summation = new List<Func<float, float>>();

        foreach (var dataPoint in dataPoints)
        {
            float input = dataPoint.Key;
            float output = dataPoint.Value;

            List<Func<float, float>> products = new List<Func<float, float>>();

            foreach (var otherDataPoints in dataPoints)
            {
                if (otherDataPoints.Key == dataPoint.Key)
                {
                    continue;
                }

                products.Add((x) =>
                {
                    return (x - otherDataPoints.Key) / (input - otherDataPoints.Key);
                });
            }

            summation.Add((x) =>
            {
                float mult = output;

                foreach(var p in products)
                {
                    mult *= p(x);
                }

                return mult;
            });
        }

        return (x) =>
        {
            float sum = 0;

            foreach (var l in summation)
            {
                sum += l(x);
            }

            return sum;
        };
    }
}
