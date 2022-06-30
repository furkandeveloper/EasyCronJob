using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCronJob.Abstractions
{
    public static class GenericExtensions
    {
        public static bool IsNull<T>(this T obj) where T : class
        {
            if (obj is string)
            {
                return (obj as string).IsNullOrEmpty();
            }
            return obj == null;
        }
        public static void IsNull<T>(this T obj, Action<T> Act) where T : class
        {
            if (obj is string)
            {
                if ((obj as string).IsNullOrEmpty())
                {
                    Act(obj);
                }
            }

            if (obj == null)
            {
                Act(obj);
            }
        }
        public static bool IsNotNull<T>(this T obj)
        {
            if (obj is string)
            {
                return !(obj as string).IsNullOrEmpty();
            }
            return obj != null;
        }
        public static void IsNotNull<T>(this T obj, Action<T> Act)
        {
            if (obj is string)
            {
                if (!(obj as string).IsNullOrEmpty())
                {
                    Act(obj);
                }
            }

            if (obj != null)
            {
                Act(obj);
            }
        }
        public static TResult IsNotNull<T, TResult>(this T obj, Func<T,TResult> Act)
        {
            try
            {
                if (obj is string)
                {
                    return !(obj as string).IsNullOrEmpty() ? Act(obj) : default; 
                }

                return obj.IsNotNull() ? Act(obj) : default;
            }
            catch (Exception)
            {
                return default;
            }           
        }

        public static void Do(this Boolean val, Action action)
        {
            if (val)
            {
                action();
            }
        }
    }
}
