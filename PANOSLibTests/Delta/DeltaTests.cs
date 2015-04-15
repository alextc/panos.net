namespace PANOSLibTest.Delta
{
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DeltaTests
    {
        [TestMethod]
        public void Array1HasMoreMembersThanArray2()
        {
            string[] array1 = { "2.0", "2.1", "2.2", "2.3" };
            string[] array2 = { "2.2" };
            var areDifferent = array1.Except(array2).Any();
            Assert.IsTrue(areDifferent);
            var delta = array1.Except(array2);
            foreach (var d in delta)
            {
                // This should print  "2.0", "2.1", "2.3"
                Debug.WriteLine(d);
            }
        }

        [TestMethod]
        public void Array1HasLessMembersThanArray2()
        {
            string[] array1 = { "2.2" };
            string[] array2 = { "2.0", "2.1", "2.2", "2.3" };
            var areDifferent = array1.Except(array2).Any();
            Assert.IsFalse(areDifferent);

            areDifferent = array2.Except(array1).Any();
            Assert.IsTrue(areDifferent);

            var delta = array2.Except(array1);
            foreach (var d in delta)
            {
                // This should print  "2.0", "2.1", "2.3"
                Debug.WriteLine(d);
            }
        }

        [TestMethod]
        public void Array1HasTheSameMembersAsArray2()
        {
            string[] array1 = { "2.0", "2.1", "2.2", "2.3" };
            string[] array2 = { "2.0", "2.1", "2.2", "2.3" };
            var areDifferent = array1.Except(array2).Any();
            Assert.IsFalse(areDifferent);
        }

        [TestMethod]
        public void Array1HasTheSameMembersAsArray2ButInDifferentOrder()
        {
            string[] array1 = { "2.3", "2.1", "2.2", "2.0" };
            string[] array2 = { "2.0", "2.1", "2.2", "2.3" };
            var areDifferent = array1.Except(array2).Any();
            Assert.IsFalse(areDifferent);
        }
    }
}
