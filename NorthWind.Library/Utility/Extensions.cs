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

        public static IQueryable<T> ApplyWhere<T>(this IQueryable<T> source, string propertyName, object propertyValue, ColumnTypeEnum propertyType, ExpressionOperationEnum operation) where T : class,new()
        {
            string proName = Utility.GetStandardPropertyName(new T(), propertyName);
            // 1. Retrieve member access expression
            var mba = PropertyAccessorCache<T>.Get(proName);
            if (mba == null) return source;

            //2.Try converting value to correct type
            object value;

            //cách kiểm tra xem có phải là kiểu nullable ko?
            var uType = Nullable.GetUnderlyingType(mba.ReturnType);
            string typeName = (uType != null) ? uType.Name : mba.ReturnType.Name;
            try
            {
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
            //convert
            
            var typeCompare = uType!=null? uType:mba.ReturnType;
            var body = Expression.Convert( mba.Body,typeCompare);
            var valExpress = Expression.Constant(value, typeCompare);

            
            // 3. Construct expression tree
            Expression eqe = null;
            if (operation == ExpressionOperationEnum.Equals) {
                 eqe = Expression.Equal(body, valExpress);
            }
            //lớn hơn
            else if (operation == ExpressionOperationEnum.GreatThan)
            {
                if (typeName.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                {
                    eqe = Expression.GreaterThan(body, Workaround_3361((DateTime)value));
                }
                else
                {
                    eqe = Expression.GreaterThan(body, valExpress);
                }
            }
            //lớn hơn hoặc bằng
            else if (operation == ExpressionOperationEnum.GreatThanEqual)
            {
                if (typeName.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                {
                    eqe = Expression.GreaterThanOrEqual(body, Workaround_3361((DateTime)value));
                }
                else
                {
                    eqe = Expression.GreaterThanOrEqual(body, valExpress);
                }
            }
            //nhỏ hơn
            else if (operation == ExpressionOperationEnum.LessThan)
            {
                if (typeName.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                {
                    eqe = Expression.LessThan(body, Workaround_3361((DateTime)value));
                }
                else
                {
                    eqe = Expression.LessThan(body, valExpress);
                }
            }
            //nhỏ hơn hoặc bằng
            else if (operation == ExpressionOperationEnum.LessThanEqual)
            {
                
                if (typeName.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                {
                    eqe = Expression.LessThanOrEqual(body, Workaround_3361((DateTime)value));
                }
                else
                {
                    eqe = Expression.LessThanOrEqual(body, valExpress);
                }
            }

            LambdaExpression expression = null;
            //chứa
            if (operation == ExpressionOperationEnum.Contains || operation == ExpressionOperationEnum.StartsWith|| operation == ExpressionOperationEnum.EndsWith)
            {
                string methodName = "Contains";
                if (operation == ExpressionOperationEnum.StartsWith)
                {
                    methodName = "StartsWith";
                }
                else if (operation == ExpressionOperationEnum.EndsWith)
                {
                    methodName = "EndsWith";
                }
                var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
                var valExpression = Expression.Constant(value, mba.ReturnType);
                var paramExp = Expression.Parameter(typeof(T), "x");

                var memberExpression = Expression.Property(paramExp, propertyName);
                var methodExp = Expression.Call(memberExpression, method, valExpression);
                expression = Expression.Lambda<Func<T, bool>>(methodExp, paramExp);

            }
            else
            {
                expression = Expression.Lambda(eqe, mba.Parameters[0]);
            }
            // 4. Construct new query
            MethodCallExpression resultExpression = Expression.Call(
                null,
               Utility.GetMethodInfo(Queryable.Where, source, (Expression<Func<T, bool>>)null),
                new Expression[] { source.Expression, Expression.Quote(expression) });

            return source.Provider.CreateQuery<T>(resultExpression);
        }
        private static Expression Workaround_3361(DateTime date)
        {
            Expression<Func<DateTime>> closure = () => date;
            return closure.Body;
        }


    }
}
