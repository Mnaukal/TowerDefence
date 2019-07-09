using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image progress;
    public Image background;
    public Text text;

    private int maxHealth = 1;

    public void EnemyHit(object sender, EnemyHitEventArgs args)
    {
        UpdateHealth(args.Health);
    }

    public void SetMax(int max, bool setHealthToMax = true)
    {
        maxHealth = max;
        if (setHealthToMax)
            UpdateHealth(maxHealth);
    }

    public void UpdateHealth(int health)
    {
        if (health > maxHealth)
            throw new System.ArgumentException("Health can't be bigger than max health (use SetMax to set it)");

        // sets the right edge of the progress bar of health 
        progress.rectTransform.offsetMax = new Vector2(
            -(maxHealth - health) * background.rectTransform.rect.width / maxHealth,
            progress.rectTransform.offsetMax.y);

        text.text = health + "/" + maxHealth;
    }    
}
