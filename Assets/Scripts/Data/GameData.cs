using System.Collections.Generic;

namespace Data
{
    [System.Serializable]
    public struct GameData
    {
        public string someStringValue;
        public int someIntValue;
        public decimal someDecimalValue;
        public long someLongValue;
        public List<ShopItem> shopItems;
    }
}