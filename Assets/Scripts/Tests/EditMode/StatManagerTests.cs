using System;
using Droppy.StatSystem;
using IdeaToGame.PersistenceSystem;
using NUnit.Framework;
using UnityEngine.TestTools.Utils;

namespace Droppy.Tests.EditMode
{
    public class StatManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            Persistence.ClearAllData();
        }
        
        [TearDown]
        public void TearDown()
        {
            Persistence.ClearAllData();
        }
        
        // ─── Reset ──────────────────────────────────────────────────────────────────
        [Test]
        [TestCase(0.0f)]
        [TestCase(-10.0f)]
        [TestCase(10.0f)]
        [TestCase(25.3f)]
        [TestCase(-9.5f)]
        public void Reset_Stat_Modifier_Returns_Stat_Value_To_Default(float initialAmount)
        {
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);

            // Makes sure the value gets initialized as the default
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(initialAmount).Using(FloatEqualityComparer.Instance));
            
            // Sets to -100 to make the value different from the default
            const float statTestValue = -100.0f;
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, statTestValue));
            
            statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(statTestValue).Using(FloatEqualityComparer.Instance));
            
            // Resets the value
            StatManager.Modify(stat, new StatModifier(StatModifierType.Reset));
            statValue = StatManager.Read(stat);

            Assert.That(statValue, Is.EqualTo(initialAmount).Using(FloatEqualityComparer.Instance));
        }
        
        [Test]
        public void Reset_Without_Prior_Modification_Returns_Initial_Amount()
        {
            const float initialAmount = 42.0f;
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Reset));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(initialAmount).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Reset_After_Multiple_Modifications_Returns_Initial_Amount()
        {
            const float initialAmount = 10.0f;
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, 5.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Multiply, 3.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, 999.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Reset));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(initialAmount).Using(FloatEqualityComparer.Instance));
        }
 
        // ─── Set ──────────────────────────────────────────────────────────────────
 
        [Test]
        [TestCase(0.0f, 50.0f)]
        [TestCase(10.0f, -10.0f)]
        [TestCase(-5.0f, 0.0f)]
        [TestCase(100.0f, 100.0f)]
        [TestCase(7.7f, 3.3f)]
        public void Set_Stat_Modifier_Overrides_Current_Value(float initialAmount, float setValue)
        {
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, setValue));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(setValue).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Set_Stat_Modifier_Is_Idempotent_When_Applied_Twice()
        {
            Stat stat = Stat.Create("TestStat", "TestStat", 0.0f);
            const float setValue = 77.0f;
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, setValue));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, setValue));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(setValue).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Set_Stat_Modifier_Last_Call_Wins_When_Applied_Multiple_Times()
        {
            Stat stat = Stat.Create("TestStat", "TestStat", 0.0f);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, 10.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, 20.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, 30.0f));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(30.0f).Using(FloatEqualityComparer.Instance));
        }
 
        // ─── Add ──────────────────────────────────────────────────────────────────
 
        [Test]
        [TestCase(0.0f, 10.0f, 10.0f)]
        [TestCase(10.0f, 5.0f, 15.0f)]
        [TestCase(10.0f, -5.0f, 5.0f)]
        [TestCase(-10.0f, -10.0f, -20.0f)]
        [TestCase(7.5f, 2.5f, 10.0f)]
        public void Add_Stat_Modifier_Sums_Amount_To_Current_Value(float initialAmount, float addAmount, float expected)
        {
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, addAmount));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(expected).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Add_Stat_Modifier_Accumulates_Across_Multiple_Calls()
        {
            Stat stat = Stat.Create("TestStat", "TestStat", 0.0f);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, 10.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, 20.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, 30.0f));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(60.0f).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Add_Zero_Does_Not_Change_Current_Value()
        {
            const float initialAmount = 50.0f;
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, 0.0f));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(initialAmount).Using(FloatEqualityComparer.Instance));
        }
 
        // ─── Multiply ─────────────────────────────────────────────────────────────
 
        [Test]
        [TestCase(10.0f, 2.0f, 20.0f)]
        [TestCase(10.0f, 0.5f, 5.0f)]
        [TestCase(10.0f, -1.0f, -10.0f)]
        [TestCase(-10.0f, -1.0f, 10.0f)]
        [TestCase(7.0f, 3.0f, 21.0f)]
        public void Multiply_Stat_Modifier_Scales_Current_Value(
            float initialAmount, float multiplyAmount, float expected)
        {
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Multiply, multiplyAmount));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(expected).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Multiply_By_Zero_Results_In_Zero()
        {
            Stat stat = Stat.Create("TestStat", "TestStat", 99.0f);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Multiply, 0.0f));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(0.0f).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Multiply_By_One_Does_Not_Change_Current_Value()
        {
            const float initialAmount = 42.0f;
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Multiply, 1.0f));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(initialAmount).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Multiply_Accumulates_Across_Multiple_Calls()
        {
            Stat stat = Stat.Create("TestStat", "TestStat", 2.0f);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Multiply, 3.0f)); // 6
            StatManager.Modify(stat, new StatModifier(StatModifierType.Multiply, 2.0f)); // 12
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(12.0f).Using(FloatEqualityComparer.Instance));
        }
 
        // ─── OnStatModified event ─────────────────────────────────────────────────
 
        [Test]
        public void Modify_Fires_OnStatModified_With_Correct_Stat_ID()
        {
            Stat stat = Stat.Create("EventStat", "EventStat", 0.0f);
 
            string receivedId = null;
            Action<string> onStatModified = id => receivedId = id;
            
            StatManager.OnStatModified += onStatModified;
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, 1.0f));
 
            Assert.That(receivedId, Is.EqualTo(stat.ID));
 
            StatManager.OnStatModified -= onStatModified;
        }
 
        // ─── Isolation between different Stats ────────────────────────────────────
 
        [Test]
        public void Modifying_One_Stat_Does_Not_Affect_Another_Stat()
        {
            Stat statA = Stat.Create("StatA", "StatA", 10.0f);
            Stat statB = Stat.Create("StatB", "StatB", 20.0f);
 
            StatManager.Modify(statA, new StatModifier(StatModifierType.Set, 99.0f));
 
            float valueB = StatManager.Read(statB);
            Assert.That(valueB, Is.EqualTo(20.0f).Using(FloatEqualityComparer.Instance));
        }
 
        // ─── Combination scenarios ────────────────────────────────────────────────
 
        [Test]
        public void Set_Then_Add_Then_Multiply_Produces_Correct_Result()
        {
            // set(10) + add(5) = 15 * multiply(2) = 30
            Stat stat = Stat.Create("TestStat", "TestStat", 0.0f);
 
            StatManager.Modify(stat, new StatModifier(StatModifierType.Set, 10.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Add, 5.0f));
            StatManager.Modify(stat, new StatModifier(StatModifierType.Multiply, 2.0f));
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(30.0f).Using(FloatEqualityComparer.Instance));
        }
 
        [Test]
        public void Read_Without_Any_Modification_Returns_Initial_Amount()
        {
            const float initialAmount = 55.5f;
            Stat stat = Stat.Create("TestStat", "TestStat", initialAmount);
 
            float statValue = StatManager.Read(stat);
            Assert.That(statValue, Is.EqualTo(initialAmount).Using(FloatEqualityComparer.Instance));
        }
    }
}