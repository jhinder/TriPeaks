using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TriPeaks.Test
{
    [TestClass]
    public class ViewModelInstatiationTests
    {
        [TestMethod]
        public void TestAboutDialogVM()
        {
            var advm = new AboutDialogViewModel();
            Assert.IsFalse(string.IsNullOrEmpty(advm.Copyright));
            Assert.IsFalse(string.IsNullOrEmpty(advm.Version));
        }

        [TestMethod]
        public void TestBackSelectDialogVM()
        {
            var bsvm = new BackSelectViewModel();
            int changes = 0;
            bsvm.PropertyChanged += (s, e) => changes++;
            int oldValue = bsvm.SelectedBack;
            bsvm.SelectedBack = 1;
            bsvm.Save();
            var secondBsvm = new BackSelectViewModel();
            Assert.AreEqual(bsvm.SelectedBack, secondBsvm.SelectedBack);
            Assert.AreEqual(1, changes);
            secondBsvm.SelectedBack = oldValue;

            bsvm.SelectedBack = -1;
            Assert.AreEqual(0, bsvm.SelectedBack);
            bsvm.SelectedBack = 10;
            Assert.AreEqual(0, bsvm.SelectedBack);
        }
    }
}
