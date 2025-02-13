using NUnit.Framework;
using ObservableVariables.BasicTypes;

namespace ObservableVariables.EditorTests
{
    public class BasicTypes
    {
        [Test]
        public void FloatValueChangedEvent()
        {
            const float targetValue = 5.3f;
            ObservableFloat floatVar = new ObservableFloat(0);
            bool hasChanged = false;
            floatVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            floatVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void IntValueChangedEvent()
        {
            const int targetValue = 3;
            ObservableInt intVar = new ObservableInt(0);
            bool hasChanged = false;
            intVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            intVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void BoolValueChangedEvent()
        {
            const bool targetValue = true;
            ObservableBool boolVar = new ObservableBool(false);
            bool hasChanged = false;
            boolVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            boolVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void FloatValueSet()
        {
            const float targetValue = 5.3f;
            ObservableFloat floatVar = new ObservableFloat(0);

            floatVar.Value = targetValue;

            Assert.IsTrue(floatVar.Value == targetValue);
        }
        
        [Test]
        public void IntValueSet()
        {
            const int targetValue = 33;
            ObservableInt intVar = new ObservableInt(0);

            intVar.Value = targetValue;

            Assert.IsTrue(intVar.Value == targetValue);
        }
        
        [Test]
        public void BoolValueSet()
        {
            const bool targetValue = true;
            ObservableBool boolVar = new ObservableBool(false);

            boolVar.Value = targetValue;

            Assert.IsTrue(boolVar.Value == targetValue);
        }
        
        [Test]
        public void FloatValueCast()
        {
            const float targetValue = 5.3f;
            ObservableFloat floatVar = targetValue;
            Assert.IsTrue(floatVar.Value == targetValue);
        }
        
        [Test]
        public void IntValueCast()
        {
            const int targetValue = 33;
            ObservableInt intVar = targetValue;
            Assert.IsTrue(intVar.Value == targetValue);
        }
        
        [Test]
        public void BoolValueCast()
        {
            const bool targetValue = true;
            ObservableBool boolVar = targetValue;
            Assert.IsTrue(boolVar.Value == targetValue);
        }
    }
}