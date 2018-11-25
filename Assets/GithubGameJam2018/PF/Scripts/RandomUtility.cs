using System;

public static class RandomUtility {

    static Random random;

    public static float RandFloat(float min, float max)
    {
        if(random == null) { random = new Random(); }

        float f = (float)(random.NextDouble());

        return f * (max - min) + min;
    }
}
