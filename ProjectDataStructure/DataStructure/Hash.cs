using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectDataStructure.Model;

namespace ProjectDataStructure
{
    internal class Hash<TKey, TValue>
    {
        LinkedList<Data>[] hashArr;
        const double LOAD_FACTOR = 1.3;
        int currentItemsCount;
        int maxItemCount;

        public Hash(int itemsCount = 1024)
        {
            hashArr = new LinkedList<Data>[(int)(itemsCount * LOAD_FACTOR)];
            maxItemCount = itemsCount;
        }

        public void Add(TKey key, TValue value)
        {
            int index = GetHashCode(key);
            if (hashArr[index] == null)
            {
                hashArr[index] = new LinkedList<Data>();
                hashArr[index].AddLast(new Data(key, value));
            }
            else
            {
                if (hashArr[index].Any((d) => d.key.Equals(key)))
                    throw new ArgumentException("Key exists");
                hashArr[index].AddLast(new Data(key, value));
            }
            currentItemsCount++;
            if (currentItemsCount > maxItemCount)
            {
                Rehash();
            }
        }


        public bool GetValue(TKey key, out TValue saveValue)
        {
            int ind = GetHashCode(key);
            if (hashArr[ind] != null)
            {
                Data data = hashArr[ind].FirstOrDefault((d) => d.key.Equals(key));
                if (data != null)
                {
                    saveValue = data.value;
                    return true;
                }
            }
            saveValue = default(TValue);
            return false;
        }

        private void Rehash()
        {
            LinkedList<Data>[] oldArr = hashArr;
            hashArr = new LinkedList<Data>[oldArr.Length * 2];
            maxItemCount *= 2;
            currentItemsCount = 0;

            foreach (var list in oldArr)
            {
                if (list != null)
                {
                    foreach (var data in list)
                    {
                        Add(data.key, data.value);
                    }
                }
            }
        }


        public bool Remove(TKey key, out TValue saveValue)
        {
            int ind = GetHashCode(key);
            if (hashArr[ind] != null)
            {
                Data data = hashArr[ind].FirstOrDefault((d) => d.key.Equals(key));
                if (data != null)
                {
                    saveValue = data.value;
                    hashArr[ind].Remove(data);
                    return true;
                }
            }
            saveValue = default(TValue);
            return false;
        }



        private int GetHashCode(TKey key)
        {
            return Math.Abs(key.GetHashCode() % hashArr.Length);
        }


        class Data
        {
            public TKey key;
            public TValue value;

            public Data(TKey key, TValue value)
            {
                this.value = value;
                this.key = key;
            }
        }


    }
}


