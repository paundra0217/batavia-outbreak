using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] private int currency;

    public void AddCurrency(int amount)
    {
        currency += amount;
    }

    public void DeductCurrency(int amount)
    {
        currency -= amount;
    }

    public int GetCurrency()
    {
        return currency;
    }
}
