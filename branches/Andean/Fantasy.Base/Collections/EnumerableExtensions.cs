using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy
{
    public static class EnumerableExtensions
    {
        public static TSource Single<TSource>(this IEnumerable<TSource> source, string errorMessage)
        {

            try
            {
                return source.Single();
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage, error);
            }

        }

        public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string errorMessage)
        {
            try
            {
                return source.Single(predicate);
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage, error);
            }
        }

        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string errorMessage)
        {
            try
            {
                return source.SingleOrDefault(predicate);
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage, error);
            }
        }

        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, string errorMessage)
        {
            try
            {
                return source.SingleOrDefault();
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage,error)  ;
            }
        }


        public static TSource First<TSource>(this IEnumerable<TSource> source, string errorMessage)
        {

            try
            {
                return source.First();
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage, error);
            }

        }

        public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string errorMessage)
        {
            try
            {
                return source.First(predicate);
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage, error);
            }
        }

        //public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string errorMessage)
        //{
        //    try
        //    {
        //        return source.FirstOrDefault(predicate);
        //    }
        //    catch (InvalidOperationException error)
        //    {

        //        throw new InvalidOperationException(errorMessage, error);
        //    }
        //}

        //public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, string errorMessage)
        //{
        //    try
        //    {
        //        return source.FirstOrDefault();
        //    }
        //    catch (InvalidOperationException error)
        //    {

        //        throw new InvalidOperationException(errorMessage, error);
        //    }
        //}


        public static TSource Last<TSource>(this IEnumerable<TSource> source, string errorMessage)
        {

            try
            {
                return source.Last();
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage, error);
            }

        }

        public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string errorMessage)
        {
            try
            {
                return source.Last(predicate);
            }
            catch (InvalidOperationException error)
            {

                throw new InvalidOperationException(errorMessage, error);
            }
        }

        //public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string errorMessage)
        //{
        //    try
        //    {
        //        return source.LastOrDefault(predicate);
        //    }
        //    catch (InvalidOperationException error)
        //    {

        //        throw new InvalidOperationException(errorMessage, error);
        //    }
        //}

        //public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, string errorMessage)
        //{
        //    try
        //    {
        //        return source.LastOrDefault();
        //    }
        //    catch (InvalidOperationException error)
        //    {

        //        throw new InvalidOperationException(errorMessage, error);
        //    }
        //}


    }
}
