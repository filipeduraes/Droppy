using System;
using System.Collections.Generic;

namespace Droppy.Shared
{
    public static class ListUtils 
    {
        public static void Resize<T>(this List<T> list, int newSize, Func<T> createDefaultElement)
        {
            if (list.Count == newSize)
            {
                return;
            }

            if (list.Count > newSize)
            {
                for (int i = 0; i < list.Count - newSize; i++)
                {
                    list.RemoveAt(list.Count - 1);
                }
            }
            else if (list.Count < newSize)
            {
                for (int i = 0; i < newSize - list.Count; i++)
                {
                    list.Add(createDefaultElement());
                }
            }
        }
    }
}