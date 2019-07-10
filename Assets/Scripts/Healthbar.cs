using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HealthBar of enemies
/// </summary>
public class Healthbar : MonoBehaviour
{
    // links to GameObjects used to draw Healthbar
    [SerializeField] 
    private Image progress;
    [SerializeField] 
    private Image background;
    [SerializeField]
    private Text text;

    private int maxHealth = 1;

    /// <summary>
    /// Linked to EnemyHit event; updates healthbar
    /// </summary>
    public void EnemyHit(object sender, EnemyHitEventArgs args)
    {
        UpdateHealth(args.Health);
    }

    /// <summary>
    /// Sets the maximum value of health to be shown on Healthbar
    /// </summary>
    /// <param name="max">new maximum value</param>
    /// <param name="setHealthToMax">determines whether to set current drawn health to the new maximum</param>
    public void SetMax(int max, bool setHealthToMax = true)
    {
        maxHealth = max;
        if (setHealthToMax)
            UpdateHealth(maxHealth);
    }

    /// <summary>
    /// Sets the new value of health to be drawn
    /// </summary>
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
