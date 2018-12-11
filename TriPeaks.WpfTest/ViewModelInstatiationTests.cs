using Xunit;

namespace TriPeaks.Test
{
    public class ViewModelInstatiationTests
    {
        [Fact]
        public void TestAboutDialogVM()
        {
            var advm = new AboutDialogViewModel();
            Assert.False(string.IsNullOrEmpty(advm.Copyright));
            Assert.False(string.IsNullOrEmpty(advm.Version));
        }

        [Fact]
        public void TestBackSelectDialogVM()
        {
            var bsvm = new BackSelectViewModel();
            int changes = 0;
            bsvm.PropertyChanged += (s, e) => changes++;
            int oldValue = bsvm.SelectedBack;
            bsvm.SelectedBack = 1;
            bsvm.Save();
            var secondBsvm = new BackSelectViewModel();
            Assert.Equal(bsvm.SelectedBack, secondBsvm.SelectedBack);
            Assert.Equal(1, changes);
            secondBsvm.SelectedBack = oldValue;

            bsvm.SelectedBack = -1;
            Assert.Equal(0, bsvm.SelectedBack);
            bsvm.SelectedBack = 10;
            Assert.Equal(0, bsvm.SelectedBack);
        }
    }
}
