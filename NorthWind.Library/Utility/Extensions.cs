using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static NorthWind.Library.Enumeration;
/// <summary>
/// class chứa các extensions
/// </summary>
/// created by: ntkien 31.05.2020
namespace NorthWind.Library
{
    public static class Extensions
    {
        /// <summary>
        /// extension sắp xếp theo thứ tự tăng dần
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// created by: ntkien 01.06.2020
        public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> source, string propertyName) where T : class
        {
            var expression = PropertyAccessorCache<T>.Get(propertyName);
            if (expression == null) return source;

            MethodCallExpression resultExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.OrderBy),
                new Type[] { typeof(T), expression.ReturnType },
                source.Expression,
                Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        /// <summary>
        /// extension sắp xếp theo thứ tự tăng dần nhiều trường
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// created by: ntkien 01.06.2020
        public static IQueryable<T> ApplyThenBy<T>(this IQueryable<T> source, string propertyName) where T : class
        {
            var expression = PropertyAccessorCache<T>.Get(propertyName);
            if (expression == null) return source;

            MethodCallExpression resultExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.ThenBy),
                new Type[] { typeof(T), expression.ReturnType },
                source.Expression,
                Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }


        /// <summary>
        /// extension sắp xếp theo thứ tự giảm dần
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// created by: ntkien 01.06.2020
        public static IQueryable<T> ApplyOrderByDesc<T>(this IQueryable<T> source, string propertyName) where T : class
        {
            var expression = PropertyAccessorCache<T>.Get(propertyName);
            if (expression == null) return source;

            MethodCallExpression resultExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.OrderByDescending),
                new Type[] { typeof(T), expression.ReturnType },
                source.Expression,
                Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        /// <summary>
        /// extension sắp xếp theo thứ tự tăng dần nhiều trường
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// created by: ntkien 01.06.2020
        public static IQueryable<T> ApplyThenByDesc<T>(this IQueryable<T> source, string propertyName) where T : class
        {
            var expression = PropertyAccessorCache<T>.Get(propertyName);
            if (expression == null) return source;

            MethodCallExpression resultExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.ThenByDescending),
                new Type[] { typeof(T), expression.ReturnType },
                source.Expression,
                Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        public static IQueryable<T> ApplyWhere<T>(this IQueryable<T> source, string propertyName, string propertyValue, ColumnTypeEnum propertyType, ExpressionOperationEnum operation) where T : class
        {
            // 1. Retrieve member access expression
            var mba = PropertyAccessorCache<T>.Get(propertyName);
            if (mba == null) return source;
            
            //2.Try converting value to correct type
            object value;
            try
            {
                //cách kiểm tra xem có phải là kiểu nullable ko?
                var uType = Nullable.GetUnderlyingType(mba.ReturnType);
                string typeName = uType != null ? uType.Name : mba.ReturnType.Name;
                value = Utility.ConvertValueByType(propertyValue, typeName);
            }
            catch (SystemException ex) when (
                ex is InvalidCastException ||
                ex is FormatException ||
                ex is OverflowException ||
                ex is ArgumentNullException)
            {
                return source;
            }

            // 3. Construct expression tree
            
            Expression eqe = Expression.Equal(
                mba.Body,
                Expression.Constant(value, mba.ReturnType));
            if (operation == ExpressionOperationEnum.Equals)
            {
                eqe = Expression.Equal(
                mba.Body,
                Expression.Constant(value, mba.ReturnType));

            }
            else if (operation == ExpressionOperationEnum.GreatThan)
            {
                eqe = Expression.GreaterThan(
                mba.Body,
                Expression.Constant(value, mba.ReturnType));
            }
            else if (operation == ExpressionOperationEnum.GreatThanEqual)
            {
                eqe = Expression.GreaterThanOrEqual(
                mba.Body,
                Expression.Constant(value, mba.ReturnType));
            }
            else if (operation == ExpressionOperationEnum.LessThan)
            {
                eqe = Expression.LessThan(
                mba.Body,
                Expression.Constant(propertyValue, mba.ReturnType));
            }
            else if (operation == ExpressionOperationEnum.LessThanEqual)
            {
                eqe = Expression.LessThanOrEqual(
                mba.Body,
                Expression.Constant(value, mba.ReturnType));
            }
            
            var expression = Expression.Lambda(eqe, mba.Parameters[0]);

            // 4. Construct new query
            MethodCallExpression resultExpression = Expression.Call(
                null,
               Utility.GetMethodInfo(Queryable.Where, source, (Expression<Func<T, bool>>)null),
                new Expression[] { source.Expression, Expression.Quote(expression) });
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        
    }
}
