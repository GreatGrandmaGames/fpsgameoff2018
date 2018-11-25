using UnityEngine;

public static class GeometryUtility {

    public enum TopType
    {
        Pointy,
        Flat
    }

    public static Vector2[] PointsAboutEllipse(int numIncrements, TopType topType, float verticalSquash = 1f)
    {
        var corners = new Vector2[numIncrements];

        float aOffset = 0;

        if(topType == TopType.Flat)
        {
            aOffset = 0.5f;
        }

        for (int inc = 0; inc < numIncrements; inc++)
        {
            float theta = ((2f * Mathf.PI) * ((inc + aOffset)/ (float)numIncrements));

            //loat radius = (1 - eccentricity * eccentricity) / (1 + eccentricity * Mathf.Cos(theta));

            corners[inc] = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta) * verticalSquash);
        }

        return corners;
    }
}
