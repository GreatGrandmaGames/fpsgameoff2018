
using System;

public static class PFRandomNames {

    static Random random;

    public static string[] adjectives = new string[]
    {
        "Noisy",
        "Shooty",
        "Heavy",
        "Mini",
        "Bangy",
        "Delicious",
        "Rapid",
        "Chaotic",
        "Red",
        "Revolving",
        "Rooty Tooty"
    };

    public static string[] nouns = new string[]
    {
        "Cricket",
        "Cuite",
        "Shooty Boi",
        "Shooty Gal",
        "BangBang",
        "Spider",
        "Nightman",
        "Peacekeeper",
        "Cutie",
        "Revolver",
        "Pointy Shooty"
    };

    public static string GenerateName()
    {
       if(random == null) { random = new Random(); }

        return string.Format("{0} {1}", adjectives[random.Next(adjectives.Length)], nouns[random.Next(nouns.Length)]);
    }
}
