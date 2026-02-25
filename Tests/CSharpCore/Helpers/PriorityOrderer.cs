using DotNetSnmp.Test.Helpers.XUnit.Project.Attributes;
using Xunit.Sdk;
using Xunit.v3;

namespace XUnit.Project.Orderers
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(
            IReadOnlyCollection<TTestCase> testCases) where TTestCase : notnull, ITestCase
        {
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();
            foreach (TTestCase testCase in testCases)
            {
                int priority = 0;
                if (testCase is IXunitTestCase xunitTestCase)
                {
                    priority = xunitTestCase.TestMethod.Method
                        .GetCustomAttributes(typeof(TestPriorityAttribute), true)
                        .OfType<TestPriorityAttribute>()
                        .FirstOrDefault()
                        ?.Priority ?? 0;
                }

                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            return sortedMethods.Keys.SelectMany(
                    priority => sortedMethods[priority].OrderBy(
                        testCase => testCase.TestMethod?.MethodName))
                .ToArray();
        }

        private static TValue GetOrCreate<TKey, TValue>(
            IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : struct
            where TValue : new() =>
            dictionary.TryGetValue(key, out TValue? result)
                ? result
                : (dictionary[key] = new TValue());
    }
}
