using UnityEngine;

namespace NMDIOC.LaneRunner
{
    public class CoinManager : MonoBehaviour
    {
        [Header("Data")]
        public int totalCoins = 0;

        void OnEnable()
        {
            Coin.OnCoinCollected += AddCoin;
        }

        void OnDisable()
        {
            Coin.OnCoinCollected -= AddCoin;
        }

        void AddCoin()
        {
            totalCoins++;
            Debug.Log("¡Moneda recolectada! Total actual: " + totalCoins);
        }
    }
}