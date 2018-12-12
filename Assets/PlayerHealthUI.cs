using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour {

    public Damageable player;
    public TextMeshProUGUI text;

	void Start () {

        player.OnDamaged += () =>
        {
            text.text = "Health\n" + player.currentHealth;
        };

        text.text = "Health\n" + player.currentHealth;
    }
}
